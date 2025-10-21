# 📱 Guide Client - Système d'Automatisation

**Version de démonstration pour tests**

---

## 🎯 Qu'est-ce que c'est ?

Un système d'automatisation qui vous permet de :
- ✅ Planifier des tâches automatiques (envoi de rapports, traitement de fichiers)
- ✅ Traiter des fichiers Excel automatiquement
- ✅ Envoyer des notifications par Email, WhatsApp et Chime
- ✅ Consulter l'historique de toutes les opérations
- ✅ Gérer vos destinataires de notifications

---

## 🚀 Comment démarrer ?

### Option 1 : Avec Docker (Recommandé) ⭐

**Prérequis** : Avoir Docker Desktop installé
- Windows/Mac : https://www.docker.com/products/docker-desktop

**Étapes** :

1. **Téléchargez le projet** (fichier ZIP fourni)
2. **Décompressez-le** dans un dossier de votre choix
3. **Ouvrez un terminal/PowerShell** dans ce dossier
4. **Exécutez** :
   ```bash
   docker-compose up -d
   ```
5. **Attendez 1-2 minutes** que tout démarre
6. **Ouvrez votre navigateur** sur : http://localhost:4200

**Identifiants de test** :
- Username : `admin`
- Password : `admin`

### Option 2 : Installation manuelle

Si vous ne pouvez pas utiliser Docker, suivez le guide [INSTALLATION.md](INSTALLATION.md)

---

## 📖 Guide d'utilisation

### 1️⃣ Page d'accueil (Dashboard)

![Dashboard](docs/images/dashboard.png)

**Ce que vous voyez** :
- 📊 **Statistiques** : Nombre de tâches réussies/échouées
- 📋 **Tâches planifiées** : Liste des automatisations actives
- 📝 **Exécutions récentes** : Dernières opérations effectuées
- 📈 **Performance** : Taux de succès par tâche

### 2️⃣ Gestion des tâches

**Menu : Tâches**

#### Créer une nouvelle tâche
1. Cliquez sur **"Nouvelle tâche"**
2. Remplissez :
   - **Nom** : Ex. "Rapport quotidien"
   - **Type** : Choisissez le type de tâche
   - **Description** : Expliquez ce que fait la tâche
   - **Horaire (Cron)** : Quand elle doit s'exécuter
3. Cochez **"Activée"** pour la lancer automatiquement
4. Cliquez sur **"Créer"**

#### Exemples d'horaires (Cron)
- `0 9 * * *` → Tous les jours à 9h00
- `0 */2 * * *` → Toutes les 2 heures
- `30 14 * * 1-5` → Du lundi au vendredi à 14h30
- `0 0 1 * *` → Le 1er de chaque mois à minuit

💡 **Astuce** : Utilisez https://crontab.guru/ pour créer vos horaires

#### Exécuter une tâche manuellement
1. Dans la liste des tâches
2. Cliquez sur **"Exécuter"**
3. La tâche démarre immédiatement
4. Allez dans **"Logs"** pour voir le résultat

### 3️⃣ Consultation des logs

**Menu : Logs**

**Filtres disponibles** :
- 🔍 Par nom de tâche
- ✅ Par statut (Succès/Échec/Avertissement)
- 📅 Par période

**Détails d'un log** :
- Cliquez sur une ligne pour voir tous les détails
- Vous verrez : message, durée, erreurs éventuelles

### 4️⃣ Gestion des fichiers

**Menu : Fichiers**

#### Uploader un fichier Excel
1. Cliquez sur **"Télécharger un fichier"**
2. Sélectionnez votre fichier `.xlsx`
3. Le système le traite automatiquement
4. Vous voyez le statut de traitement

**Formats acceptés** :
- ✅ Excel (.xlsx, .xls)
- ✅ Taille max : 50 MB

#### Télécharger un fichier
1. Dans la liste, cliquez sur **"Télécharger"**
2. Le fichier est sauvegardé sur votre ordinateur

### 5️⃣ Gestion des destinataires

**Menu : Destinataires**

#### Ajouter un destinataire
1. Cliquez sur **"Nouveau destinataire"**
2. Choisissez le type :
   - **Email** : Pour recevoir par email
   - **WhatsApp** : Pour recevoir sur WhatsApp
   - **Chime** : Pour notifications d'équipe
3. Entrez les informations requises
4. Cochez **"Actif"**
5. Cliquez sur **"Créer"**

#### Désactiver temporairement un destinataire
- Cliquez sur **"Désactiver"** (il ne recevra plus de notifications)
- Pour réactiver : cliquez sur **"Activer"**

### 6️⃣ Paramètres

**Menu : Paramètres**

