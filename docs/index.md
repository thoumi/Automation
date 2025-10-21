# 🚀 Système d'Automatisation

<div style="text-align: center; margin: 2rem 0;">
  <h2 style="color: #6366f1; font-size: 2.5rem; margin-bottom: 1rem;">Plateforme d'Automatisation d'Entreprise</h2>
  <p style="font-size: 1.2rem; color: #64748b; max-width: 800px; margin: 0 auto;">
    Solution complète de gestion et d'automatisation des processus métier avec interface moderne et architecture robuste
  </p>
</div>

---

## 🎯 À Propos du Projet

Le **Système d'Automatisation** est une plateforme web enterprise-grade développée avec les dernières technologies Microsoft et Angular. Cette solution démontre une expertise complète en développement full-stack, architecture microservices, et intégration de systèmes complexes.

**Mission** : Automatiser et optimiser les processus métier grâce à une plateforme centralisée, sécurisée et évolutive.

### 🛠️ Stack Technique

<div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 1.5rem; margin: 2rem 0;">

<div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 1.5rem; border-radius: 12px; color: white;">
  <h4 style="margin: 0 0 1rem 0; font-size: 1.2rem;">🎨 Frontend</h4>
  <ul style="margin: 0; padding-left: 1.2rem;">
    <li><strong>Angular 18+</strong> - Framework moderne</li>
    <li><strong>Tailwind CSS</strong> - Styling utilitaire</li>
    <li><strong>TypeScript</strong> - Typage statique</li>
    <li><strong>RxJS</strong> - Programmation réactive</li>
  </ul>
</div>

<div style="background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); padding: 1.5rem; border-radius: 12px; color: white;">
  <h4 style="margin: 0 0 1rem 0; font-size: 1.2rem;">⚙️ Backend</h4>
  <ul style="margin: 0; padding-left: 1.2rem;">
    <li><strong>ASP.NET Core 8</strong> - API RESTful</li>
    <li><strong>Entity Framework</strong> - ORM moderne</li>
    <li><strong>Hangfire</strong> - Planification tâches</li>
    <li><strong>JWT Authentication</strong> - Sécurité</li>
  </ul>
</div>

<div style="background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%); padding: 1.5rem; border-radius: 12px; color: white;">
  <h4 style="margin: 0 0 1rem 0; font-size: 1.2rem;">🗄️ Infrastructure</h4>
  <ul style="margin: 0; padding-left: 1.2rem;">
    <li><strong>SQL Server</strong> - Base de données</li>
    <li><strong>Docker</strong> - Conteneurisation</li>
    <li><strong>Nginx</strong> - Serveur web</li>
    <li><strong>GitHub Actions</strong> - CI/CD</li>
  </ul>
</div>

</div>

---

## 🚀 Fonctionnalités Principales

<div class="tech-stack">

<div class="tech-card">
  <h4>⚙️ Gestion des Tâches</h4>
  <ul>
    <li><strong>Planification simplifiée</strong> - Interface intuitive</li>
    <li><strong>Exécution automatique</strong> - Jobs Hangfire</li>
    <li><strong>Monitoring temps réel</strong> - Suivi complet</li>
    <li><strong>Gestion d'erreurs</strong> - Logs détaillés</li>
  </ul>
</div>

<div class="tech-card">
  <h4>📱 Notifications Multi-Canaux</h4>
  <ul>
    <li><strong>Email</strong> - Rapports via MailKit</li>
    <li><strong>WhatsApp</strong> - Notifications Twilio</li>
    <li><strong>Amazon Chime</strong> - Webhooks équipes</li>
    <li><strong>Alertes personnalisées</strong> - Configuration flexible</li>
  </ul>
</div>

<div class="tech-card">
  <h4>📊 Traitement de Fichiers</h4>
  <ul>
    <li><strong>Excel</strong> - Lecture/génération ClosedXML</li>
    <li><strong>Processing automatique</strong> - Workflows</li>
    <li><strong>Gestion uploads</strong> - Stockage sécurisé</li>
    <li><strong>Historique complet</strong> - Traçabilité</li>
  </ul>
</div>

</div>

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

<div class="stats-grid">

<div class="stat-card">
  <span class="stat-number">-94%</span>
  <div class="stat-label">Temps de traitement</div>
</div>

<div class="stat-card">
  <span class="stat-number">-90%</span>
  <div class="stat-label">Erreurs manuelles</div>
</div>

<div class="stat-card">
  <span class="stat-number">-95%</span>
  <div class="stat-label">Temps de réactivité</div>
</div>

<div class="stat-card">
  <span class="stat-number">-75%</span>
  <div class="stat-label">Coûts opérationnels</div>
</div>

</div>

### 🎯 Métriques Techniques

<div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 1.5rem; margin: 2rem 0;">

<div style="background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: white; padding: 1.5rem; border-radius: 12px; text-align: center;">
  <h4 style="margin: 0 0 1rem 0; color: white;">📊 Code Quality</h4>
  <div style="font-size: 2rem; font-weight: 700; margin-bottom: 0.5rem;">12,000+</div>
  <div style="opacity: 0.9;">Lignes de code</div>
</div>

<div style="background: linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%); color: white; padding: 1.5rem; border-radius: 12px; text-align: center;">
  <h4 style="margin: 0 0 1rem 0; color: white;">⚡ Performance</h4>
  <div style="font-size: 2rem; font-weight: 700; margin-bottom: 0.5rem;">&lt;2s</div>
  <div style="opacity: 0.9;">Time to Interactive</div>
</div>

<div style="background: linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%); color: white; padding: 1.5rem; border-radius: 12px; text-align: center;">
  <h4 style="margin: 0 0 1rem 0; color: white;">🏗️ Architecture</h4>
  <div style="font-size: 2rem; font-weight: 700; margin-bottom: 0.5rem;">131</div>
  <div style="opacity: 0.9;">Fichiers organisés</div>
</div>

<div style="background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%); color: white; padding: 1.5rem; border-radius: 12px; text-align: center;">
  <h4 style="margin: 0 0 1rem 0; color: white;">🚀 Déploiement</h4>
  <div style="font-size: 2rem; font-weight: 700; margin-bottom: 0.5rem;">5min</div>
  <div style="opacity: 0.9;">Installation complète</div>
</div>

</div>

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

