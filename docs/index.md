# Système d'Automatisation - Documentation Technique

## Plateforme Complète d'Automatisation d'Entreprise

---

## 🎯 Présentation du Projet

**Système d'Automatisation** est une plateforme web complète développée avec **ASP.NET Core 8** et **Angular 18**. Ce projet démontre une expertise technique complète en développement full-stack, architecture en couches, et intégration de services externes.

**Contexte de développement** : Projet professionnel démontrant la maîtrise des technologies modernes d'automatisation d'entreprise.

### Spécifications Techniques

| Composant | Technologie | Version | Description |
|-----------|-------------|---------|-------------|
| **Frontend** | Angular | 18+ | Interface utilisateur moderne |
| **Backend** | ASP.NET Core | 8.0 | API RESTful performante |
| **Base de données** | SQL Server | 2019+ | Stockage relationnel |
| **ORM** | Entity Framework Core | 8.0 | Mapping objet-relationnel |
| **Scheduler** | Hangfire | Latest | Gestion des tâches planifiées |
| **Temps réel** | SignalR (à venir) | - | Communication temps réel |
| **Architecture** | Layered | - | Séparation des responsabilités |

---

## 🚀 Fonctionnalités Principales

### Gestion des Tâches Automatisées

- ✅ **Planification simplifiée** : Interface intuitive pour créer des horaires
- ✅ **Exécution automatique** : Jobs Hangfire configurables
- ✅ **Monitoring en temps réel** : Suivi des exécutions
- ✅ **Gestion d'erreurs** : Logs détaillés et alertes

### Notifications Multi-Canaux

- 📧 **Email** : Envoi de rapports et alertes via MailKit
- 📱 **WhatsApp** : Notifications instantanées via Twilio
- 💬 **Amazon Chime** : Intégration webhooks pour équipes

### Traitement de Fichiers

- 📊 **Excel** : Lecture et génération avec ClosedXML
- 🔄 **Processing automatique** : Workflows configurables
- 📁 **Gestion des uploads** : Stockage et historique

### Intégrations Externes

- 🔌 **API CORTEX** : Intégration métier personnalisée
- 🌐 **Services tiers** : Architecture extensible
- 🔐 **Authentification sécurisée** : JWT avec refresh tokens

---

## 📊 Architecture et Technologies

### Vue d'Ensemble Technique