**Accès aux outils d'administration** :
- **Hangfire Dashboard** : Voir toutes les tâches en temps réel
- **Swagger API** : Documentation technique de l'API

---

## 🎮 Scénarios de test

### Test 1 : Tâche simple
**Objectif** : Créer et exécuter votre première tâche

1. Menu **Tâches** → **Nouvelle tâche**
2. Nom : "Test quotidien"
3. Type : "Routenverfuegbarkeit"
4. Horaire : `0 10 * * *` (tous les jours à 10h)
5. Cochez "Activée"
6. Créer
7. Cliquez sur **"Exécuter"** pour tester immédiatement
8. Allez dans **Logs** pour voir le résultat

### Test 2 : Upload de fichier
**Objectif** : Traiter un fichier Excel

1. Créez un fichier Excel simple avec quelques données
2. Menu **Fichiers** → **Télécharger un fichier**
3. Sélectionnez votre fichier
4. Attendez le traitement (status change)
5. Téléchargez-le à nouveau pour vérifier

### Test 3 : Gérer les notifications
**Objectif** : Configurer les destinataires

1. Menu **Destinataires** → **Nouveau destinataire**
2. Type : Email
3. Email : votre-email@example.com
4. Nom : "Moi"
5. Actif : ✅
6. Créer
7. Testez en exécutant une tâche (si email configuré)

---

## 📊 Monitoring

### Tableau de bord Hangfire

**URL** : http://localhost:5000/hangfire

**Vous pouvez** :
- 👀 Voir toutes les tâches en cours d'exécution
- 📜 Consulter l'historique complet
- 🔄 Relancer des tâches échouées
- 📈 Voir des statistiques détaillées

### Logs système

**Menu Logs de l'application** :
- Tous les événements sont enregistrés
- Filtrez par date, statut, tâche
- Cliquez sur une ligne pour les détails complets

---

## 🔐 Sécurité

### En mode test
- ⚠️ Les identifiants par défaut sont : `admin` / `admin`
- ⚠️ Ne pas utiliser en production tel quel

### Pour la production
Le système supporte :
- ✅ Authentification Azure AD
- ✅ JWT sécurisé
- ✅ HTTPS obligatoire
- ✅ Gestion des secrets avec Azure Key Vault

---

## 🐛 Problèmes courants

### Je ne peux pas me connecter
- Vérifiez que l'URL est bien `http://localhost:4200`
- Essayez `admin` / `admin`
- Videz le cache du navigateur (Ctrl+Shift+R)

### La page est blanche
- Attendez 30 secondes de plus (le backend démarre)
- Vérifiez dans Docker Desktop que les 3 conteneurs sont "Running"
- Redémarrez : `docker-compose restart`

### Une tâche échoue
- Menu **Logs** → Cliquez sur la tâche échouée
- Lisez le message d'erreur
- Les causes communes :
  - Services externes non configurés (email, WhatsApp)
  - Fichier Excel manquant
  - Format de données incorrect

### Le fichier ne s'upload pas
- Vérifiez la taille (max 50 MB)
- Format supporté : .xlsx, .xls
- Essayez avec un fichier plus simple

---

## 📞 Support

### Questions fréquentes

**Q : Puis-je utiliser mes propres comptes email ?**
R : Oui, modifiez la configuration dans `appsettings.json` (section Email)

**Q : Comment ajouter plus de tâches ?**
R : Via l'interface ou en créant de nouvelles classes Job dans le code

**Q : Les données sont-elles sauvegardées ?**
R : Oui, tout est stocké dans la base de données SQL Server

**Q : Puis-je accéder depuis un autre ordinateur ?**
R : En local non, mais vous pouvez déployer sur un serveur

### Contact

Pour toute question technique :
- 📧 Email : [votre-email]
- 📄 Documentation complète : README.md
- 🔧 Guide technique : INSTALLATION.md

---

## 🎯 Prochaines étapes

Après avoir testé :

1. **Retour d'expérience** : Notez ce qui fonctionne bien et les améliorations souhaitées
2. **Configuration production** : Si validé, on configure avec vos vrais comptes
3. **Personnalisation** : Ajout de vos tâches spécifiques
4. **Déploiement** : Mise en production sur Azure/AWS

---

## ✅ Checklist de test

- [ ] Connexion réussie
- [ ] Dashboard affiché correctement
- [ ] Création d'une tâche
- [ ] Exécution manuelle d'une tâche
- [ ] Consultation des logs
- [ ] Upload d'un fichier Excel
- [ ] Ajout d'un destinataire
- [ ] Navigation dans Hangfire Dashboard
- [ ] Test de toutes les pages du menu

---

**Merci de tester notre système d'automatisation !** 🚀

N'hésitez pas à nous faire vos retours pour améliorer l'application.

