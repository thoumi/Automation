# 🚀 DÉMARRER MAINTENANT - Guide Express

**Temps estimé : 5 minutes**

---

## ✅ Prérequis

**Vous devez avoir :**
- ✅ Windows 10/11, macOS ou Linux
- ✅ Docker Desktop installé → https://www.docker.com/products/docker-desktop
- ✅ 4 GB RAM disponible
- ✅ 10 GB espace disque

---

## 🎯 Démarrage en 3 étapes

### Étape 1️⃣ : Copier la configuration de test

**Sur Windows (PowerShell) :**
```powershell
Copy-Item .env.example .env
```

**Sur Mac/Linux :**
```bash
cp .env.example .env
```

**Ou manuellement :** Créez un fichier `.env` et copiez ce contenu :
```
SA_PASSWORD=Test@Password123
EMAIL_USERNAME=test@example.com
EMAIL_PASSWORD=test
TWILIO_ACCOUNT_SID=test
TWILIO_AUTH_TOKEN=test
CHIME_WEBHOOK_URL=https://test.example.com
CORTEX_API_KEY=test
```

### Étape 2️⃣ : Démarrer l'application

**Sur Windows :**
- Double-cliquez sur `start.bat`

**Sur Mac/Linux :**
```bash
chmod +x start.sh
./start.sh
```

**Ou utilisez Docker directement :**
```bash
docker-compose up -d
```

### Étape 3️⃣ : Ouvrir l'application

Attendez 30 secondes, puis ouvrez :

🌐 **http://localhost:4200**

**Connexion :**
- Username : `admin`
- Password : `admin`

---

## 🎉 C'EST FAIT !

Vous devriez voir le **Dashboard** avec les statistiques.

---

## 🧪 Tests rapides

### Test 1 : Voir les tâches planifiées (30 secondes)
1. Cliquez sur **"Tâches"** dans le menu
2. Vous voyez 3 tâches par défaut
3. ✅ Fonctionnel !

### Test 2 : Exécuter une tâche (1 minute)
1. Menu **Tâches**
2. Cliquez sur **"Exécuter"** pour "Routenverfügbarkeit"
3. Menu **Logs**
4. Vous voyez l'exécution avec son statut
5. ✅ Fonctionnel !

### Test 3 : Uploader un fichier (2 minutes)
1. Créez un fichier Excel simple (ex: liste de noms)
2. Menu **Fichiers**
3. Cliquez sur **"Télécharger un fichier"**
4. Sélectionnez votre fichier
5. Il apparaît dans la liste
6. ✅ Fonctionnel !

### Test 4 : Voir Hangfire (1 minute)
1. Menu **Paramètres**
2. Cliquez sur **"Ouvrir Hangfire Dashboard"**
3. Vous voyez toutes les tâches en temps réel
4. ✅ Fonctionnel !

---

## 🔗 URLs importantes

| Service | URL | Description |
|---------|-----|-------------|
| **Application** | http://localhost:4200 | Interface principale |
| **API** | http://localhost:5000 | Backend REST API |
| **Hangfire** | http://localhost:5000/hangfire | Monitoring des tâches |
| **Swagger** | http://localhost:5000/swagger | Documentation API |

---

## 🛑 Arrêter l'application

**Sur Windows/Mac/Linux :**
```bash
docker-compose down
```

---

## 🐛 Problèmes ?

### ❌ "Port already in use" (port déjà utilisé)

**Solution :** Changez les ports dans `docker-compose.yml`

```yaml
# Frontend : ligne ~60
ports:
  - "4300:80"  # Au lieu de 4200

# Backend : ligne ~40
ports:
  - "6000:80"  # Au lieu de 5000
```

Puis : http://localhost:4300

### ❌ "Cannot connect to Docker daemon"

**Solution :** Lancez Docker Desktop

### ❌ Page blanche sur http://localhost:4200

**Solution :** Attendez 1-2 minutes de plus

```bash
# Vérifier que tout tourne
docker-compose ps

# Tous les services doivent être "Up"
```

### ❌ "Error: Cannot find module"

**Solution :** Reconstruisez les images

```bash
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

---

## 📚 Documentation complète

- 📖 **Guide utilisateur** : [GUIDE_CLIENT.md](GUIDE_CLIENT.md)
- 🚀 **Démarrage rapide** : [QUICK_START.md](QUICK_START.md)
- 🛠️ **Documentation technique** : [README.md](README.md)
- 🔧 **Installation** : [INSTALLATION.md](INSTALLATION.md)

---

## ✅ Checklist de test

- [ ] Application démarre sans erreur
- [ ] Connexion réussie (admin/admin)
- [ ] Dashboard affiché
- [ ] 3 tâches visibles dans "Tâches"
- [ ] Exécution manuelle fonctionne
- [ ] Logs affichés
- [ ] Upload de fichier fonctionne
- [ ] Hangfire accessible

**Si tout est ✅ → Félicitations ! L'application fonctionne !**

---

## 🎯 Prochaines étapes

### Pour tester avec de vrais services :

1. **Configurez vos comptes** dans le fichier `.env` :
   - Compte Email (Outlook/Gmail)
   - Compte Twilio (WhatsApp)
   - Webhook Chime
   - API CORTEX

2. **Redémarrez** :
   ```bash
   docker-compose restart
   ```

3. **Testez les notifications** réelles

### Pour personnaliser :

- Créez vos propres tâches via l'interface
- Ajoutez vos destinataires
- Configurez vos horaires

### Pour déployer en production :

Consultez : [DEPLOYMENT.md](DEPLOYMENT.md)

---

## 💬 Besoin d'aide ?

**Documentation :**
- Tout est dans les fichiers .md à la racine

**Vérifications :**
```bash
# Voir les logs en temps réel
docker-compose logs -f

# Vérifier l'état des conteneurs
docker-compose ps

# Redémarrer un service spécifique
docker-compose restart backend
```

---

**Bon test ! 🎉**

Si tout fonctionne, vous pouvez partager ce projet avec votre client en utilisant le guide [PACKAGE_CLIENT.md](PACKAGE_CLIENT.md).

