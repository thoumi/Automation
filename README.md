# 🤖 Système d'Automatisation d'Entreprise

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-18-DD0031?logo=angular)](https://angular.io/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-Proprietary-red.svg)]()

> Plateforme complète d'automatisation pour la gestion de tâches récurrentes, notifications multi-canaux et intégration d'APIs externes.

---

## 📋 Table des matières

- [Vue d'ensemble](#-vue-densemble)
- [Fonctionnalités](#-fonctionnalités)
- [Architecture](#-architecture)
- [Technologies](#-technologies)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Utilisation](#-utilisation)
- [Documentation](#-documentation)
- [Contribution](#-contribution)
- [Support](#-support)

---

## 🎯 Vue d'ensemble

Ce système d'automatisation permet de :
- **Planifier et exécuter** des tâches automatisées selon un calendrier personnalisé
- **Traiter des fichiers Excel** et générer des rapports automatiquement
- **Envoyer des notifications** via Email, WhatsApp et Amazon Chime
- **Intégrer des APIs externes** (CORTEX, services tiers)
- **Monitorer l'exécution** avec un tableau de bord en temps réel
- **Gérer les utilisateurs** avec authentification sécurisée

### Cas d'usage typiques

✅ Vérification automatique de disponibilité de routes (Routenverfuegbarkeit)  
✅ Génération de plans de staging quotidiens  
✅ Extraction et traitement d'unités DNR  
✅ Envoi de rapports programmés par email  
✅ Notifications d'alertes en temps réel  

---

## ✨ Fonctionnalités

### 🗓️ Planification Simplifiée
- Interface intuitive pour créer des horaires (quotidien, hebdomadaire, mensuel)
- Plus besoin de CRON expressions complexes
- Aperçu en français de la planification

### 📊 Tableau de Bord
- Vue d'ensemble des tâches en cours d'exécution
- Historique complet des exécutions
- Statistiques de succès/échec
- Graphiques de performance

### 📧 Notifications Multi-Canaux
- **Email** : Envoi de rapports et alertes
- **WhatsApp** : Notifications instantanées via Twilio
- **Amazon Chime** : Intégration webhooks pour équipes

### 📁 Gestion de Fichiers
- Upload de fichiers Excel
- Traitement automatique avec ClosedXML
- Génération de rapports et images
- Historique des traitements

### 🔐 Sécurité
- Authentification JWT
- Gestion des utilisateurs et rôles
- Hashage des mots de passe avec BCrypt
- Protection CORS configurée

### 🐳 Déploiement Facile
- Architecture conteneurisée (Docker)
- Configuration via variables d'environnement
- Base de données SQL Server incluse
- Reverse proxy Nginx pour le frontend

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                         FRONTEND                            │
│              Angular 18 + Tailwind CSS                      │
│         (http://localhost:4300)                             │
└────────────────────┬────────────────────────────────────────┘
                     │ HTTP/REST
                     ▼
┌─────────────────────────────────────────────────────────────┐
│                         BACKEND API                          │
│                    ASP.NET Core 8                           │
│         (http://localhost:5555/api)                         │
│                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐    │
│  │   Controllers │  │   Services   │  │   Jobs       │    │
│  │   (REST API)  │  │  (Business)  │  │  (Hangfire)  │    │
│  └──────────────┘  └──────────────┘  └──────────────┘    │
└────────────────────┬────────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────────┐
│                    INFRASTRUCTURE                            │
│                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐    │
│  │  SQL Server  │  │  File System │  │  External    │    │
│  │  (Database)  │  │  (Uploads)   │  │  APIs        │    │
│  └──────────────┘  └──────────────┘  └──────────────┘    │
└─────────────────────────────────────────────────────────────┘
```

### Couches applicatives

1. **Frontend** (Angular)
   - Interface utilisateur responsive
   - Authentification et routing
   - Consommation API REST

2. **Backend** (ASP.NET Core)
   - API RESTful
   - Authentification JWT
   - Logique métier
   - Orchestration des jobs

3. **Infrastructure**
   - Base de données SQL Server
   - Stockage de fichiers
   - Services externes (Email, SMS, Webhooks)

---

## 🛠️ Technologies

### Backend
- **Runtime** : .NET 8.0
- **Framework** : ASP.NET Core
- **ORM** : Entity Framework Core
- **Scheduler** : Hangfire
- **Email** : MailKit
- **Excel** : ClosedXML
- **Authentication** : JWT Bearer
- **Logging** : Serilog/NLog

### Frontend
- **Framework** : Angular 18
- **Styling** : Tailwind CSS
- **HTTP Client** : Angular HttpClient
- **Routing** : Angular Router
- **Forms** : Reactive Forms

### Infrastructure
- **Database** : SQL Server 2019+
- **Containerization** : Docker & Docker Compose
- **Web Server** : Nginx (frontend)
- **Reverse Proxy** : Kestrel (backend)

### Services Externes
- **Twilio** : WhatsApp/SMS
- **Amazon Chime** : Webhooks
- **CORTEX API** : Intégrations métier

---

## 📦 Installation

### Prérequis

- Docker Desktop (Windows/Mac) ou Docker Engine (Linux)
- Git
- 4 GB RAM minimum
- Ports disponibles : 1444, 5555, 4300

### Installation Rapide

1. **Cloner le repository**
   ```bash
   git clone https://github.com/thoumi/Automation.git
   cd Automation
   ```

2. **Démarrer l'application**
   ```bash
   docker-compose up -d
   ```

3. **Initialiser la base de données**
   ```bash
   docker exec automation-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Test@Password123" -C -i /docker-entrypoint-initdb.d/init-database.sql
   ```

4. **Accéder à l'application**
   - Frontend : http://localhost:4300
   - Backend API : http://localhost:5555/api
   - Hangfire Dashboard : http://localhost:5555/hangfire

5. **Se connecter**
   - Email : `admin@example.com`
   - Mot de passe : `admin123`

---

## ⚙️ Configuration

### Variables d'environnement

Créez un fichier `.env` à la racine du projet :

```env
# Base de données
DB_SERVER=sqlserver
DB_NAME=AutomationSystem
DB_USER=sa
DB_PASSWORD=VotreMotDePasseSecurise

# JWT
JWT_KEY=VotreCleSecrete256BitsMinimum
JWT_ISSUER=AutomationSystem
JWT_AUDIENCE=AutomationSystemClient
JWT_DURATION=60

# Email
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=votre@email.com
SMTP_PASSWORD=VotreMotDePasse

# Twilio (WhatsApp)
TWILIO_ACCOUNT_SID=ACxxxxxxxxxxxx
TWILIO_AUTH_TOKEN=votre_token
TWILIO_WHATSAPP_FROM=whatsapp:+14155238886

# Amazon Chime
CHIME_WEBHOOK_URL=https://hooks.chime.aws/...

# CORTEX API
CORTEX_API_URL=https://api.cortex.com
CORTEX_API_KEY=votre_cle_api
```

### Configuration Backend

Modifiez `Backend/AutomationSystem.API/appsettings.json` :

```json
{
  "ConnectionStrings": {
    "SqlServer": "Server=sqlserver;Database=AutomationSystem;User=sa;Password=Test@Password123;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "VotreCleSecrete256BitsMinimum",
    "Issuer": "AutomationSystem",
    "Audience": "AutomationSystemClient",
    "DurationInMinutes": 60
  }
}
```

### Configuration Frontend

Modifiez `Frontend/src/environments/environment.ts` :

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5555/api'
};
```

---

## 🚀 Utilisation

### Créer une Tâche Automatisée

1. **Naviguer vers "Tâches"**
2. **Cliquer sur "Nouvelle Tâche"**
3. **Remplir les informations** :
   - Nom de la tâche
   - Description
   - Type de tâche (Routenverfuegbarkeit, StagingPlan, DNRUnits)
   - Configuration JSON si nécessaire
4. **Configurer la planification** :
   - Choisir la fréquence (quotidien, hebdomadaire, mensuel)
   - Définir l'heure d'exécution
5. **Activer la tâche**

### Ajouter des Destinataires de Notifications

1. **Naviguer vers "Destinataires"**
2. **Cliquer sur "Nouveau Destinataire"**
3. **Choisir le type** :
   - Email
   - WhatsApp
   - Chime
4. **Entrer l'identifiant** (email, numéro, webhook URL)
5. **Enregistrer**

### Consulter les Logs

1. **Naviguer vers "Logs"**
2. **Filtrer par** :
   - Tâche
   - Statut (Succès, Échec, En cours)
   - Date
3. **Voir les détails** d'une exécution

---

## 📚 Documentation

### Documentation Complète

- 📖 **[Documentation Technique](./DOCUMENTATION_TECHNIQUE.md)** : Architecture détaillée, API, modèles de données
- 📖 **[Documentation Fonctionnelle](./DOCUMENTATION_FONCTIONNELLE.md)** : Guide utilisateur, cas d'usage, workflows
- 📖 **[Guide de Démarrage](./LISEZ_MOI_EN_PREMIER.txt)** : Premier pas avec l'application
- 📖 **[Guide Planification](./GUIDE_PLANIFICATION_SIMPLE.md)** : Comment utiliser le système de planification

### API Documentation

#### Authentification

**POST** `/api/auth/login`
```json
{
  "email": "admin@example.com",
  "password": "admin123"
}
```

**Réponse** :
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Tâches

**GET** `/api/tasks` - Liste toutes les tâches  
**POST** `/api/tasks` - Créer une nouvelle tâche  
**PUT** `/api/tasks/{id}` - Modifier une tâche  
**DELETE** `/api/tasks/{id}` - Supprimer une tâche  
**POST** `/api/tasks/{id}/execute` - Exécuter manuellement  

#### Logs

**GET** `/api/logs` - Liste tous les logs  
**GET** `/api/logs/{id}` - Détails d'un log  

#### Destinataires

**GET** `/api/recipients` - Liste tous les destinataires  
**POST** `/api/recipients` - Créer un destinataire  
**DELETE** `/api/recipients/{id}` - Supprimer un destinataire  

---

## 🤝 Contribution

### Guidelines

1. Fork le projet
2. Créer une branche feature (`git checkout -b feature/AmazingFeature`)
3. Commit les changements (`git commit -m 'Add AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

### Standards de Code

- **Backend** : Suivre les conventions C# et .NET
- **Frontend** : Suivre les conventions Angular et TypeScript
- **Commits** : Messages clairs en français
- **Tests** : Ajouter des tests unitaires pour les nouvelles fonctionnalités

---

## 📝 Licence

Projet propriétaire - Tous droits réservés © 2025

---

## 📞 Support

### Contact

- **Email** : support@votreentreprise.com
- **Issues** : [GitHub Issues](https://github.com/thoumi/Automation/issues)

### FAQ

**Q: Comment changer le mot de passe admin ?**  
R: Exécutez le script SQL fourni dans `scripts/change-admin-password.sql`

**Q: L'application ne démarre pas ?**  
R: Vérifiez que Docker est bien lancé et que les ports 1444, 5555, 4300 sont disponibles

**Q: Comment ajouter une nouvelle API externe ?**  
R: Créez un nouveau service dans `Backend/AutomationSystem.Core/Services/`

**Q: Les emails ne s'envoient pas ?**  
R: Vérifiez les paramètres SMTP dans `appsettings.json`

---

## 🗺️ Roadmap

- [ ] Interface d'administration avancée
- [ ] Support multi-langues (EN, DE)
- [ ] Notifications Slack/Teams
- [ ] API GraphQL
- [ ] Mobile App (React Native)
- [ ] Export de rapports PDF
- [ ] Intégration CI/CD
- [ ] Tests automatisés (Unit, Integration, E2E)

---

## 📊 Statistiques du Projet

- **Lignes de code** : ~12,000
- **Fichiers** : 131
- **Langages** : C#, TypeScript, HTML, CSS, SQL
- **Commits** : 1+ (Initial release)

---

**⭐ Si ce projet vous est utile, n'hésitez pas à lui donner une étoile !**

---

*Dernière mise à jour : Octobre 2025*
