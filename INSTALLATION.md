# 📦 Guide d'Installation Détaillé

Ce guide vous accompagne pas à pas dans l'installation et la configuration du système d'automatisation.

## Table des matières

1. [Prérequis](#prérequis)
2. [Installation avec Docker](#installation-avec-docker)
3. [Installation manuelle](#installation-manuelle)
4. [Configuration](#configuration)
5. [Premier lancement](#premier-lancement)
6. [Dépannage](#dépannage)

---

## 1. Prérequis

### Logiciels requis

| Logiciel | Version minimale | Téléchargement |
|----------|------------------|----------------|
| .NET SDK | 8.0 | https://dotnet.microsoft.com/download |
| Node.js | 20.x | https://nodejs.org/ |
| Docker Desktop | 24.x | https://www.docker.com/products/docker-desktop |
| Git | 2.x | https://git-scm.com/ |

### Optionnel mais recommandé

- Visual Studio 2022 / VS Code
- SQL Server Management Studio
- Postman (pour tester l'API)

### Comptes de service nécessaires

Pour utiliser toutes les fonctionnalités :

1. **Compte Email (Outlook/Gmail)**
   - Pour lecture/envoi d'emails
   - Activer l'authentification par mot de passe d'application

2. **Compte Twilio**
   - Pour WhatsApp
   - S'inscrire sur https://www.twilio.com/
   - Obtenir AccountSid et AuthToken

3. **Amazon Chime Webhook** (optionnel)
   - Créer un webhook dans votre room Chime

4. **CORTEX API Access** (selon votre organisation)

---

## 2. Installation avec Docker (⭐ Recommandé)

### Étape 1 : Cloner le repository

```bash
git clone <repository-url>
cd Automatisation
```

### Étape 2 : Configuration des secrets

Créez un fichier `.env` à la racine :

```bash
# Base de données
SA_PASSWORD=YourStrong@Passw0rd123

# Email
EMAIL_USERNAME=votre-email@outlook.com
EMAIL_PASSWORD=votre-mot-de-passe

# Twilio
TWILIO_ACCOUNT_SID=ACxxxxxxxxxxxxxxxxxxxx
TWILIO_AUTH_TOKEN=your_auth_token
TWILIO_WHATSAPP_NUMBER=+14155238886

# Chime
CHIME_WEBHOOK_URL=https://hooks.chime.aws/incomingwebhooks/xxxxx

# CORTEX
CORTEX_API_KEY=your_cortex_api_key
```

### Étape 3 : Lancer les conteneurs

```bash
# Construire et démarrer tous les services
docker-compose up -d

# Vérifier que tout fonctionne
docker-compose ps
```

Vous devriez voir 3 conteneurs en cours d'exécution :
- `automation-sqlserver`
- `automation-backend`
- `automation-frontend`

### Étape 4 : Initialiser la base de données

```bash
# Attendre 30 secondes que SQL Server démarre

# Appliquer les migrations
docker-compose exec backend dotnet ef database update
```

### Étape 5 : Accéder à l'application

| Service | URL | Credentials |
|---------|-----|-------------|
| Frontend | http://localhost:4200 | admin / admin |
| API | http://localhost:5000 | - |
| Swagger | http://localhost:5000/swagger | - |
| Hangfire | http://localhost:5000/hangfire | - |

**Installation terminée ! 🎉**

---

## 3. Installation manuelle

### Étape 1 : Configuration de la base de données

#### Option A : SQL Server local

```bash
# Installer SQL Server Developer Edition
# https://www.microsoft.com/sql-server/sql-server-downloads

# Créer la base de données
sqlcmd -S localhost -U sa -P YourPassword
CREATE DATABASE AutomationSystem;
GO
```

#### Option B : PostgreSQL

```bash
# Installer PostgreSQL
# https://www.postgresql.org/download/

# Créer la base de données
psql -U postgres
CREATE DATABASE "AutomationSystem";
```

### Étape 2 : Backend

```bash
cd Backend

# Restaurer les packages
dotnet restore

# Mettre à jour appsettings.json avec vos paramètres
code AutomationSystem.API/appsettings.json

# Créer la base de données
dotnet ef database update \
  --project AutomationSystem.Infrastructure \
  --startup-project AutomationSystem.API

# Lancer l'application
cd AutomationSystem.API
dotnet run
```

L'API sera disponible sur `https://localhost:5001`

### Étape 3 : Frontend

```bash
cd Frontend

# Installer les dépendances
npm install

# Mettre à jour l'URL de l'API
code src/environments/environment.ts

# Lancer en développement
npm start
```

L'application sera disponible sur `http://localhost:4200`

---

## 4. Configuration

### 4.1 Configuration Email (MailKit)

#### Pour Outlook/Office 365

```json
{
  "Email": {
    "ImapHost": "outlook.office365.com",
    "ImapPort": "993",
    "SmtpHost": "smtp.office365.com",
    "SmtpPort": "587",
    "Username": "votre-email@outlook.com",
    "Password": "votre-mot-de-passe-application"
  }
}
```

**Important** : Activez l'authentification à deux facteurs et créez un mot de passe d'application :
1. Allez sur https://account.microsoft.com/security
2. Sécurité avancée → Mot de passe d'application
3. Créez un nouveau mot de passe

#### Pour Gmail

```json
{
  "Email": {
    "ImapHost": "imap.gmail.com",
    "ImapPort": "993",
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "Username": "votre-email@gmail.com",
    "Password": "votre-mot-de-passe-application"
  }
}
```

**Pour Gmail** :
1. Activez l'authentification à 2 facteurs
2. Générez un mot de passe d'application : https://myaccount.google.com/apppasswords

### 4.2 Configuration Twilio (WhatsApp)

1. Créez un compte sur https://www.twilio.com/
2. Obtenez vos credentials dans la console
3. Activez WhatsApp Sandbox pour le test
4. Ajoutez dans `appsettings.json` :

```json
{
  "Twilio": {
    "AccountSid": "ACxxxxxxxxxxxxx",
    "AuthToken": "votre_token",
    "WhatsAppNumber": "+14155238886"
  }
}
```

### 4.3 Configuration Amazon Chime

1. Dans votre room Chime, allez dans les paramètres
2. Créez un nouveau webhook
3. Copiez l'URL générée
4. Ajoutez dans `appsettings.json` :

```json
{
  "Chime": {
    "WebhookUrl": "https://hooks.chime.aws/incomingwebhooks/xxxxx"
  }
}
```

### 4.4 Configuration CORTEX API

```json
{
  "Cortex": {
    "BaseUrl": "https://cortex.amazon.com/api",
    "ApiKey": "votre_api_key"
  }
}
```

### 4.5 Configuration JWT

Pour sécuriser l'API, changez la clé JWT :

```json
{
  "Jwt": {
    "Key": "VotreCléSecrèteSuperLongueEtComplexe32CharsMin",
    "Issuer": "AutomationSystem",
    "Audience": "AutomationSystemClient",
    "ExpiryMinutes": 60
  }
}
```

**⚠️ Important** : En production, utilisez une clé de 32+ caractères aléatoires !

---

## 5. Premier lancement

### Vérifier que tout fonctionne

#### 1. Test de l'API

```bash
# Vérifier la santé de l'API
curl http://localhost:5000/api/tasks

# Vous devriez recevoir une liste de tâches (ou une erreur 401 - c'est normal)
```

#### 2. Créer le premier utilisateur

Pour l'instant, l'authentification est basique. Vous pouvez vous connecter avec :
- **Username** : `admin`
- **Password** : `admin`

⚠️ **À faire en production** : Implémenter une vraie gestion des utilisateurs !

#### 3. Tester Hangfire

1. Allez sur http://localhost:5000/hangfire
2. Vous devriez voir le dashboard avec 3 tâches récurrentes :
   - `routenverfuegbarkeit-job`
   - `staging-plan-job`
   - `dnr-units-job`

#### 4. Uploader un fichier de test

1. Créez un fichier Excel simple `test.xlsx`
2. Allez sur http://localhost:4200
3. Connectez-vous
4. Menu **Fichiers** → **Télécharger**
5. Sélectionnez `test.xlsx`
6. Vérifiez qu'il apparaît dans la liste

#### 5. Exécuter une tâche manuellement

1. Menu **Tâches**
2. Cliquez sur **Exécuter** pour une tâche
3. Vérifiez dans **Logs** que l'exécution a été enregistrée

---

## 6. Dépannage

### Problème : Le backend ne démarre pas

#### Erreur de connexion à la base de données

```
Microsoft.Data.SqlClient.SqlException: A network-related or instance-specific error...
```

**Solution** :
```bash
# Vérifiez que SQL Server est en cours d'exécution
docker ps | grep sqlserver

# Vérifiez la chaîne de connexion
# Elle doit correspondre au nom du conteneur si vous utilisez Docker
```

#### Port déjà utilisé

```
Unable to bind to https://localhost:5001
```

**Solution** :
```bash
# Changer le port dans launchSettings.json ou utiliser un autre port
export ASPNETCORE_URLS="https://localhost:5002;http://localhost:5003"
dotnet run
```

### Problème : Le frontend ne se connecte pas au backend

#### CORS Error

```
Access to XMLHttpRequest has been blocked by CORS policy
```

**Solution** :
1. Vérifiez que le backend autorise l'origine du frontend dans `Program.cs`
2. L'URL doit correspondre exactement (attention au port)

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Vérifiez cette URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

### Problème : Hangfire n'exécute pas les tâches

#### Les tâches apparaissent comme "Enqueued" mais ne s'exécutent jamais

**Solution** :
```bash
# Vérifiez que Hangfire Server est démarré
# Dans Program.cs, vous devez avoir :
builder.Services.AddHangfireServer();
```

#### Expression Cron invalide

**Solution** :
- Format : `minute heure jour mois jour_semaine`
- Exemple : `0 9 * * *` = Tous les jours à 9h00
- Testez sur https://crontab.guru/

### Problème : Impossible d'envoyer des emails

#### Authentication failed

**Solution** :
1. Vérifiez que vous utilisez un mot de passe d'application (pas le mot de passe principal)
2. Vérifiez que l'authentification à 2 facteurs est activée
3. Pour Gmail, autorisez les applications moins sécurisées (ou utilisez un mot de passe d'app)

### Problème : Docker compose échoue

#### Erreur de permissions

**Solution Linux/Mac** :
```bash
sudo chown -R $USER:$USER .
```

#### Out of memory

**Solution** :
1. Augmentez la mémoire allouée à Docker Desktop
2. Settings → Resources → Memory → 4GB minimum

---

## 🎓 Prochaines étapes

Une fois l'installation réussie :

1. ✅ Lisez le [README.md](README.md) pour comprendre l'architecture
2. ✅ Consultez la documentation API sur `/swagger`
3. ✅ Créez vos propres tâches planifiées
4. ✅ Configurez vos destinataires de notifications
5. ✅ Uploadez vos fichiers Excel de production

---

## 📞 Besoin d'aide ?

- **Documentation complète** : Voir [README.md](README.md)
- **API Reference** : http://localhost:5000/swagger
- **Hangfire Docs** : https://docs.hangfire.io/
- **Angular Docs** : https://angular.io/docs

**Bonne installation ! 🚀**

