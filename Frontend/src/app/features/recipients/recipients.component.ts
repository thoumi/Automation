import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { NotificationRecipient, NotificationType } from '../../core/models/notification.model';

@Component({
  selector: 'app-recipients',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="space-y-6">
      <div class="flex justify-between items-center">
        <h1 class="text-3xl font-bold text-gray-900">Destinataires</h1>
        <button class="btn btn-primary" (click)="showAddForm = true">
          <svg class="w-5 h-5 mr-2 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path>
          </svg>
          Nouveau destinataire
        </button>
      </div>

      <!-- Add Form -->
      @if (showAddForm) {
        <div class="card">
          <h2 class="text-xl font-semibold mb-4">Nouveau destinataire</h2>
          <form (ngSubmit)="createRecipient()" class="space-y-4">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Nom</label>
                <input type="text" [(ngModel)]="newRecipient.name" name="name" required class="input">
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Type</label>
                <select [(ngModel)]="newRecipient.type" name="type" required class="input">
                  <option [ngValue]="0">Email</option>
                  <option [ngValue]="1">WhatsApp</option>
                  <option [ngValue]="2">Chime</option>
                </select>
              </div>
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">
                {{ getIdentifierLabel(newRecipient.type ?? 0) }}
              </label>
              <input type="text" [(ngModel)]="newRecipient.identifier" name="identifier" 
                     [placeholder]="getIdentifierPlaceholder(newRecipient.type ?? 0)" required class="input">
            </div>
            <div class="flex items-center">
              <input type="checkbox" [(ngModel)]="newRecipient.isActive" name="isActive" 
                     id="isActive" class="rounded text-primary-600">
              <label for="isActive" class="ml-2 text-sm text-gray-700">Actif</label>
            </div>
            <div class="flex justify-end space-x-3">
              <button type="button" (click)="cancelAdd()" class="btn btn-secondary">Annuler</button>
              <button type="submit" class="btn btn-primary">Créer</button>
            </div>
          </form>
        </div>
      }

      <!-- Tabs -->
      <div class="border-b border-gray-200">
        <nav class="-mb-px flex space-x-8">
          <button 
            (click)="selectedType = undefined; loadRecipients()"
            [class.border-primary-500]="selectedType === undefined"
            [class.text-primary-600]="selectedType === undefined"
            class="whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm">
            Tous
          </button>
          <button 
            (click)="selectedType = 0; loadRecipients()"
            [class.border-primary-500]="selectedType === 0"
            [class.text-primary-600]="selectedType === 0"
            class="whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm">
            Email
          </button>
          <button 
            (click)="selectedType = 1; loadRecipients()"
            [class.border-primary-500]="selectedType === 1"
            [class.text-primary-600]="selectedType === 1"
            class="whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm">
            WhatsApp
          </button>
          <button 
            (click)="selectedType = 2; loadRecipients()"
            [class.border-primary-500]="selectedType === 2"
            [class.text-primary-600]="selectedType === 2"
            class="whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm">
            Chime
          </button>
        </nav>
      </div>

      <!-- Recipients List -->
      <div class="card">
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead>
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Nom</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Type</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Identifiant</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Statut</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Actions</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              @for (recipient of recipients; track recipient.id) {
                <tr>
                  <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {{ recipient.name }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ getTypeLabel(recipient.type) }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ recipient.identifier }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap">
                    @if (recipient.isActive) {
                      <span class="badge badge-success">Actif</span>
                    } @else {
                      <span class="badge">Inactif</span>
                    }
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm font-medium space-x-2">
                    <button (click)="toggleRecipient(recipient)" class="text-primary-600 hover:text-primary-900">
                      {{ recipient.isActive ? 'Désactiver' : 'Activer' }}
                    </button>
                    <button (click)="deleteRecipient(recipient.id)" class="text-red-600 hover:text-red-900">
                      Supprimer
                    </button>
                  </td>
                </tr>
              } @empty {
                <tr>
                  <td colspan="5" class="px-6 py-8 text-center text-gray-500">
                    Aucun destinataire configuré
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `
})
export class RecipientsComponent implements OnInit {
  recipients: NotificationRecipient[] = [];
  showAddForm = false;
  selectedType: NotificationType | undefined = undefined;
  newRecipient: Partial<NotificationRecipient> = {
    name: '',
    type: 0,
    identifier: '',
    isActive: true
  };

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadRecipients();
  }

  loadRecipients(): void {
    const params = this.selectedType !== undefined ? { type: this.selectedType } : {};
    this.api.get<NotificationRecipient[]>('recipients', params).subscribe({
      next: (recipients) => this.recipients = recipients,
      error: (err) => console.error('Error loading recipients', err)
    });
  }

  createRecipient(): void {
    this.api.post<NotificationRecipient>('recipients', this.newRecipient).subscribe({
      next: () => {
        this.loadRecipients();
        this.cancelAdd();
      },
      error: (err) => console.error('Error creating recipient', err)
    });
  }

  toggleRecipient(recipient: NotificationRecipient): void {
    this.api.patch(`recipients/${recipient.id}/toggle`).subscribe({
      next: () => this.loadRecipients(),
      error: (err) => console.error('Error toggling recipient', err)
    });
  }

  deleteRecipient(id: number): void {
    if (confirm('Êtes-vous sûr de vouloir supprimer ce destinataire ?')) {
      this.api.delete(`recipients/${id}`).subscribe({
        next: () => this.loadRecipients(),
        error: (err) => console.error('Error deleting recipient', err)
      });
    }
  }

  cancelAdd(): void {
    this.showAddForm = false;
    this.newRecipient = {
      name: '',
      type: 0,
      identifier: '',
      isActive: true
    };
  }

  getTypeLabel(type: NotificationType): string {
    const labels: Record<number, string> = {
      0: 'Email',
      1: 'WhatsApp',
      2: 'Chime'
    };
    return labels[type] || 'Inconnu';
  }

  getIdentifierLabel(type: NotificationType): string {
    const labels: Record<number, string> = {
      0: 'Adresse email',
      1: 'Numéro de téléphone',
      2: 'Webhook URL'
    };
    return labels[type] || 'Identifiant';
  }

  getIdentifierPlaceholder(type: NotificationType): string {
    const placeholders: Record<number, string> = {
      0: 'exemple@domaine.com',
      1: '+33612345678',
      2: 'https://hooks.chime.aws/...'
    };
    return placeholders[type] || '';
  }
}