```
┌─────────────────────────────────────────────────────────────┐
│                         FRONTEND                            │
│              Angular 18 + Tailwind CSS                      │
│         (http://localhost:4300)                             │
└────────────────────┬────────────────────────────────────────┘
                     │ HTTP/REST + JWT
                     ▼
┌─────────────────────────────────────────────────────────────┐
│                         BACKEND API                          │
│                    ASP.NET Core 8                           │
│         (http://localhost:5555/api)                         │
│                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐    │
│  │ Controllers  │  │   Services   │  │   Jobs       │    │
│  │  (REST API)  │  │  (Business)  │  │  (Hangfire)  │    │
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

### Stack Technique Complet

#### Frontend (Angular 18)

- **Framework** : Angular 18+ avec Standalone Components
- **Styling** : Tailwind CSS + DaisyUI
- **State Management** : Services avec RxJS
- **Routing** : Angular Router avec Guards
- **HTTP** : HttpClient avec Interceptors
- **Build** : ESBuild pour performances optimales

#### Backend (ASP.NET Core 8)

- **Framework** : ASP.NET Core 8 Web API
- **ORM** : Entity Framework Core 8
- **Authentication** : JWT Bearer Tokens
- **Scheduler** : Hangfire pour jobs récurrents
- **Email** : MailKit pour SMTP
- **Excel** : ClosedXML pour traitement
- **Logging** : Serilog/NLog

#### Infrastructure

- **Database** : SQL Server 2019+
- **Containerization** : Docker + Docker Compose
- **Web Server** : Nginx (frontend) + Kestrel (backend)
- **Storage** : File system local (extensible à cloud)

---

## 🌟 Expertise Technique Développée

### Développement Full-Stack

- ✅ **Frontend moderne** : Angular 18, TypeScript, Tailwind
- ✅ **Backend robuste** : ASP.NET Core, C#, EF Core
- ✅ **Base de données** : SQL Server, migrations
- ✅ **Architecture** : Layered architecture, Repository pattern

### Technologies Avancées

- ✅ **Planification** : Système simplifié remplaçant CRON
- ✅ **Automatisation** : Hangfire pour jobs récurrents
- ✅ **Intégrations** : APIs externes (CORTEX, Twilio, Chime)
- ✅ **Sécurité** : JWT, BCrypt, CORS, HTTPS

### DevOps et Déploiement

- ✅ **Conteneurisation** : Docker, Docker Compose
- ✅ **CI/CD ready** : Structure pour GitHub Actions
- ✅ **Configuration** : Variables d'environnement
- ✅ **Monitoring** : Logs centralisés

---

## 📦 Installation et Démarrage

### Prérequis Techniques

- **Docker Desktop** (Windows/Mac) ou Docker Engine (Linux)
- **Git** pour cloner le repository
- **4 GB RAM** minimum
- **Ports disponibles** : 1444, 5555, 4300

### Installation Rapide

=== "Docker Compose (Recommandé)"

    ```bash
    # 1. Cloner le repository
    git clone https://github.com/thoumi/Automation.git
    cd Automation

    # 2. Démarrer tous les services
    docker-compose up -d

    # 3. Initialiser la base de données
    docker exec automation-sqlserver /opt/mssql-tools18/bin/sqlcmd \
      -S localhost -U sa -P "Test@Password123" -C \
      -i /docker-entrypoint-initdb.d/init-database.sql

    # 4. Accéder à l'application
    # Frontend : http://localhost:4300
    # Backend  : http://localhost:5555/api
    # Hangfire : http://localhost:5555/hangfire
    ```

=== "Développement Local"

    ```bash
    # 1. Cloner le repository
    git clone https://github.com/thoumi/Automation.git
    cd Automation

    # 2. Backend - Configuration
    cd Backend/AutomationSystem.API
    dotnet restore
    dotnet ef database update
    dotnet run  # Port 5555

    # 3. Frontend - Configuration
    cd ../../Frontend
    npm install
    ng serve  # Port 4200
    ```

### Connexion

**Identifiants par défaut** :

- **Email** : `admin@example.com`
- **Mot de passe** : `admin123`

!!! warning "Important"
    Changez le mot de passe admin après la première connexion !

---

## 🎓 Cas d'Usage Métier

### 1. Vérification Automatique de Routes

**Problème** : Vérification manuelle quotidienne de disponibilité des routes

**Solution** :
- Tâche planifiée quotidienne à 8h
- Appel API CORTEX automatique
- Génération rapport Excel
- Envoi email aux responsables logistique

**Bénéfice** : 2 heures économisées par jour

### 2. Génération de Plans de Staging

**Problème** : Création manuelle de plans chaque lundi

**Solution** :
- Tâche hebdomadaire automatisée
- Extraction données depuis multiple sources
- Génération Excel formaté
- Notifications WhatsApp + Chime

**Bénéfice** : Process de 4h réduit à 5 minutes

### 3. Alertes Temps Réel

**Problème** : Détection tardive des anomalies

**Solution** :
- Monitoring toutes les 15 minutes
- Détection automatique d'anomalies
- Alertes WhatsApp instantanées
- Logs centralisés pour analyse

**Bénéfice** : Réactivité améliorée de 95%

---

## 📈 Résultats et Bénéfices

### Gains de Productivité

| Métrique | Avant | Après | Amélioration |
|----------|-------|-------|--------------|
| **Temps de traitement** | 4h/jour | 15min/jour | **-94%** |
| **Erreurs manuelles** | 5-10/mois | 0-1/mois | **-90%** |
| **Réactivité** | 2-4h | 5min | **-95%** |
| **Coûts opérationnels** | 100% | 25% | **-75%** |

### Statistiques Techniques

- **Lignes de code** : ~12,000
- **Fichiers** : 131
- **Couverture tests** : À venir
- **Performance** : <2s time-to-interactive

---

## 📚 Documentation Complète

- [**Installation Rapide**](quickstart.md) : Démarrage en 5 minutes
- [**Guide Utilisateur**](user-guide/overview.md) : Utilisation complète
- [**Guide Technique**](technical/architecture.md) : Architecture détaillée
- [**Déploiement**](deployment/docker.md) : Docker et production
- [**Support**](support/faq.md) : FAQ et résolution de problèmes

---

## 🤝 Contact et Support

### Liens Utiles

- **Repository GitHub** : [github.com/thoumi/Automation](https://github.com/thoumi/Automation)
- **Issues** : [Signaler un bug](https://github.com/thoumi/Automation/issues)
- **Documentation** : [Site de documentation](https://thoumi.github.io/Automation/)

### Support Technique

- **Email** : support@automation-system.com
- **GitHub Issues** : Pour bugs et feature requests
- **LinkedIn** : [linkedin.com/in/thoumi](https://linkedin.com/in/thoumi)

---

!!! success "Projet en Production"
    Cette plateforme est actuellement utilisée en production et gère plus de **1000 exécutions automatiques par mois** avec un taux de succès de **99.5%**.

---

_Documentation mise à jour : Octobre 2025_
_Développé avec ❤️ pour démontrer l'expertise en automatisation d'entreprise_

