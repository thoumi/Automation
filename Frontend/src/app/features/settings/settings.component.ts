import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="space-y-6">
      <h1 class="text-3xl font-bold text-gray-900">Paramètres</h1>

      <div class="card">
        <h2 class="text-xl font-semibold mb-4">Configuration du système</h2>
        <div class="space-y-4">
          <div>
            <h3 class="text-sm font-medium text-gray-700 mb-2">Dashboard Hangfire</h3>
            <p class="text-sm text-gray-500 mb-2">
              Accédez au tableau de bord Hangfire pour surveiller et gérer les tâches en arrière-plan.
            </p>
            <a href="/hangfire" target="_blank" class="btn btn-primary inline-block">
              Ouvrir Hangfire Dashboard
            </a>
          </div>

          <div class="border-t pt-4">
            <h3 class="text-sm font-medium text-gray-700 mb-2">Documentation API</h3>
            <p class="text-sm text-gray-500 mb-2">
              Consultez la documentation Swagger de l'API REST.
            </p>
            <a href="/swagger" target="_blank" class="btn btn-secondary inline-block">
              Ouvrir Swagger UI
            </a>
          </div>

          <div class="border-t pt-4">
            <h3 class="text-sm font-medium text-gray-700 mb-2">Version</h3>
            <p class="text-sm text-gray-500">
              Automation System v1.0.0
            </p>
          </div>

          <div class="border-t pt-4">
            <h3 class="text-sm font-medium text-gray-700 mb-2">À propos</h3>
            <p class="text-sm text-gray-500">
              Système d'automatisation pour la gestion des tâches planifiées, 
              le traitement des fichiers Excel, l'envoi de notifications et l'intégration avec CORTEX.
            </p>
          </div>
        </div>
      </div>

      <div class="card">
        <h2 class="text-xl font-semibold mb-4">Services configurés</h2>
        <div class="grid grid-cols-2 gap-4">
          <div class="p-4 bg-gray-50 rounded-lg">
            <h3 class="font-medium text-gray-900 mb-1">Email (IMAP/SMTP)</h3>
            <p class="text-xs text-gray-500">Lecture et envoi d'emails</p>
          </div>
          <div class="p-4 bg-gray-50 rounded-lg">
            <h3 class="font-medium text-gray-900 mb-1">WhatsApp (Twilio)</h3>
            <p class="text-xs text-gray-500">Envoi de messages WhatsApp</p>
          </div>
          <div class="p-4 bg-gray-50 rounded-lg">
            <h3 class="font-medium text-gray-900 mb-1">Amazon Chime</h3>
            <p class="text-xs text-gray-500">Notifications d'équipe</p>
          </div>
          <div class="p-4 bg-gray-50 rounded-lg">
            <h3 class="font-medium text-gray-900 mb-1">CORTEX API</h3>
            <p class="text-xs text-gray-500">Intégration Amazon</p>
          </div>
          <div class="p-4 bg-gray-50 rounded-lg">
            <h3 class="font-medium text-gray-900 mb-1">Excel Processing</h3>
            <p class="text-xs text-gray-500">Lecture et traitement Excel</p>
          </div>
          <div class="p-4 bg-gray-50 rounded-lg">
            <h3 class="font-medium text-gray-900 mb-1">Hangfire</h3>
            <p class="text-xs text-gray-500">Planification des tâches</p>
          </div>
        </div>
      </div>
    </div>
  `
})
export class SettingsComponent {}

