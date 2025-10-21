# 📗 Documentation Fonctionnelle - Système d'Automatisation

## Table des matières

1. [Introduction](#1-introduction)
2. [Accès à l'Application](#2-accès-à-lapplication)
3. [Tableau de Bord](#3-tableau-de-bord)
4. [Gestion des Tâches](#4-gestion-des-tâches)
5. [Planification Simplifiée](#5-planification-simplifiée)
6. [Historique et Logs](#6-historique-et-logs)
7. [Gestion des Destinataires](#7-gestion-des-destinataires)
8. [Gestion des Fichiers](#8-gestion-des-fichiers)
9. [Cas d'Usage](#9-cas-dusage)
10. [FAQ](#10-faq)

---

## 1. Introduction

### Qu'est-ce que le Système d'Automatisation ?

Le Système d'Automatisation est une plateforme qui permet d'**automatiser des tâches répétitives** dans votre entreprise. Il peut :

✅ **Exécuter des tâches automatiquement** selon un horaire que vous définissez  
✅ **Traiter des fichiers Excel** et générer des rapports  
✅ **Envoyer des notifications** par email, WhatsApp ou Amazon Chime  
✅ **Se connecter à vos APIs** existantes (CORTEX, etc.)  
✅ **Vous alerter en cas de problème**  

### Qui peut utiliser cette application ?

- **Administrateurs** : Configuration complète du système
- **Gestionnaires** : Création et suivi des tâches
- **Utilisateurs finaux** : Consultation des rapports et logs

---

## 2. Accès à l'Application

### 2.1 Connexion

1. Ouvrez votre navigateur web
2. Accédez à : **http://localhost:4300** (ou l'URL de votre serveur)
3. Entrez vos identifiants :
   - **Email** : `admin@example.com`
   - **Mot de passe** : `admin123`
4. Cliquez sur **"Se connecter"**

![Écran de connexion](docs/images/login.png)

### 2.2 Première Connexion

**⚠️ Important** : Changez le mot de passe par défaut après votre première connexion !

1. Allez dans **Paramètres** (icône engrenage)
2. Cliquez sur **"Changer le mot de passe"**
3. Entrez votre nouveau mot de passe (minimum 8 caractères)
4. Confirmez

---

## 3. Tableau de Bord

### 3.1 Vue d'ensemble

Le **Tableau de Bord** est la première page que vous voyez après connexion. Il affiche :

#### 📊 Statistiques Principales

- **Nombre total de tâches** actives
- **Exécutions aujourd'hui**
- **Taux de succès** (%)
- **Dernière exécution**

#### 📈 Graphiques

- **Graphique des exécutions** (7 derniers jours)
- **Répartition par tâche** (succès vs échecs)

#### 📝 Exécutions Récentes

Liste des 10 dernières exécutions avec :
- Nom de la tâche
- Statut (Succès ✅ / Échec ❌ / En cours ⏳)
- Date et heure
- Durée d'exécution

### 3.2 Interprétation des Statuts

| Statut | Icône | Signification |
|--------|-------|---------------|
| **Succès** | ✅ | La tâche s'est exécutée correctement |
| **Échec** | ❌ | Une erreur s'est produite |
| **En cours** | ⏳ | La tâche est actuellement en cours d'exécution |
| **En attente** | 🕐 | La tâche attend son heure d'exécution |

---

## 4. Gestion des Tâches

### 4.1 Qu'est-ce qu'une Tâche ?

Une **tâche** est une action automatisée que le système va exécuter selon un horaire défini. Par exemple :

- Vérifier la disponibilité des routes chaque matin à 9h
- Générer un rapport Excel tous les lundis
- Envoyer un résumé hebdomadaire par email

### 4.2 Créer une Nouvelle Tâche

1. **Cliquez sur "Tâches"** dans le menu de gauche
2. **Cliquez sur le bouton "+ Nouvelle Tâche"**
3. **Remplissez le formulaire** :

#### Informations Générales

- **Nom** : Un nom descriptif (ex: "Vérification Routes Quotidienne")
- **Description** : Expliquez ce que fait la tâche
- **Type de tâche** : Choisissez parmi :
  - `Routenverfuegbarkeit` : Vérification disponibilité routes
  - `StagingPlan` : Génération de plans de staging
  - `DNRUnits` : Extraction unités DNR

#### Planification

Voir section [5. Planification Simplifiée](#5-planification-simplifiée)

#### Configuration (Optionnel)

Si votre tâche nécessite des paramètres spécifiques, entrez-les au format JSON :

```json
{
  "source": "CORTEX",
  "maxResults": 100,
  "includeArchived": false
}
```

4. **Cochez "Activer la tâche"** si vous voulez qu'elle démarre immédiatement
5. **Cliquez sur "Créer"**

### 4.3 Modifier une Tâche Existante

1. Dans la liste des tâches, cliquez sur l'icône **✏️ Modifier**
2. Modifiez les champs souhaités
3. Cliquez sur **"Enregistrer"**

### 4.4 Activer / Désactiver une Tâche

Pour mettre en pause temporairement une tâche sans la supprimer :

1. Trouvez la tâche dans la liste
2. Cliquez sur le **toggle** (interrupteur) à côté du nom
3. Confirmez l'action

**💡 Astuce** : Désactivez les tâches pendant les périodes de maintenance.

### 4.5 Exécuter Manuellement une Tâche

Parfois, vous voulez exécuter une tâche immédiatement sans attendre l'horaire :

1. Cliquez sur l'icône **▶️ Exécuter** à côté de la tâche
2. Confirmez l'exécution
3. Un message de confirmation s'affiche
4. Consultez les logs pour voir le résultat

### 4.6 Supprimer une Tâche

⚠️ **Attention** : Cette action est irréversible !

1. Cliquez sur l'icône **🗑️ Supprimer**
2. Confirmez en tapant le nom de la tâche
3. Validez

---

## 5. Planification Simplifiée

### 5.1 Pourquoi un Système Simplifié ?

Au lieu de comprendre les expressions CRON complexes (`0 9 * * 1`), notre système utilise **une interface visuelle intuitive** en français.

### 5.2 Types de Planification

#### 🕐 Toutes les X Minutes

Pour des tâches très fréquentes (ex: monitoring).

**Exemple** : Toutes les 15 minutes
```
Fréquence : Minute
Intervalle : 15
```

#### 🕐 Toutes les X Heures

Pour des tâches régulières dans la journée.

**Exemple** : Toutes les 3 heures
```
Fréquence : Heure
Intervalle : 3
```

#### 📅 Quotidien (Chaque Jour)

Pour des tâches journalières à heure fixe.

**Exemple** : Tous les jours à 9h00
```
Fréquence : Quotidien
Heure : 09:00
```

#### 📅 Hebdomadaire (Chaque Semaine)

Pour des tâches hebdomadaires un jour précis.

**Exemple** : Chaque lundi à 14h30
```
Fréquence : Hebdomadaire
Jour : Lundi
Heure : 14:30
```

#### 📅 Mensuel (Chaque Mois)

Pour des tâches mensuelles à une date fixe.

**Exemple** : Le 1er de chaque mois à 8h00
```
Fréquence : Mensuel
Jour du mois : 1
Heure : 08:00
```

### 5.3 Aperçu de la Planification

Sous le formulaire de planification, vous verrez un **résumé en français** :

> 📘 **Résumé** : Chaque lundi à 14:30

Cela vous permet de vérifier que vous avez bien configuré ce que vous souhaitez.

---

## 6. Historique et Logs

### 6.1 Consulter l'Historique

1. Cliquez sur **"Logs"** dans le menu de gauche
2. Vous voyez la liste de toutes les exécutions

### 6.2 Filtrer les Logs

Utilisez les filtres en haut de page :

- **Par tâche** : Afficher uniquement une tâche spécifique
- **Par statut** : Succès, Échec, En cours
- **Par date** : Aujourd'hui, Cette semaine, Ce mois, Personnalisé

**Exemple** : Pour voir tous les échecs de la semaine :
```
Statut : Échec
Date : Cette semaine
```

### 6.3 Voir les Détails d'une Exécution

Cliquez sur une ligne pour voir :

- **Heure de début et de fin**
- **Durée totale**
- **Message de succès / erreur**
- **Données de sortie** (résultats JSON)

**En cas d'échec**, le message d'erreur vous indique ce qui s'est mal passé :

```
❌ Erreur : Impossible de se connecter à l'API CORTEX
Détails : Connection timeout après 30 secondes
```

### 6.4 Exporter les Logs

Pour créer un rapport :

1. Appliquez vos filtres
2. Cliquez sur **"Exporter"**
3. Choisissez le format (Excel, CSV, PDF)
4. Le fichier se télécharge automatiquement

---

## 7. Gestion des Destinataires

### 7.1 Qu'est-ce qu'un Destinataire ?

Un **destinataire** est une personne ou un canal qui recevra les notifications automatiques (rapports, alertes).

### 7.2 Ajouter un Destinataire

1. Cliquez sur **"Destinataires"** dans le menu
2. Cliquez sur **"+ Nouveau Destinataire"**
3. Choisissez le **type de notification** :

#### 📧 Email

- **Identifiant** : Adresse email (ex: `rapport@entreprise.com`)
- **Utilisation** : Rapports détaillés, pièces jointes Excel

#### 📱 WhatsApp

- **Identifiant** : Numéro de téléphone au format international (ex: `+33612345678`)
- **Utilisation** : Alertes urgentes, notifications rapides

#### 💬 Amazon Chime

- **Identifiant** : URL du webhook Chime
- **Utilisation** : Notifications d'équipe, intégration avec Chime

4. Cochez **"Actif"** pour que le destinataire reçoive les notifications
5. Cliquez sur **"Ajouter"**

### 7.3 Désactiver Temporairement un Destinataire

Vous pouvez désactiver un destinataire sans le supprimer :

1. Trouvez le destinataire dans la liste
2. Décochez **"Actif"**
3. Il ne recevra plus de notifications jusqu'à réactivation

### 7.4 Supprimer un Destinataire

1. Cliquez sur l'icône **🗑️ Supprimer**
2. Confirmez l'action

---

## 8. Gestion des Fichiers

### 8.1 Uploader un Fichier

Pour traiter un fichier Excel manuellement :

1. Cliquez sur **"Fichiers"** dans le menu
2. Cliquez sur **"📁 Upload"**
3. Sélectionnez votre fichier (`.xlsx`, `.xls`)
4. Le fichier est automatiquement uploadé et traité
5. Consultez le statut de traitement

### 8.2 Statuts de Traitement

| Statut | Signification |
|--------|---------------|
| **En attente** | Le fichier est uploadé, traitement pas encore commencé |
| **En cours** | Le fichier est en cours de traitement |
| **Traité** | Traitement réussi, résultats disponibles |
| **Échec** | Erreur pendant le traitement |

### 8.3 Télécharger un Fichier Traité

1. Trouvez le fichier dans la liste
2. Cliquez sur **"⬇️ Télécharger"**
3. Le fichier se télécharge automatiquement

### 8.4 Supprimer un Fichier

1. Cliquez sur **"🗑️ Supprimer"**
2. Confirmez l'action
3. Le fichier est supprimé du serveur

---

## 9. Cas d'Usage

### 9.1 Vérification Quotidienne des Routes

**Objectif** : Vérifier chaque matin à 8h si les routes sont disponibles et envoyer un rapport par email.

**Configuration** :

1. **Créer la tâche** :
   - Nom : "Vérification Routes Quotidienne"
   - Type : `Routenverfuegbarkeit`
   - Planification : Quotidien à 08:00

2. **Ajouter un destinataire email** :
   - Type : Email
   - Identifiant : `logistique@entreprise.com`

3. **Résultat** :
   - Chaque jour à 8h, le système vérifie les routes via l'API CORTEX
   - Un rapport Excel est généré
   - Le rapport est envoyé par email à `logistique@entreprise.com`

### 9.2 Alertes WhatsApp en Cas de Problème

**Objectif** : Recevoir une alerte WhatsApp immédiate si une tâche critique échoue.

**Configuration** :

1. **Ajouter un destinataire WhatsApp** :
   - Type : WhatsApp
   - Identifiant : `+33612345678`

2. **Configuration automatique** :
   - Le système envoie automatiquement un message WhatsApp en cas d'échec d'une tâche critique

3. **Message reçu** :
   ```
   🚨 Alerte Système d'Automatisation
   
   La tâche "Vérification Routes" a échoué à 08:05
   
   Erreur : Impossible de se connecter à l'API CORTEX
   
   Consultez les logs pour plus de détails.
   ```

### 9.3 Rapport Hebdomadaire de Performance

**Objectif** : Recevoir chaque vendredi un résumé de la semaine avec les statistiques.

**Configuration** :

1. **Créer la tâche** :
   - Nom : "Rapport Hebdomadaire"
   - Type : `StagingPlan`
   - Planification : Hebdomadaire, Vendredi à 17:00

2. **Ajouter destinataires** :
   - Email : `direction@entreprise.com`
   - Chime : (webhook de votre canal Chime "Rapports")

3. **Résultat** :
   - Chaque vendredi à 17h, un rapport Excel est généré
   - Envoyé par email + notification Chime

### 9.4 Traitement de Fichiers en Batch

**Objectif** : Traiter automatiquement tous les fichiers Excel uploadés.

**Configuration** :

1. **Upload de fichiers** : Les utilisateurs uploadent des fichiers via l'interface
2. **Traitement automatique** : Une tâche toutes les heures traite les fichiers en attente
3. **Notification** : Email envoyé quand le traitement est terminé

---

## 10. FAQ

### Questions Générales

**Q : Puis-je créer plusieurs tâches avec le même nom ?**  
R : Oui, mais ce n'est pas recommandé pour éviter la confusion. Utilisez des noms uniques et descriptifs.

**Q : Combien de tâches puis-je créer ?**  
R : Il n'y a pas de limite technique, mais nous recommandons de ne pas dépasser 50 tâches actives pour des raisons de performance.

**Q : Les tâches s'exécutent-elles si le serveur est éteint ?**  
R : Non. Les tâches manquées pendant l'arrêt du serveur ne seront pas exécutées rétroactivement.

### Planification

**Q : Que se passe-t-il si une tâche est encore en cours à l'heure de la prochaine exécution ?**  
R : La nouvelle exécution sera mise en file d'attente et démarrera dès que la précédente se termine.

**Q : Puis-je exécuter une tâche plusieurs fois par jour à des heures différentes ?**  
R : Actuellement non. Créez plusieurs tâches distinctes avec des horaires différents.

**Q : Comment annuler une tâche en cours d'exécution ?**  
R : Allez dans "Logs", trouvez la tâche en cours, et cliquez sur "Annuler". (Fonctionnalité à venir)

### Notifications

**Q : Pourquoi mes emails ne sont pas envoyés ?**  
R : Vérifiez :
   1. Que le destinataire est actif
   2. Les paramètres SMTP dans la configuration
   3. Que votre serveur mail n'a pas bloqué l'adresse

**Q : Les messages WhatsApp coûtent-ils de l'argent ?**  
R : Oui, via Twilio. Consultez la tarification Twilio pour votre région.

**Q : Puis-je envoyer des notifications à plusieurs personnes ?**  
R : Oui, ajoutez simplement plusieurs destinataires. Tous les destinataires actifs recevront les notifications.

### Fichiers

**Q : Quels types de fichiers sont supportés ?**  
R : Actuellement : `.xlsx`, `.xls` (Excel). Support PDF et images à venir.

**Q : Quelle est la taille maximale de fichier ?**  
R : 50 MB par défaut. Contactez l'administrateur pour augmenter cette limite.

**Q : Les fichiers sont-ils supprimés automatiquement ?**  
R : Oui, les fichiers de plus de 30 jours sont automatiquement supprimés pour libérer de l'espace.

### Sécurité

**Q : Mes données sont-elles sécurisées ?**  
R : Oui :
   - Mots de passe hashés (BCrypt)
   - Connexion sécurisée (HTTPS en production)
   - Authentification JWT
   - Accès restreint par utilisateur

**Q : Combien de temps reste-t-on connecté ?**  
R : 60 minutes d'inactivité. Après, vous devrez vous reconnecter.

**Q : Puis-je partager mon compte avec un collègue ?**  
R : Non, chaque utilisateur doit avoir son propre compte pour des raisons de sécurité et de traçabilité.

### Problèmes Courants

**Q : J'ai oublié mon mot de passe, que faire ?**  
R : Contactez l'administrateur système qui pourra réinitialiser votre mot de passe.

**Q : La page ne se charge pas, que faire ?**  
R : 
   1. Vérifiez votre connexion internet
   2. Effacez le cache de votre navigateur
   3. Essayez avec un autre navigateur
   4. Contactez le support si le problème persiste

**Q : Les graphiques ne s'affichent pas correctement.**  
R : Utilisez un navigateur moderne (Chrome, Firefox, Edge dernière version). Internet Explorer n'est pas supporté.

---

## Support et Assistance

### Contacter le Support

- **Email** : support@votreentreprise.com
- **Téléphone** : +33 X XX XX XX XX
- **Heures d'ouverture** : Lundi-Vendredi, 9h-18h

### Signaler un Bug

1. Allez sur : https://github.com/thoumi/Automation/issues
2. Cliquez sur "New Issue"
3. Décrivez le problème en détail avec captures d'écran si possible

### Demander une Nouvelle Fonctionnalité

Même procédure que pour un bug, mais choisissez le template "Feature Request".

---

## Glossaire

| Terme | Définition |
|-------|------------|
| **Tâche** | Action automatisée exécutée selon un horaire |
| **CRON** | Format standard pour définir des horaires d'exécution |
| **Job** | Autre terme pour tâche, utilisé techniquement |
| **Hangfire** | Système de gestion des tâches en arrière-plan |
| **Webhook** | URL qui reçoit des notifications automatiques |
| **JWT** | Token d'authentification sécurisé |
| **API** | Interface permettant la communication entre systèmes |
| **CORTEX** | API externe utilisée pour récupérer des données |

---

**Document mis à jour** : Octobre 2025  
**Version** : 2.0  
**Pour** : Utilisateurs finaux et gestionnaires

