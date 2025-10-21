# 🤖 Système d'Automatisation - Architecture Complète

Un système d'automatisation robuste avec architecture trois couches pour gérer les tâches planifiées, le traitement de fichiers Excel, les notifications multi-canaux et l'intégration avec CORTEX.

## 📋 Table des matières

- [Architecture](#architecture)
- [Fonctionnalités](#fonctionnalités)
- [Technologies utilisées](#technologies-utilisées)
- [Installation](#installation)
- [Configuration](#configuration)
- [Utilisation](#utilisation)
- [API Documentation](#api-documentation)
- [Déploiement](#déploiement)

---

## 🏗️ Architecture

```
+--------------------------------------------------------+
|                     FRONTEND (Angular)                 |
|  - Dashboard (état des tâches, logs, configuration)    |
|  - Interface admin : upload fichiers, config horaires  |
+--------------------------------------------------------+
|                     BACKEND (ASP.NET Core)             |
|  - Service d'automatisation planifiée (Hangfire)       |
|  - Modules : Mail, Excel, CORTEX API, WhatsApp, Chime  |
|  - API REST pour Angular                               |
+--------------------------------------------------------+
|                     INFRASTRUCTURE                     |
|  - Base de données (SQL Server / PostgreSQL)           |
|  - Stockage fichiers (local ou S3)                     |
|  - Authentification / Sécurité                         |
+--------------------------------------------------------+
```

### Structure du projet

```
Automatisation/
├── Backend/
│   ├── AutomationSystem.API/          # API REST ASP.NET Core
│   ├── AutomationSystem.Core/         # Logique métier et services
│   ├── AutomationSystem.Infrastructure/ # Base de données et infrastructure
│   └── AutomationSystem.sln
├── Frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/                  # Services, guards, interceptors
│   │   │   └── features/              # Composants fonctionnels
│   │   └── environments/
│   ├── angular.json
│   └── package.json
├── docker-compose.yml
└── README.md
```

---

## ✨ Fonctionnalités

### 🔄 Automatisation planifiée

- **Routenverfügbarkeit** (08h25) : Envoi quotidien du planning de disponibilité
- **Staging Plan** (09h15) : Traitement et analyse du plan de staging
- **DNR Units** (toutes les 15 min) : Traitement automatique des emails DNR

### 📊 Traitement de données

- Lecture et analyse de fichiers Excel (ClosedXML)
- Conversion Excel → HTML → Image
- Extraction et validation de données
- Génération de rapports

### 📨 Notifications multi-canaux

- **Email** : Lecture IMAP et envoi SMTP (MailKit)
- **WhatsApp** : Envoi via Twilio API
- **Amazon Chime** : Webhooks pour notifications d'équipe

### 🔌 Intégrations

- **CORTEX API (Amazon)** : Récupération des données de tournées
- **Azure Key Vault** : Gestion sécurisée des secrets (optionnel)
- **Hangfire Dashboard** : Monitoring des tâches en temps réel

### 📱 Interface Web

- Dashboard avec statistiques en temps réel
- Gestion des tâches planifiées
- Consultation des logs d'exécution
- Upload et gestion de fichiers
- Configuration des destinataires

---

## 🛠️ Technologies utilisées

### Backend
- **ASP.NET Core 8.0** - Framework web
- **Hangfire** - Planification et exécution de tâches
- **Entity Framework Core** - ORM
- **ClosedXML** - Traitement Excel
- **MailKit** - Gestion des emails
- **Twilio** - Envoi WhatsApp
- **Serilog** - Logging

### Frontend
- **Angular 18** - Framework SPA
- **Tailwind CSS** - Styling
- **RxJS** - Programmation réactive
- **TypeScript** - Langage typé

### Infrastructure
- **SQL Server 2022** (ou PostgreSQL 15)
- **Docker & Docker Compose**
- **Nginx** - Reverse proxy

---

## 🚀 Installation

### Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (recommandé)

### Option 1 : Docker (Recommandé)

```bash
# Cloner le repository
git clone <repository-url>
cd Automatisation

# Lancer avec Docker Compose
docker-compose up -d

# Accéder à l'application
# Frontend: http://localhost:4200
# API: http://localhost:5000
# Hangfire: http://localhost:5000/hangfire
# Swagger: http://localhost:5000/swagger
```

### Option 2 : Installation manuelle

#### Backend

```bash
cd Backend

# Restaurer les packages NuGet
dotnet restore

# Créer la base de données
dotnet ef database update --project AutomationSystem.Infrastructure --startup-project AutomationSystem.API

# Lancer l'API
cd AutomationSystem.API
dotnet run
```

#### Frontend

```bash
cd Frontend

# Installer les dépendances
npm install

# Lancer en mode développement
npm start

# Build pour production
npm run build
```

---

## ⚙️ Configuration

### Backend - appsettings.json

```json
{
  "ConnectionStrings": {
    "SqlServer": "Server=localhost;Database=AutomationSystem;Trusted_Connection=True;"
  },
  "Email": {
    "ImapHost": "imap.outlook.com",
    "ImapPort": "993",
    "SmtpHost": "smtp.outlook.com",
    "SmtpPort": "587",
    "Username": "votre-email@outlook.com",
    "Password": "votre-mot-de-passe"
  },
  "Twilio": {
    "AccountSid": "VOTRE_ACCOUNT_SID",
    "AuthToken": "VOTRE_AUTH_TOKEN",
    "WhatsAppNumber": "+14155238886"
  },
  "Chime": {
    "WebhookUrl": "https://hooks.chime.aws/incomingwebhooks/VOTRE_WEBHOOK"
  },
  "Cortex": {
    "BaseUrl": "https://cortex.amazon.com/api",
    "ApiKey": "VOTRE_API_KEY"
  }
}
```

### Frontend - environment.ts

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
```

### Variables d'environnement (Production)

Pour la production, utilisez des variables d'environnement ou Azure Key Vault :

```bash
export Email__Username="votre-email@outlook.com"
export Email__Password="votre-mot-de-passe"
export Twilio__AccountSid="VOTRE_SID"
export Twilio__AuthToken="VOTRE_TOKEN"
```

---

## 📖 Utilisation

### Créer une nouvelle tâche planifiée

1. Accédez à l'interface web : `http://localhost:4200`
2. Connectez-vous (utilisateur par défaut : admin / admin)
3. Allez dans **Tâches** → **Nouvelle tâche**
4. Configurez :
   - Nom de la tâche
   - Type (Routenverfuegbarkeit, StagingPlan, DNRUnits)
   - Expression Cron (ex: `25 8 * * *` = tous les jours à 8h25)
5. Activez la tâche

### Uploader un fichier Excel

1. Allez dans **Fichiers**
2. Cliquez sur **Télécharger un fichier**
3. Sélectionnez votre fichier `.xlsx`
4. Le système le traitera automatiquement

### Ajouter des destinataires

1. Allez dans **Destinataires**
2. Cliquez sur **Nouveau destinataire**
3. Choisissez le type (Email, WhatsApp, Chime)
4. Entrez l'identifiant approprié

### Consulter les logs

1. Allez dans **Logs**
2. Filtrez par :
   - Nom de la tâche
   - Statut (Succès, Échec, Avertissement)
   - Période
3. Cliquez sur une ligne pour voir les détails

---

## 📚 API Documentation

### Endpoints principaux

#### Tâches
- `GET /api/tasks` - Liste des tâches planifiées
- `POST /api/tasks` - Créer une tâche
- `PUT /api/tasks/{id}` - Modifier une tâche
- `DELETE /api/tasks/{id}` - Supprimer une tâche
- `POST /api/tasks/{id}/execute` - Exécuter manuellement

#### Logs
- `GET /api/logs` - Liste des logs (avec pagination et filtres)
- `GET /api/logs/{id}` - Détails d'un log
- `GET /api/logs/stats` - Statistiques d'exécution

#### Fichiers
- `GET /api/files` - Liste des fichiers
- `POST /api/files/upload` - Upload un fichier
- `GET /api/files/{id}/download` - Télécharger un fichier
- `DELETE /api/files/{id}` - Supprimer un fichier

#### Destinataires
- `GET /api/recipients` - Liste des destinataires
- `POST /api/recipients` - Créer un destinataire
- `PATCH /api/recipients/{id}/toggle` - Activer/Désactiver

Documentation complète : `http://localhost:5000/swagger`

---

## 🚢 Déploiement

### Azure App Service

```bash
# Publier le backend
cd Backend/AutomationSystem.API
dotnet publish -c Release -o ./publish

# Déployer avec Azure CLI
az webapp up --name votre-app-name --resource-group votre-rg
```

### AWS EC2 / Elastic Beanstalk

```bash
# Créer une image Docker
docker build -t automation-backend ./Backend
docker build -t automation-frontend ./Frontend

# Pousser vers ECR
docker tag automation-backend:latest xxx.dkr.ecr.region.amazonaws.com/automation-backend
docker push xxx.dkr.ecr.region.amazonaws.com/automation-backend
```

### Docker Swarm / Kubernetes

Utilisez les fichiers de configuration fournis dans `/deployment`.

---

## 🔐 Sécurité

### Bonnes pratiques implémentées

- ✅ Authentification JWT
- ✅ HTTPS obligatoire en production
- ✅ Secrets gérés via variables d'environnement
- ✅ Validation des entrées utilisateur
- ✅ CORS configuré
- ✅ Rate limiting sur l'API (recommandé)

### Pour la production

1. Changez toutes les clés secrètes dans `appsettings.json`
2. Utilisez Azure Key Vault ou AWS Secrets Manager
3. Activez l'authentification Azure AD / OAuth2
4. Configurez HTTPS avec un certificat valide
5. Mettez en place un WAF (Web Application Firewall)

---

## 📊 Monitoring

### Hangfire Dashboard

Accédez à `http://localhost:5000/hangfire` pour :
- Voir les tâches en cours
- Historique des exécutions
- Réessayer les tâches échouées
- Statistiques de performance

### Logs

Les logs sont écrits dans :
- Console (développement)
- Fichiers : `logs/automation-{Date}.txt`
- Application Insights (production - optionnel)

---

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à :
1. Fork le projet
2. Créer une branche feature (`git checkout -b feature/AmazingFeature`)
3. Commit vos changements (`git commit -m 'Add AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

---

## 📝 Licence

Ce projet est sous licence MIT.

---

## 📞 Support

Pour toute question ou problème :
- Ouvrez une issue sur GitHub
- Consultez la documentation Swagger
- Vérifiez les logs dans Hangfire Dashboard

---

## 🎯 Roadmap

- [ ] Intégration IA pour analyse prédictive
- [ ] Support multi-tenant
- [ ] API GraphQL
- [ ] Application mobile (React Native)
- [ ] Export des données en PDF
- [ ] Notifications push navigateur
- [ ] Tableau de bord analytique avancé

---

**Fait avec ❤️ pour l'automatisation intelligente**

