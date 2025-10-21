# FAQ - Foire Aux Questions

Trouvez rapidement des réponses aux questions les plus fréquentes ! 💡

---

## 🔐 Authentification et Accès

### Comment changer mon mot de passe ?

1. Connectez-vous à l'application
2. Cliquez sur l'icône de profil (en haut à droite)
3. Sélectionnez "Paramètres"
4. Cliquez sur "Changer le mot de passe"
5. Entrez votre mot de passe actuel et le nouveau
6. Validez

### J'ai oublié mon mot de passe

Contactez l'administrateur système qui pourra réinitialiser votre mot de passe.

### Combien de temps puis-je rester connecté ?

**60 minutes d'inactivité**. Après ce délai, vous devrez vous reconnecter pour des raisons de sécurité.

---

## ⚙️ Tâches et Automatisation

### Combien de tâches puis-je créer ?

Il n'y a **pas de limite technique**, mais nous recommandons de ne pas dépasser **50 tâches actives** simultanées pour maintenir des performances optimales.

### Puis-je créer plusieurs tâches avec le même nom ?

Oui, mais ce n'est **pas recommandé**. Utilisez des noms uniques et descriptifs pour éviter les confusions.

### Que se passe-t-il si une tâche est encore en cours à l'heure de la prochaine exécution ?

La nouvelle exécution sera **mise en file d'attente** et démarrera dès que la précédente se termine. Elles ne s'exécutent jamais en parallèle.

### Les tâches s'exécutent-elles si le serveur est éteint ?

**Non**. Les tâches manquées pendant l'arrêt du serveur ne seront pas exécutées rétroactivement. Planifiez les maintenances en conséquence.

### Puis-je exécuter une tâche manuellement ?

Oui ! Dans la liste des tâches, cliquez sur l'icône **▶️ Exécuter** à côté de la tâche souhaitée.

### Comment annuler une tâche en cours d'exécution ?

Cette fonctionnalité est **à venir**. Actuellement, vous devez attendre la fin de l'exécution ou redémarrer le service backend.

---

## 📅 Planification

### Puis-je exécuter une tâche plusieurs fois par jour à des heures différentes ?

Non directement. **Solution** : Créez plusieurs tâches distinctes avec des horaires différents.

### Comment planifier une tâche le dernier jour du mois ?

Utilisez la planification **Mensuel** avec le jour **28** (fonctionne pour tous les mois). Pour les jours 29-31, créez 3 tâches séparées.

### Quelle est la fréquence minimale ?

**Toutes les minutes** (ne pas descendre en dessous pour éviter la surcharge).

### Comment désactiver temporairement une tâche sans la supprimer ?

Cliquez sur le **toggle** (interrupteur) à côté du nom de la tâche dans la liste.

---

## 📧 Notifications

### Pourquoi mes emails ne sont pas envoyés ?

Vérifiez dans l'ordre :

