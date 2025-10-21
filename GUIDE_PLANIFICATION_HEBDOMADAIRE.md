# 📅 Guide : Planification Hebdomadaire Détaillée

## 🎯 Nouvelle Fonctionnalité

Le système d'automatisation supporte maintenant une **planification hebdomadaire détaillée** qui permet de :

- ✅ **Choisir un jour spécifique** de la semaine (lundi à dimanche)
- ✅ **Définir une heure précise** d'exécution
- ✅ **Interface intuitive** avec boutons de sélection
- ✅ **Heures rapides** pour les créneaux courants
- ✅ **Aperçu en temps réel** de la planification

---

## 🚀 Comment Utiliser

### 1. Créer une Nouvelle Tâche

1. **Allez dans** la section "Tâches"
2. **Cliquez** sur "Ajouter une tâche"
3. **Remplissez** les informations de base (nom, description, type)

### 2. Configurer la Planification Hebdomadaire

1. **Sélectionnez** "Hebdomadaire" dans la fréquence
2. **Choisissez le jour** en cliquant sur le bouton correspondant :
   - 📅 **Lundi** à **Dimanche**
   - Interface visuelle avec boutons colorés
3. **Définissez l'heure** :
   - 🕐 **Saisie manuelle** : Tapez l'heure (ex: 14:30)
   - ⚡ **Heures rapides** : Cliquez sur 9h, 12h, ou 18h

### 3. Vérifier la Planification

L'aperçu vous montre immédiatement :
- 📋 **Résumé** : "Chaque [jour] à [heure]"
- 💡 **Information** : "La tâche s'exécutera automatiquement chaque semaine"

---

## 🎨 Interface Améliorée

### Sélection des Jours
```
📅 Quel jour de la semaine ?
┌─────────┬─────────┐
│  Lundi  │ Mardi   │
├─────────┼─────────┤
│Mercredi │ Jeudi   │
├─────────┼─────────┤
│ Vendredi│ Samedi  │
├─────────┼─────────┤
│Dimanche │         │
└─────────┴─────────┘
```

### Sélection de l'Heure
```
🕐 À quelle heure ?
[14:30] [9h] [12h] [18h]
```

---

## 📊 Exemples d'Utilisation

### Exemple 1 : Rapport Hebdomadaire
- **Jour** : Vendredi
- **Heure** : 17:00
- **Description** : "Chaque vendredi à 17:00"
- **Usage** : Rapport de fin de semaine

### Exemple 2 : Nettoyage de Base
- **Jour** : Dimanche
- **Heure** : 02:00
- **Description** : "Chaque dimanche à 02:00"
- **Usage** : Maintenance pendant les heures creuses

### Exemple 3 : Backup Quotidien
- **Jour** : Lundi, Mercredi, Vendredi
- **Heure** : 20:00
- **Description** : "Chaque [jour] à 20:00"
- **Usage** : Sauvegarde régulière

---

## 🔧 Configuration Technique

### Expression CRON Générée
- **Lundi 9h** : `0 9 * * 1`
- **Vendredi 18h** : `0 18 * * 5`
- **Dimanche 2h** : `0 2 * * 0`

### Validation Automatique
- ✅ **Jour valide** : 0-6 (dimanche-samedi)
- ✅ **Heure valide** : 00:00-23:59
- ✅ **Format correct** : HH:mm

---

## 💡 Conseils d'Utilisation

### 🕐 Heures Recommandées
- **9h** : Début de journée, rapports matinaux
- **12h** : Pause déjeuner, synchronisation
- **18h** : Fin de journée, rapports quotidiens
- **2h** : Maintenance, nettoyage, backup

### 📅 Jours Recommandés
- **Lundi** : Rapports de début de semaine
- **Mercredi** : Point milieu de semaine
- **Vendredi** : Rapports de fin de semaine
- **Dimanche** : Maintenance, backup

### ⚠️ Bonnes Pratiques
- **Évitez** les heures de pointe (8h-9h, 17h-18h)
- **Préférez** les heures creuses pour les tâches lourdes
- **Testez** d'abord avec des tâches simples
- **Surveillez** les logs d'exécution

---

## 🚨 Dépannage

### Problème : La tâche ne s'exécute pas
1. **Vérifiez** que la tâche est activée
2. **Contrôlez** la planification dans l'aperçu
3. **Consultez** les logs d'exécution
4. **Vérifiez** que Hangfire est en cours d'exécution

### Problème : Mauvaise heure d'exécution
1. **Vérifiez** le fuseau horaire du serveur
2. **Contrôlez** l'heure système
3. **Testez** avec une heure différente

### Problème : Jour incorrect
1. **Vérifiez** la sélection du jour
2. **Contrôlez** l'aperçu de planification
3. **Testez** avec un autre jour

---

## 🎉 Résultat

Avec cette nouvelle fonctionnalité, vous pouvez maintenant :

- 🎯 **Planifier précisément** vos tâches hebdomadaires
- 🕐 **Choisir l'heure exacte** d'exécution
- 📅 **Sélectionner visuellement** le jour de la semaine
- ⚡ **Utiliser des raccourcis** pour les heures courantes
- 📋 **Voir immédiatement** le résumé de votre planification

**Votre système d'automatisation est maintenant encore plus flexible et puissant ! 🚀**
