# 🚀 Guide de Déploiement en Production

Ce guide couvre le déploiement du système d'automatisation sur différentes plateformes cloud.

## Table des matières

1. [Prérequis de production](#prérequis-de-production)
2. [Déploiement sur Azure](#déploiement-sur-azure)
3. [Déploiement sur AWS](#déploiement-sur-aws)
4. [Configuration de production](#configuration-de-production)
5. [Monitoring et maintenance](#monitoring-et-maintenance)

---

## 1. Prérequis de production

### Checklist de sécurité

- [ ] Changer toutes les clés secrètes
- [ ] Utiliser Azure Key Vault ou AWS Secrets Manager
- [ ] Configurer HTTPS avec certificat SSL valide
- [ ] Activer l'authentification forte (Azure AD / OAuth2)
- [ ] Configurer le pare-feu et les règles réseau
- [ ] Mettre en place la sauvegarde automatique de la base de données
- [ ] Configurer le monitoring et les alertes
- [ ] Documenter les procédures d'urgence

### Variables d'environnement requises

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__SqlServer=<connection-string-sécurisée>
Jwt__Key=<clé-jwt-32-chars-minimum>
Email__Username=<email-production>
Email__Password=<password-from-keyvault>
Twilio__AccountSid=<from-keyvault>
Twilio__AuthToken=<from-keyvault>
Chime__WebhookUrl=<webhook-production>
Cortex__ApiKey=<from-keyvault>
```

---

## 2. Déploiement sur Azure

### Option A : Azure App Service + Azure SQL

#### Étape 1 : Créer les ressources Azure

```bash
# Se connecter à Azure
az login

# Créer un groupe de ressources
az group create --name rg-automation-prod --location westeurope

# Créer Azure SQL Database
az sql server create \
  --name sql-automation-prod \
  --resource-group rg-automation-prod \
  --location westeurope \
  --admin-user sqladmin \
  --admin-password <VotreMdpFort123!>

az sql db create \
  --resource-group rg-automation-prod \
  --server sql-automation-prod \
  --name AutomationSystem \
  --service-objective S1

# Créer un App Service Plan
az appservice plan create \
  --name plan-automation-prod \
  --resource-group rg-automation-prod \
  --sku P1V2 \
  --is-linux

# Créer le Web App pour le backend
az webapp create \
  --resource-group rg-automation-prod \
  --plan plan-automation-prod \
  --name app-automation-backend-prod \
  --runtime "DOTNETCORE:8.0"

# Créer le Web App pour le frontend
az webapp create \
  --resource-group rg-automation-prod \
  --plan plan-automation-prod \
  --name app-automation-frontend-prod \
  --runtime "NODE:20-lts"
```

#### Étape 2 : Configurer Azure Key Vault

```bash
# Créer le Key Vault
az keyvault create \
  --name kv-automation-prod \
  --resource-group rg-automation-prod \
  --location westeurope

# Ajouter les secrets
az keyvault secret set --vault-name kv-automation-prod --name "Email--Password" --value "<votre-password>"
az keyvault secret set --vault-name kv-automation-prod --name "Twilio--AccountSid" --value "<votre-sid>"
az keyvault secret set --vault-name kv-automation-prod --name "Twilio--AuthToken" --value "<votre-token>"
az keyvault secret set --vault-name kv-automation-prod --name "Cortex--ApiKey" --value "<votre-key>"

# Activer Managed Identity pour le Web App
az webapp identity assign \
  --resource-group rg-automation-prod \
  --name app-automation-backend-prod

# Donner les permissions au Web App
PRINCIPAL_ID=$(az webapp identity show --resource-group rg-automation-prod --name app-automation-backend-prod --query principalId -o tsv)

az keyvault set-policy \
  --name kv-automation-prod \
  --object-id $PRINCIPAL_ID \
  --secret-permissions get list
```

#### Étape 3 : Déployer le backend

```bash
cd Backend/AutomationSystem.API

# Publier l'application
dotnet publish -c Release -o ./publish

# Créer un fichier zip
cd publish
zip -r ../deploy.zip *
cd ..

# Déployer sur Azure
az webapp deployment source config-zip \
  --resource-group rg-automation-prod \
  --name app-automation-backend-prod \
  --src deploy.zip
```

#### Étape 4 : Configurer les App Settings

```bash
# Chaîne de connexion
az webapp config connection-string set \
  --resource-group rg-automation-prod \
  --name app-automation-backend-prod \
  --settings SqlServer="<connection-string>" \
  --connection-string-type SQLAzure

# Configuration avec Key Vault references
az webapp config appsettings set \
  --resource-group rg-automation-prod \
  --name app-automation-backend-prod \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    Email__Password="@Microsoft.KeyVault(SecretUri=https://kv-automation-prod.vault.azure.net/secrets/Email--Password/)" \
    Twilio__AccountSid="@Microsoft.KeyVault(SecretUri=https://kv-automation-prod.vault.azure.net/secrets/Twilio--AccountSid/)" \
    Twilio__AuthToken="@Microsoft.KeyVault(SecretUri=https://kv-automation-prod.vault.azure.net/secrets/Twilio--AuthToken/)"
```

#### Étape 5 : Déployer le frontend

```bash
cd Frontend

# Build pour production
npm run build

# Déployer
az webapp up \
  --name app-automation-frontend-prod \
  --resource-group rg-automation-prod \
  --plan plan-automation-prod \
  --location westeurope \
  --runtime "NODE:20-lts"
```

### Option B : Azure Container Instances

```bash
# Créer un Azure Container Registry
az acr create \
  --resource-group rg-automation-prod \
  --name acrautomation \
  --sku Basic

# Build et push les images
az acr build --registry acrautomation --image automation-backend:latest ./Backend
az acr build --registry acrautomation --image automation-frontend:latest ./Frontend

# Créer un Container Group
az container create \
  --resource-group rg-automation-prod \
  --name aci-automation \
  --image acrautomation.azurecr.io/automation-backend:latest \
  --cpu 2 \
  --memory 4 \
  --registry-login-server acrautomation.azurecr.io \
  --registry-username <username> \
  --registry-password <password> \
  --dns-name-label automation-backend \
  --ports 80 443
```

---

## 3. Déploiement sur AWS

### Option A : AWS Elastic Beanstalk

#### Étape 1 : Installer EB CLI

```bash
pip install awsebcli
```

#### Étape 2 : Initialiser Elastic Beanstalk

```bash
cd Backend/AutomationSystem.API

# Initialiser EB
eb init -p "64bit Amazon Linux 2023 v3.0.0 running .NET 8" -r eu-west-1 automation-backend

# Créer un environnement
eb create automation-backend-prod --database.engine sqlserver-ex
```

#### Étape 3 : Configurer les variables d'environnement

```bash
eb setenv \
  ASPNETCORE_ENVIRONMENT=Production \
  Email__Username=<email> \
  Email__Password=<password> \
  Twilio__AccountSid=<sid> \
  Twilio__AuthToken=<token>
```

#### Étape 4 : Déployer

```bash
eb deploy
```

### Option B : AWS ECS (Fargate)

#### Étape 1 : Créer un ECR Repository

```bash
# Créer les repositories
aws ecr create-repository --repository-name automation-backend
aws ecr create-repository --repository-name automation-frontend

# Se connecter à ECR
aws ecr get-login-password --region eu-west-1 | docker login --username AWS --password-stdin <account-id>.dkr.ecr.eu-west-1.amazonaws.com

# Build et push les images
docker build -t automation-backend ./Backend
docker tag automation-backend:latest <account-id>.dkr.ecr.eu-west-1.amazonaws.com/automation-backend:latest
docker push <account-id>.dkr.ecr.eu-west-1.amazonaws.com/automation-backend:latest
```

#### Étape 2 : Créer un cluster ECS

```bash
aws ecs create-cluster --cluster-name automation-cluster
```

#### Étape 3 : Créer une task definition

Créez un fichier `task-definition.json` :

```json
{
  "family": "automation-backend",
  "networkMode": "awsvpc",
  "requiresCompatibilities": ["FARGATE"],
  "cpu": "1024",
  "memory": "2048",
  "containerDefinitions": [
    {
      "name": "backend",
      "image": "<account-id>.dkr.ecr.eu-west-1.amazonaws.com/automation-backend:latest",
      "portMappings": [
        {
          "containerPort": 80,
          "protocol": "tcp"
        }
      ],
      "environment": [
        {
          "name": "ASPNETCORE_ENVIRONMENT",
          "value": "Production"
        }
      ],
      "secrets": [
        {
          "name": "ConnectionStrings__SqlServer",
          "valueFrom": "arn:aws:secretsmanager:region:account:secret:db-connection"
        }
      ]
    }
  ]
}
```

```bash
aws ecs register-task-definition --cli-input-json file://task-definition.json
```

#### Étape 4 : Créer un service

```bash
aws ecs create-service \
  --cluster automation-cluster \
  --service-name automation-backend-service \
  --task-definition automation-backend \
  --desired-count 2 \
  --launch-type FARGATE \
  --network-configuration "awsvpcConfiguration={subnets=[subnet-xxx],securityGroups=[sg-xxx],assignPublicIp=ENABLED}"
```

---

## 4. Configuration de production

### 4.1 Secrets Management

#### Azure Key Vault

Dans `appsettings.Production.json` :

```json
{
  "KeyVault": {
    "VaultUri": "https://kv-automation-prod.vault.azure.net/"
  }
}
```

Dans `Program.cs` :

```csharp
if (builder.Environment.IsProduction())
{
    var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUri),
        new DefaultAzureCredential());
}
```

#### AWS Secrets Manager

```csharp
if (builder.Environment.IsProduction())
{
    var region = RegionEndpoint.EUWest1;
    var client = new AmazonSecretsManagerClient(region);
    
    var request = new GetSecretValueRequest
    {
        SecretId = "automation-secrets"
    };
    
    var response = await client.GetSecretValueAsync(request);
    var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(response.SecretString);
    
    // Ajouter les secrets à la configuration
}
```

### 4.2 HTTPS et certificats

#### Azure App Service

```bash
# Ajouter un domaine personnalisé
az webapp config hostname add \
  --webapp-name app-automation-backend-prod \
  --resource-group rg-automation-prod \
  --hostname automation.votredomaine.com

# Créer un certificat SSL managé
az webapp config ssl create \
  --name app-automation-backend-prod \
  --resource-group rg-automation-prod \
  --hostname automation.votredomaine.com
```

#### AWS avec Certificate Manager

```bash
# Demander un certificat
aws acm request-certificate \
  --domain-name automation.votredomaine.com \
  --validation-method DNS

# Configurer CloudFront ou ALB pour utiliser le certificat
```

### 4.3 Logging et monitoring

#### Application Insights (Azure)

```bash
# Créer une ressource Application Insights
az monitor app-insights component create \
  --app app-automation-insights \
  --location westeurope \
  --resource-group rg-automation-prod

# Obtenir la clé d'instrumentation
INSTRUMENTATION_KEY=$(az monitor app-insights component show \
  --app app-automation-insights \
  --resource-group rg-automation-prod \
  --query instrumentationKey -o tsv)

# Configurer dans App Settings
az webapp config appsettings set \
  --resource-group rg-automation-prod \
  --name app-automation-backend-prod \
  --settings ApplicationInsights__InstrumentationKey=$INSTRUMENTATION_KEY
```

Dans `Program.cs` :

```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

#### CloudWatch (AWS)

```bash
# Les logs vont automatiquement dans CloudWatch Logs
# Créer des alarmes
aws cloudwatch put-metric-alarm \
  --alarm-name automation-high-cpu \
  --alarm-description "CPU utilization exceeds 80%" \
  --metric-name CPUUtilization \
  --namespace AWS/ECS \
  --statistic Average \
  --period 300 \
  --threshold 80 \
  --comparison-operator GreaterThanThreshold
```

---

## 5. Monitoring et maintenance

### 5.1 Health checks

Ajoutez dans `Program.cs` :

```csharp
app.MapHealthChecks("/health");
app.MapHealthChecks("/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
```

### 5.2 Sauvegarde automatique

#### Azure SQL

```bash
# La sauvegarde est automatique, configurez la rétention
az sql db ltr-policy set \
  --resource-group rg-automation-prod \
  --server sql-automation-prod \
  --database AutomationSystem \
  --weekly-retention P12W \
  --monthly-retention P12M \
  --yearly-retention P7Y \
  --week-of-year 1
```

#### AWS RDS

```bash
aws rds modify-db-instance \
  --db-instance-identifier automation-db \
  --backup-retention-period 30 \
  --preferred-backup-window "03:00-04:00"
```

### 5.3 Scaling automatique

#### Azure

```bash
az monitor autoscale create \
  --resource-group rg-automation-prod \
  --resource app-automation-backend-prod \
  --resource-type Microsoft.Web/serverfarms \
  --name autoscale-automation \
  --min-count 2 \
  --max-count 10 \
  --count 2

az monitor autoscale rule create \
  --resource-group rg-automation-prod \
  --autoscale-name autoscale-automation \
  --condition "Percentage CPU > 70 avg 5m" \
  --scale out 2
```

#### AWS

```bash
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --resource-id service/automation-cluster/automation-backend-service \
  --scalable-dimension ecs:service:DesiredCount \
  --min-capacity 2 \
  --max-capacity 10

aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --resource-id service/automation-cluster/automation-backend-service \
  --scalable-dimension ecs:service:DesiredCount \
  --policy-name cpu-scaling \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration file://scaling-policy.json
```

---

## ✅ Checklist post-déploiement

- [ ] Vérifier que l'application est accessible
- [ ] Tester toutes les fonctionnalités principales
- [ ] Vérifier les logs (pas d'erreurs)
- [ ] Tester les tâches planifiées Hangfire
- [ ] Vérifier l'envoi d'emails
- [ ] Tester WhatsApp / Chime
- [ ] Configurer les alertes de monitoring
- [ ] Documenter les URLs de production
- [ ] Former l'équipe à l'utilisation
- [ ] Mettre en place un plan de disaster recovery

**Bon déploiement ! 🎉**