1. ✅ Le destinataire est **actif**
2. ✅ Les **paramètres SMTP** sont corrects (vérifiez avec l'admin)
3. ✅ Votre serveur mail n'a pas **bloqué** l'adresse
4. ✅ Consultez les **logs** pour voir l'erreur exacte

### Les messages WhatsApp coûtent-ils de l'argent ?

**Oui**, via Twilio. Consultez la [tarification Twilio](https://www.twilio.com/pricing) pour votre région.

### Puis-je envoyer des notifications à plusieurs personnes ?

**Oui** ! Ajoutez simplement plusieurs destinataires. Tous les destinataires **actifs** recevront les notifications.

### Comment tester les notifications sans créer une tâche ?

Utilisez l'option **"Tester"** dans la page Destinataires (icône 🧪 à côté du destinataire).

---

## 📁 Fichiers

### Quels types de fichiers sont supportés ?

Actuellement : **`.xlsx`** et **`.xls`** (Excel). Support PDF et images **à venir**.

### Quelle est la taille maximale de fichier ?

**50 MB** par défaut. Contactez l'administrateur pour augmenter cette limite.

### Les fichiers sont-ils supprimés automatiquement ?

Oui, les fichiers de plus de **30 jours** sont automatiquement supprimés pour libérer de l'espace.

### Où sont stockés mes fichiers ?

Sur le **serveur local** dans `/app/uploads`. Pour le cloud, contactez l'administrateur.

---

## 📊 Logs et Historique

### Combien de temps les logs sont conservés ?

**90 jours** par défaut. Les logs plus anciens sont archivés.

### Puis-je exporter les logs ?

**Oui** ! Utilisez le bouton **"Exporter"** en haut de la page Logs. Formats disponibles : Excel, CSV, PDF.

### Comment filtrer les logs par date ?

Utilisez les filtres en haut de la page Logs :
- Aujourd'hui
- Cette semaine
- Ce mois
- Personnalisé (choisissez vos dates)

---

## 🔒 Sécurité

### Mes données sont-elles sécurisées ?

**Oui** :
- ✅ Mots de passe **hashés** (BCrypt)
- ✅ Connexion **HTTPS** en production
- ✅ Authentification **JWT**
- ✅ Accès **restreint** par utilisateur

### Puis-je partager mon compte avec un collègue ?

**Non**, pour des raisons de **sécurité** et de **traçabilité**. Chaque utilisateur doit avoir son propre compte.

### Comment savoir si quelqu'un a accédé à mon compte ?

Consultez la date de **"Dernière connexion"** dans vos paramètres de profil.

---

## 🐛 Problèmes Techniques

### La page ne se charge pas

Essayez dans l'ordre :

1. Vérifiez votre **connexion internet**
2. **Effacez** le cache de votre navigateur (Ctrl+Shift+Delete)
3. Essayez avec un **autre navigateur** (Chrome, Firefox, Edge)
4. Contactez le **support** si le problème persiste

### Les graphiques ne s'affichent pas

Utilisez un **navigateur moderne** :
- ✅ Chrome/Edge (dernière version)
- ✅ Firefox (dernière version)
- ❌ Internet Explorer **NON SUPPORTÉ**

### J'ai une erreur 500

Il s'agit d'une **erreur serveur**. Contactez immédiatement l'administrateur système.

### J'ai une erreur 401/403

Votre **session a expiré** ou vous n'avez **pas les droits**. Reconnectez-vous.

---

## 📈 Performance

### L'application est lente

Vérifiez :

1. Votre **connexion internet** (débit)
2. Le nombre de **tâches actives** (<50 recommandé)
3. La **taille des fichiers** uploadés
4. Contactez l'admin pour vérifier les **ressources serveur**

### Combien d'utilisateurs peuvent se connecter simultanément ?

**Illimité** en théorie, mais dépend des ressources serveur. Performances testées jusqu'à **100 utilisateurs simultanés**.

---

## 💡 Astuces et Bonnes Pratiques

### Comment nommer mes tâches ?

**Format recommandé** :
```
[TYPE] - [FRÉQUENCE] - [DESCRIPTION]
Exemple : "Routes - Quotidien - Vérification Disponibilité"
```

### Comment organiser mes tâches ?

Utilisez des **préfixes** :
- `PROD-` : Tâches de production
- `TEST-` : Tâches de test
- `DEV-` : Tâches de développement

### Dois-je surveiller les logs ?

**Oui**, au moins **une fois par semaine**. Cela permet de détecter les problèmes avant qu'ils ne deviennent critiques.

---

## 🆘 Contact et Support

### Comment signaler un bug ?

1. **GitHub** : [Créer une issue](https://github.com/thoumi/Automation/issues)
2. **Email** : support@automation-system.com
3. Fournissez : captures d'écran, logs, étapes pour reproduire

### Comment demander une nouvelle fonctionnalité ?

Même procédure que pour un bug, mais choisissez le template **"Feature Request"** sur GitHub.

### Horaires du support

- **Email** : 24/7 (réponse sous 24-48h)
- **GitHub** : 24/7 (communauté)
- **Téléphone** : Lundi-Vendredi, 9h-18h

---

## 🔄 Mises à Jour

### À quelle fréquence l'application est mise à jour ?

**Une fois par mois** pour les mises à jour mineures, plus souvent pour les correctifs de sécurité.

### Comment savoir quelle version j'utilise ?

Consultez le pied de page de l'application ou la page **"À propos"** dans les paramètres.

### Les mises à jour cassent-elles la compatibilité ?

Non, nous suivons le **versioning sémantique** (SemVer) :
- **Majeur** (2.0.0) : Changements incompatibles
- **Mineur** (1.1.0) : Nouvelles fonctionnalités compatibles
- **Patch** (1.0.1) : Corrections de bugs

---

## ❓ Question Non Listée ?

Votre question n'est pas ici ? Pas de problème !

- 📖 Consultez la [**Documentation Complète**](../index.md)
- 🔧 Visitez [**Résolution de Problèmes**](troubleshooting.md)
- 💬 [**Contactez-nous**](contact.md)

---

**Cette FAQ est mise à jour régulièrement** avec les nouvelles questions ! 📝

