# 📅 Guide de Planification Simplifiée

## ✨ Nouveau Système de Planification Facile

Fini les expressions CRON complexes ! Le système utilise maintenant une interface intuitive pour planifier vos tâches.

---

## 🎯 Comment Créer une Tâche Planifiée

### Étape 1 : Accéder aux Tâches
1. Connectez-vous à l'application : **http://localhost:4300**
2. Cliquez sur **"Tâches"** dans le menu de gauche
3. Cliquez sur **"Nouvelle tâche"**

### Étape 2 : Remplir les Informations de Base
- **Nom** : Donnez un nom à votre tâche (ex: "Génération rapport quotidien")
- **Type** : Choisissez le type de tâche
  - **Routenverfügbarkeit** : Vérification des routes
  - **Staging Plan** : Plan de staging
  - **DNR Units** : Unités DNR
- **Description** : Décrivez ce que fait la tâche (optionnel)

### Étape 3 : Choisir la Fréquence (Section Planification)

#### 📌 Option 1 : Toutes les X minutes
- **Quand utiliser** : Pour des tâches qui doivent s'exécuter très fréquemment
- **Exemple** : Toutes les 5 minutes, Toutes les 15 minutes
- **Configuration** :
  - Fréquence : "Toutes les X minutes"
  - Intervalle : 5 (pour toutes les 5 minutes)

#### 🕐 Option 2 : Toutes les X heures
- **Quand utiliser** : Pour des tâches qui s'exécutent plusieurs fois par jour
- **Exemple** : Toutes les 2 heures, Toutes les 6 heures
- **Configuration** :
  - Fréquence : "Toutes les X heures"
  - Intervalle : 2 (pour toutes les 2 heures)

#### 📆 Option 3 : Quotidien
- **Quand utiliser** : Pour des tâches qui s'exécutent une fois par jour
- **Exemple** : Tous les jours à 9h00
- **Configuration** :
  - Fréquence : "Quotidien"
  - Heure : 09:00

#### 📅 Option 4 : Hebdomadaire
- **Quand utiliser** : Pour des tâches qui s'exécutent une fois par semaine
- **Exemple** : Chaque lundi à 8h30
- **Configuration** :
  - Fréquence : "Hebdomadaire"
  - Jour : Lundi
  - Heure : 08:30

#### 📊 Option 5 : Mensuel
- **Quand utiliser** : Pour des tâches qui s'exécutent une fois par mois
- **Exemple** : Le 1er de chaque mois à 7h00
- **Configuration** :
  - Fréquence : "Mensuel"
  - Jour du mois : 1
  - Heure : 07:00

---

## 💡 Exemples de Planifications Courantes

### Exemple 1 : Rapport Quotidien
```
Nom : Rapport quotidien
Type : Routenverfügbarkeit
Fréquence : Quotidien
Heure : 08:00
✓ Activée
```
**Résultat** : La tâche s'exécute tous les jours à 8h00

---

### Exemple 2 : Vérification Toutes les Heures
```
Nom : Vérification horaire
Type : DNR Units
Fréquence : Toutes les X heures
Intervalle : 1
✓ Activée
```
**Résultat** : La tâche s'exécute toutes les heures

---

### Exemple 3 : Rapport Hebdomadaire
```
Nom : Rapport hebdomadaire
Type : Staging Plan
Fréquence : Hebdomadaire
Jour : Lundi
Heure : 09:00
✓ Activée
```
**Résultat** : La tâche s'exécute chaque lundi à 9h00

---

### Exemple 4 : Rapport Mensuel
```
Nom : Rapport mensuel
Type : Routenverfügbarkeit
Fréquence : Mensuel
Jour du mois : 1
Heure : 06:00
✓ Activée
```
**Résultat** : La tâche s'exécute le 1er de chaque mois à 6h00

---

## 🔍 Résumé de Planification

Une fois que vous avez configuré votre planification, un **résumé en français** s'affiche automatiquement :

- "Tous les jours à 09:00"
- "Toutes les 5 minutes"
- "Chaque lundi à 08:30"
- "Le 1 de chaque mois à 07:00"

Cela vous permet de vérifier immédiatement que la planification est correcte !

---

## ✅ Activation / Désactivation

- **Cocher "Activée"** : La tâche s'exécutera automatiquement selon la planification
- **Décocher "Activée"** : La tâche est en pause et ne s'exécutera pas

Vous pouvez activer/désactiver une tâche à tout moment en cliquant sur le toggle dans la liste des tâches.

---

## 🚀 Exécution Manuelle

Même si une tâche est planifiée, vous pouvez l'exécuter immédiatement en cliquant sur le bouton **"▶ Exécuter"** dans la liste des tâches.

---

## 📊 Suivi d'Exécution

Dans la page **"Logs"**, vous pouvez voir :
- ✅ Les exécutions réussies (en vert)
- ❌ Les exécutions échouées (en rouge)
- ⏱️ La durée d'exécution
- 📝 Les messages détaillés

---

## 💬 Conseils

### ⏰ Choisir la Bonne Fréquence

- **Données en temps réel** → Toutes les 5-15 minutes
- **Rapports quotidiens** → Quotidien (le matin tôt)
- **Rapports hebdomadaires** → Lundi matin
- **Rapports mensuels** → 1er du mois

### 🔋 Optimiser les Performances

- Ne pas planifier trop de tâches en même temps
- Espacer les heures d'exécution si vous avez plusieurs tâches quotidiennes
- Privilégier les heures creuses (nuit, tôt le matin) pour les tâches lourdes

### 🛡️ Tester avant de Planifier

1. Créez la tâche avec la planification désirée
2. Cochez "Activée"
3. Cliquez sur "▶ Exécuter" pour tester immédiatement
4. Vérifiez les logs pour voir si tout fonctionne
5. La tâche continuera à s'exécuter automatiquement selon la planification

---

## 🆘 Besoin d'Aide ?

- Consultez le **Dashboard** pour voir les statistiques d'exécution
- Vérifiez les **Logs** pour comprendre les erreurs
- Les tâches désactivées n'apparaissent pas dans les statistiques

---

**C'est tout !** Fini les expressions CRON comme `25 8 * * *` 🎉

Le système s'occupe de tout automatiquement ! ✨

