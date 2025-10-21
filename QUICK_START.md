# 🚀 Démarrage Rapide - 5 Minutes

Guide ultra-rapide pour démarrer l'application en mode test.

## ⚡ Méthode Express (Docker)

### Étape 1 : Prérequis
- Docker Desktop installé et démarré
- Rien d'autre !

### Étape 2 : Configuration minimale

Créez un fichier `.env` à la racine du projet :

```bash
# Copier ce contenu dans .env
SA_PASSWORD=Test@Password123
EMAIL_USERNAME=test@example.com
EMAIL_PASSWORD=test
TWILIO_ACCOUNT_SID=test
TWILIO_AUTH_TOKEN=test
CHIME_WEBHOOK_URL=https://test.example.com
CORTEX_API_KEY=test
```

### Étape 3 : Démarrer

```bash
# Dans le dossier racine
docker-compose up -d
```

⏱️ **Attendez 1-2 minutes** que tout démarre...

### Étape 4 : Tester

Ouvrez votre navigateur :
- **Application** : http://localhost:4200
- **Connexion** : `admin` / `admin`

🎉 **C'est tout !**

---

## 🔧 Démarrage sans Docker

### Étape 1 : Backend

```bash
cd Backend/AutomationSystem.API

# Configuration minimale - créer appsettings.Test.json
{
  "ConnectionStrings": {
    "SqlServer": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AutomationTest;Integrated Security=True;"
  }
}

# Démarrer
dotnet run --environment Test
```

### Étape 2 : Frontend

```bash
cd Frontend

# Installer et démarrer
npm install
npm start
```

### Étape 3 : Accéder

- **Frontend** : http://localhost:4200
- **Backend API** : https://localhost:5001
- **Hangfire** : https://localhost:5001/hangfire
- **Swagger** : https://localhost:5001/swagger

---

## ✅ Vérifications rapides

### 1. Backend fonctionne ?
```bash
curl http://localhost:5000/swagger
# Vous devriez voir la page Swagger
```

### 2. Base de données créée ?
```bash
docker exec -it automation-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Test@Password123" -Q "SELECT name FROM sys.databases"
# Vous devriez voir "AutomationSystem"
```

### 3. Frontend accessible ?
```bash
curl http://localhost:4200
# Vous devriez voir le HTML de l'application
```

---

## 🎯 Premiers tests

### Test 1 : Connexion
1. Allez sur http://localhost:4200
2. Login : `admin` / `admin`
3. Vous devriez voir le Dashboard

### Test 2 : Voir les tâches planifiées
1. Menu **Tâches**
2. Vous devriez voir 3 tâches par défaut

### Test 3 : Exécuter une tâche manuellement
1. Menu **Tâches**
2. Cliquez sur **Exécuter** sur une tâche
3. Menu **Logs** pour voir le résultat

### Test 4 : Uploader un fichier
1. Menu **Fichiers**
2. Créez un fichier Excel simple
3. Uploadez-le
4. Vérifiez qu'il apparaît dans la liste

---

## 🛑 Arrêter l'application

```bash
# Avec Docker
docker-compose down

# Sans Docker : Ctrl+C dans chaque terminal
```

---

## 🐛 Problèmes courants

### Port déjà utilisé
```bash
# Changer les ports dans docker-compose.yml
# Frontend : 4200 → 4300
# Backend : 5000 → 6000
```

### Docker ne démarre pas
```bash
# Vérifier que Docker Desktop est lancé
# Redémarrer Docker Desktop
# Réessayer : docker-compose up -d
```

### Base de données inaccessible
```bash
# Attendre 30 secondes de plus
# Vérifier les logs : docker-compose logs sqlserver
```

---

## 📞 Besoin d'aide ?

Consultez :
- **Documentation complète** : [README.md](README.md)
- **Installation détaillée** : [INSTALLATION.md](INSTALLATION.md)

