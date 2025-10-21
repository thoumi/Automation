import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../core/services/api.service';
import { FileUpload } from '../../core/models/file.model';

@Component({
  selector: 'app-files',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="space-y-6">
      <div class="flex justify-between items-center">
        <h1 class="text-3xl font-bold text-gray-900">Gestion des fichiers</h1>
        <label class="btn btn-primary cursor-pointer">
          <svg class="w-5 h-5 mr-2 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12"></path>
          </svg>
          Télécharger un fichier
          <input type="file" class="hidden" (change)="onFileSelected($event)" #fileInput>
        </label>
      </div>

      @if (uploading) {
        <div class="card">
          <div class="flex items-center justify-center space-x-3">
            <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-primary-600"></div>
            <span class="text-gray-700">Téléchargement en cours...</span>
          </div>
        </div>
      }

      <div class="card">
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead>
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Fichier</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Type</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Taille</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Téléchargé par</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Date</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Statut</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Actions</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              @for (file of files; track file.id) {
                <tr>
                  <td class="px-6 py-4 whitespace-nowrap">
                    <div class="flex items-center">
                      <svg class="w-8 h-8 text-gray-400 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z"></path>
                      </svg>
                      <div>
                        <div class="text-sm font-medium text-gray-900">{{ file.originalFileName }}</div>
                        <div class="text-xs text-gray-500">{{ file.fileName }}</div>
                      </div>
                    </div>
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ file.fileType }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ formatFileSize(file.fileSize) }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ file.uploadedBy }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {{ file.uploadedAt | date:'short' }}
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap">
                    @switch (file.processingStatus) {
                      @case (0) {
                        <span class="badge">En attente</span>
                      }
                      @case (1) {
                        <span class="badge badge-info">En cours</span>
                      }
                      @case (2) {
                        <span class="badge badge-success">Terminé</span>
                      }
                      @case (3) {
                        <span class="badge badge-error">Échec</span>
                      }
                    }
                  </td>
                  <td class="px-6 py-4 whitespace-nowrap text-sm font-medium space-x-2">
                    <button (click)="downloadFile(file.id)" class="text-primary-600 hover:text-primary-900">
                      Télécharger
                    </button>
                    <button (click)="deleteFile(file.id)" class="text-red-600 hover:text-red-900">
                      Supprimer
                    </button>
                  </td>
                </tr>
              } @empty {
                <tr>
                  <td colspan="7" class="px-6 py-8 text-center text-gray-500">
                    Aucun fichier téléchargé
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
export class FilesComponent implements OnInit {
  files: FileUpload[] = [];
  uploading = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadFiles();
  }

  loadFiles(): void {
    this.api.get<FileUpload[]>('files').subscribe({
      next: (files) => this.files = files,
      error: (err) => console.error('Error loading files', err)
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.uploading = true;
      this.api.upload<FileUpload>('files/upload', file).subscribe({
        next: () => {
          this.uploading = false;
          this.loadFiles();
          event.target.value = '';
        },
        error: (err) => {
          console.error('Error uploading file', err);
          this.uploading = false;
          alert('Erreur lors du téléchargement du fichier');
        }
      });
    }
  }

  downloadFile(id: number): void {
    window.open(`/api/files/${id}/download`, '_blank');
  }

  deleteFile(id: number): void {
    if (confirm('Êtes-vous sûr de vouloir supprimer ce fichier ?')) {
      this.api.delete(`files/${id}`).subscribe({
        next: () => this.loadFiles(),
        error: (err) => console.error('Error deleting file', err)
      });
    }
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i];
  }
}

