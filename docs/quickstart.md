# Installation Rapide ⚡

Démarrez votre Système d'Automatisation en **moins de 5 minutes** !

---

## 🎯 Prérequis

Avant de commencer, assurez-vous d'avoir :

- [x] **Docker Desktop** installé ([Télécharger](https://www.docker.com/products/docker-desktop))
- [x] **Git** installé ([Télécharger](https://git-scm.com/))
- [x] **4 GB de RAM** disponible minimum
- [x] **Ports libres** : 1444, 5555, 4300

---

## 📥 Étape 1 : Cloner le Repository

```bash
git clone https://github.com/thoumi/Automation.git
cd Automation
```

---

## 🐳 Étape 2 : Démarrer avec Docker

```bash
# Démarrer tous les services (Backend, Frontend, SQL Server)
docker-compose up -d
```

!!! info "Temps de démarrage"
    Le premier démarrage prend 2-3 minutes pour télécharger les images Docker.

---

## 🗄️ Étape 3 : Initialiser la Base de Données

```bash
# Windows PowerShell
docker exec automation-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Test@Password123" -C -i /docker-entrypoint-initdb.d/init-database.sql

# Linux/Mac
docker exec automation-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "Test@Password123" -C \
  -i /docker-entrypoint-initdb.d/init-database.sql
```

---

## 🚀 Étape 4 : Accéder à l'Application

| Service | URL | Description |
|---------|-----|-------------|
| **Frontend** | http://localhost:4300 | Interface utilisateur |
| **Backend API** | http://localhost:5555/api | API REST |
| **Hangfire** | http://localhost:5555/hangfire | Dashboard des jobs |

---

## 🔐 Étape 5 : Se Connecter

**Identifiants par défaut** :

=== "Email"
    ```
    admin@example.com
    ```

=== "Mot de passe"
    ```
    admin123
    ```

!!! warning "Sécurité"
    **Changez immédiatement** le mot de passe après la première connexion !

---

## ✅ Vérification de l'Installation

### 1. Vérifier les Services

```bash
docker-compose ps
```

Vous devriez voir 3 services **running** (UP) :

```
NAME                    STATUS
automation-backend      Up
automation-frontend     Up  
automation-sqlserver    Up
```

### 2. Tester le Backend

```bash
curl http://localhost:5555/api/health
```

Réponse attendue : `Healthy`

### 3. Tester le Frontend

Ouvrez http://localhost:4300 dans votre navigateur. La page de connexion doit s'afficher.

---

## 🎉 Première Tâche

### Créer votre première tâche automatisée

1. **Connectez-vous** avec les identifiants par défaut
2. **Cliquez sur "Tâches"** dans le menu
3. **Cliquez sur "+ Nouvelle Tâche"**
4. **Remplissez** :
   - Nom : "Test Quotidien"
   - Description : "Ma première tâche automatique"
   - Type : "Routenverfuegbarkeit"
   - Planification : Quotidien à 09:00
5. **Activez** et **Créez** !

Votre première tâche est maintenant planifiée ! 🎊

---

## 🆘 Problèmes Courants

### Port déjà utilisé

Si un port est déjà utilisé, modifiez `docker-compose.yml` :

```yaml
ports:
  - "4301:80"  # Au lieu de 4300
```

### SQL Server ne démarre pas

Vérifiez les ressources Docker (minimum 4GB RAM) :

```bash
docker stats
```

### Frontend ne se connecte pas

Vérifiez que `environment.ts` pointe vers le bon port :

```typescript
apiUrl: 'http://localhost:5555/api'
```

---

## 🔧 Commandes Utiles

```bash
# Voir les logs
docker-compose logs -f backend
docker-compose logs -f frontend

# Redémarrer un service
docker-compose restart backend

# Arrêter tout
docker-compose down

# Nettoyer et redémarrer
docker-compose down -v
docker-compose up -d --build
```

---

## 📚 Prochaines Étapes

Maintenant que votre système fonctionne :

1. 📖 [**Découvrir le Tableau de Bord**](user-guide/dashboard.md)
2. ⚙️ [**Créer des Tâches Avancées**](user-guide/tasks.md)
3. 📊 [**Configurer les Notifications**](user-guide/recipients.md)
4. 🔐 [**Sécuriser votre Installation**](deployment/configuration.md)

---

!!! success "Installation Réussie !"
    Votre Système d'Automatisation est opérationnel ! 🚀

