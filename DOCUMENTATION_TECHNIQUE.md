# 📘 Documentation Technique - Système d'Automatisation

## Table des matières

1. [Architecture Détaillée](#1-architecture-détaillée)
2. [Structure du Projet](#2-structure-du-projet)
3. [Modèles de Données](#3-modèles-de-données)
4. [API Reference](#4-api-reference)
5. [Services Backend](#5-services-backend)
6. [Jobs et Planification](#6-jobs-et-planification)
7. [Frontend Architecture](#7-frontend-architecture)
8. [Sécurité](#8-sécurité)
9. [Base de Données](#9-base-de-données)
10. [Déploiement](#10-déploiement)
11. [Maintenance](#11-maintenance)
12. [Performance](#12-performance)

---

## 1. Architecture Détaillée

### 1.1 Vue d'ensemble

Le système suit une architecture en couches (Layered Architecture) avec séparation claire des responsabilités :

```
┌──────────────────────────────────────────────────────────┐
│                    Presentation Layer                     │
│                  (Angular Frontend)                       │
└───────────────────────┬──────────────────────────────────┘
                        │ HTTP/JSON
┌───────────────────────▼──────────────────────────────────┐
│                    Application Layer                      │
│                (ASP.NET Core API)                         │
│  ┌────────────────────────────────────────────────────┐  │
│  │          Controllers (REST Endpoints)              │  │
│  └────────────────────────────────────────────────────┘  │
└───────────────────────┬──────────────────────────────────┘
                        │
┌───────────────────────▼──────────────────────────────────┐
│                    Business Layer                         │
│              (Core Services & Logic)                      │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │Excel Service │  │Email Service │  │Cortex Service│  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │WhatsApp Svc  │  │Chime Service │  │Schedule Svc  │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
└───────────────────────┬──────────────────────────────────┘
                        │
┌───────────────────────▼──────────────────────────────────┐
│                  Infrastructure Layer                     │
│        (Data Access, External Services)                   │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │EF Core DbCtx │  │  Repositories │  │  Hangfire    │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
└───────────────────────┬──────────────────────────────────┘
                        │
┌───────────────────────▼──────────────────────────────────┐
│                    Data Layer                             │
│                (SQL Server Database)                      │
└──────────────────────────────────────────────────────────┘
```

### 1.2 Patterns de Conception Utilisés

#### Dependency Injection
- Injection de toutes les dépendances via le conteneur IoC d'ASP.NET Core
- Configuration dans `Program.cs`

```csharp
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICortexService, CortexService>();
```

#### Repository Pattern
- Abstraction de l'accès aux données
- `ApplicationDbContext` comme repository principal

#### Service Layer Pattern
- Logique métier encapsulée dans des services
- Séparation entre API Controllers et Business Logic

#### Background Job Pattern
- Utilisation de Hangfire pour les tâches asynchrones
- Jobs récurrents basés sur CRON

---

## 2. Structure du Projet

### 2.1 Backend Structure

```
Backend/
├── AutomationSystem.API/              # API Layer
│   ├── Controllers/                   # REST Controllers
│   │   ├── AuthController.cs
│   │   ├── TasksController.cs
│   │   ├── LogsController.cs
│   │   ├── FilesController.cs
│   │   └── RecipientsController.cs
│   ├── Program.cs                     # Application Entry Point
│   ├── appsettings.json               # Configuration
│   └── AutomationSystem.API.csproj
│
├── AutomationSystem.Core/             # Business Layer
│   ├── Models/                        # Domain Models
│   │   ├── User.cs
│   │   ├── ScheduledTask.cs
│   │   ├── TaskExecutionLog.cs
│   │   ├── NotificationRecipient.cs
│   │   ├── FileUpload.cs
│   │   └── TaskSchedule.cs
│   ├── Interfaces/                    # Service Contracts
│   │   ├── IExcelService.cs
│   │   ├── IEmailService.cs
│   │   ├── ICortexService.cs
│   │   ├── IWhatsAppService.cs
│   │   └── IChimeService.cs
│   ├── Services/                      # Business Logic
│   │   ├── ExcelService.cs
│   │   ├── EmailService.cs
│   │   ├── CortexService.cs
│   │   ├── WhatsAppService.cs
│   │   ├── ChimeService.cs
│   │   └── ScheduleService.cs
│   ├── Jobs/                          # Hangfire Jobs
│   │   ├── RoutenverfuegbarkeitJob.cs
│   │   ├── StagingPlanJob.cs
│   │   └── DNRUnitsJob.cs
│   └── AutomationSystem.Core.csproj
│
└── AutomationSystem.Infrastructure/   # Data Layer
    ├── Data/
    │   ├── ApplicationDbContext.cs    # EF Core Context
    │   └── Migrations/                # Database Migrations
    └── AutomationSystem.Infrastructure.csproj
```

### 2.2 Frontend Structure

```
Frontend/
├── src/
│   ├── app/
│   │   ├── core/                      # Core Module
│   │   │   ├── models/                # TypeScript Models
│   │   │   │   ├── task.model.ts
│   │   │   │   ├── schedule.model.ts
│   │   │   │   ├── notification.model.ts
│   │   │   │   └── file.model.ts
│   │   │   ├── services/              # Angular Services
│   │   │   │   ├── api.service.ts
│   │   │   │   ├── auth.service.ts
│   │   │   │   ├── task.service.ts
│   │   │   │   └── log.service.ts
│   │   │   ├── interceptors/          # HTTP Interceptors
│   │   │   │   └── auth.interceptor.ts
│   │   │   └── guards/                # Route Guards
│   │   │       └── auth.guard.ts
│   │   ├── features/                  # Feature Modules
│   │   │   ├── auth/
│   │   │   │   └── login/
│   │   │   ├── dashboard/
│   │   │   ├── tasks/
│   │   │   ├── logs/
│   │   │   ├── files/
│   │   │   ├── recipients/
│   │   │   └── settings/
│   │   ├── shared/                    # Shared Components
│   │   │   └── components/
│   │   │       ├── layout/
│   │   │       └── schedule-picker/
│   │   ├── app.component.ts           # Root Component
│   │   └── app.routes.ts              # Routing Config
│   ├── environments/                  # Environment Configs
│   │   ├── environment.ts
│   │   └── environment.prod.ts
│   ├── styles.scss                    # Global Styles
│   └── index.html                     # HTML Entry
├── tailwind.config.js                 # Tailwind Config
├── angular.json                       # Angular CLI Config
├── package.json                       # NPM Dependencies
└── tsconfig.json                      # TypeScript Config
```

---

## 3. Modèles de Données

### 3.1 User (Utilisateur)

```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }          // Unique, Required
    public string PasswordHash { get; set; }   // BCrypt Hash
    public string FullName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
```

### 3.2 ScheduledTask (Tâche Planifiée)

```csharp
public class ScheduledTask
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CronExpression { get; set; }     // CRON pour Hangfire
    public string? ScheduleJson { get; set; }      // Configuration simplifiée
    public string TaskType { get; set; }           // Type de job
    public bool IsEnabled { get; set; }
    public string? Configuration { get; set; }     // JSON config
    public DateTime CreatedAt { get; set; }
    public DateTime? LastExecutionAt { get; set; }
    public DateTime? NextExecutionAt { get; set; }
}
```

**Types de tâches** :
- `Routenverfuegbarkeit` : Vérification disponibilité routes
- `StagingPlan` : Génération de plans de staging
- `DNRUnits` : Extraction unités DNR

### 3.3 TaskExecutionLog (Log d'Exécution)

```csharp
public class TaskExecutionLog
{
    public int Id { get; set; }
    public int ScheduledTaskId { get; set; }
    public TaskStatus Status { get; set; }         // Pending, Running, Success, Failed
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public string? OutputData { get; set; }        // Résultat JSON
    
    // Navigation
    public ScheduledTask ScheduledTask { get; set; }
}
```

### 3.4 NotificationRecipient (Destinataire)

```csharp
public class NotificationRecipient
{
    public int Id { get; set; }
    public NotificationType Type { get; set; }     // Email, WhatsApp, Chime
    public string Identifier { get; set; }         // email, phone, webhook URL
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### 3.5 FileUpload (Fichier Uploadé)

```csharp
public class FileUpload
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string OriginalFileName { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }           // Excel, PDF, Image
    public long FileSize { get; set; }
    public string ProcessingStatus { get; set; }   // Pending, Processed, Failed
    public string? ProcessingMessage { get; set; }
    public DateTime UploadedAt { get; set; }
}
```

### 3.6 TaskSchedule (Planification Simplifiée)

```csharp
public enum ScheduleFrequency
{
    Minute,   // Toutes les X minutes
    Hour,     // Toutes les X heures
    Daily,    // Quotidien à une heure fixe
    Weekly,   // Hebdomadaire (jour + heure)
    Monthly   // Mensuel (jour du mois + heure)
}

public class TaskSchedule
{
    public ScheduleFrequency Frequency { get; set; }
    public int Interval { get; set; }              // X minutes/heures ou jour
    public string TimeOfDay { get; set; }          // HH:mm
    public DayOfWeek DayOfWeek { get; set; }       // Pour Weekly
    public int DayOfMonth { get; set; }            // 1-31 pour Monthly
}
```

---

## 4. API Reference

### 4.1 Authentification

#### POST /api/auth/login
Authentifie un utilisateur et retourne un JWT token.

**Request Body** :
```json
{
  "email": "admin@example.com",
  "password": "admin123"
}
```

**Response 200** :
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response 401** :
```json
{
  "message": "Invalid credentials"
}
```

### 4.2 Tasks (Tâches)

#### GET /api/tasks
Récupère toutes les tâches planifiées.

**Headers** :
```
Authorization: Bearer {token}
```

**Response 200** :
```json
[
  {
    "id": 1,
    "name": "Vérification Routes",
    "description": "Vérification quotidienne des routes",
    "cronExpression": "0 9 * * *",
    "taskType": "Routenverfuegbarkeit",
    "isEnabled": true,
    "createdAt": "2025-10-20T10:00:00Z",
    "lastExecutionAt": "2025-10-21T09:00:00Z",
    "nextExecutionAt": "2025-10-22T09:00:00Z"
  }
]
```

#### POST /api/tasks
Crée une nouvelle tâche.

**Request Body** :
```json
{
  "name": "Nouvelle Tâche",
  "description": "Description de la tâche",
  "taskType": "StagingPlan",
  "isEnabled": true,
  "scheduleJson": "{\"frequency\":\"Daily\",\"interval\":1,\"timeOfDay\":\"09:00\"}"
}
```

**Response 201** :
```json
{
  "id": 2,
  "name": "Nouvelle Tâche",
  "cronExpression": "0 9 * * *",
  ...
}
```

#### PUT /api/tasks/{id}
Met à jour une tâche existante.

#### DELETE /api/tasks/{id}
Supprime une tâche.

#### POST /api/tasks/{id}/execute
Exécute manuellement une tâche.

### 4.3 Logs (Historique)

#### GET /api/logs
Récupère l'historique des exécutions.

**Query Parameters** :
- `taskId` (optional) : Filtrer par tâche
- `status` (optional) : Filtrer par statut (Success, Failed)
- `from` (optional) : Date de début
- `to` (optional) : Date de fin

**Response 200** :
```json
[
  {
    "id": 1,
    "scheduledTaskId": 1,
    "scheduledTaskName": "Vérification Routes",
    "status": "Success",
    "startedAt": "2025-10-21T09:00:00Z",
    "completedAt": "2025-10-21T09:05:32Z",
    "errorMessage": null,
    "outputData": "{\"routesChecked\":150,\"available\":145}"
  }
]
```

#### GET /api/logs/{id}
Récupère les détails d'un log spécifique.

### 4.4 Recipients (Destinataires)

#### GET /api/recipients
Liste tous les destinataires de notifications.

#### POST /api/recipients
Crée un nouveau destinataire.

**Request Body** :
```json
{
  "type": 0,              // 0=Email, 1=WhatsApp, 2=Chime
  "identifier": "user@example.com",
  "isActive": true
}
```

#### DELETE /api/recipients/{id}
Supprime un destinataire.

### 4.5 Files (Fichiers)

#### GET /api/files
Liste tous les fichiers uploadés.

#### POST /api/files/upload
Upload un nouveau fichier.

**Request** : `multipart/form-data`

```
file: [binary file data]
```

**Response 200** :
```json
{
  "id": 1,
  "fileName": "report_2025-10-21.xlsx",
  "fileSize": 45678,
  "uploadedAt": "2025-10-21T10:30:00Z"
}
```

#### DELETE /api/files/{id}
Supprime un fichier.

---

## 5. Services Backend

### 5.1 ExcelService

Service pour le traitement de fichiers Excel avec ClosedXML.

```csharp
public interface IExcelService
{
    Task<List<Dictionary<string, object>>> ReadExcelAsync(string filePath);
    Task<string> WriteExcelAsync(List<Dictionary<string, object>> data, string fileName);
    Task<byte[]> GenerateImageFromExcelAsync(string filePath);
}
```

**Fonctionnalités** :
- Lecture de fichiers `.xlsx` / `.xls`
- Extraction de données en dictionnaires
- Génération de fichiers Excel
- Conversion Excel → Image (pour notifications)

### 5.2 EmailService

Service d'envoi d'emails via MailKit.

```csharp
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, List<string>? attachments = null);
    Task<List<EmailMessage>> ReadInboxAsync(int maxCount = 10);
}
```

**Configuration** (appsettings.json) :
```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "votre@email.com",
    "Password": "votremotdepasse",
    "FromEmail": "noreply@votreentreprise.com",
    "FromName": "Système d'Automatisation"
  }
}
```

### 5.3 WhatsAppService

Service d'envoi de messages WhatsApp via Twilio.

```csharp
public interface IWhatsAppService
{
    Task SendMessageAsync(string toNumber, string message);
    Task SendMediaAsync(string toNumber, string message, string mediaUrl);
}
```

**Configuration** :
```json
{
  "TwilioSettings": {
    "AccountSid": "ACxxxxxxxxxxxx",
    "AuthToken": "votre_token",
    "WhatsAppFrom": "whatsapp:+14155238886"
  }
}
```

### 5.4 ChimeService

Service d'envoi de messages Amazon Chime via Webhooks.

```csharp
public interface IChimeService
{
    Task SendMessageAsync(string message);
    Task SendAlertAsync(string title, string message, string severity);
}
```

### 5.5 CortexService

Service d'intégration avec l'API CORTEX.

```csharp
public interface ICortexService
{
    Task<CortexResponse> GetDataAsync(string endpoint);
    Task<CortexResponse> PostDataAsync(string endpoint, object data);
}
```

### 5.6 ScheduleService

Service de conversion entre planification simplifiée et CRON.

```csharp
public static class ScheduleService
{
    public static string ToCronExpression(TaskSchedule schedule)
    {
        return schedule.Frequency switch
        {
            ScheduleFrequency.Minute => $"*/{schedule.Interval} * * * *",
            ScheduleFrequency.Hour => $"0 */{schedule.Interval} * * *",
            ScheduleFrequency.Daily => $"{minute} {hour} * * *",
            ScheduleFrequency.Weekly => $"{minute} {hour} * * {dayOfWeek}",
            ScheduleFrequency.Monthly => $"{minute} {hour} {dayOfMonth} * *",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
```

**Exemples de conversion** :
- `Daily, 09:00` → `0 9 * * *`
- `Weekly, Monday 14:30` → `30 14 * * 1`
- `Monthly, Day 15, 08:00` → `0 8 15 * *`
- `Every 15 minutes` → `*/15 * * * *`

---

## 6. Jobs et Planification

### 6.1 Hangfire Configuration

```csharp
// Program.cs
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(connectionString);
});

builder.Services.AddHangfireServer();

app.UseHangfireDashboard("/hangfire");
```

### 6.2 Types de Jobs

#### RoutenverfuegbarkeitJob

Vérifie la disponibilité des routes.

```csharp
public class RoutenverfuegbarkeitJob
{
    public async Task ExecuteAsync(int taskId)
    {
        // 1. Appeler API CORTEX
        var routes = await _cortexService.GetRoutesAsync();
        
        // 2. Vérifier disponibilité
        var availableRoutes = routes.Where(r => r.IsAvailable).ToList();
        
        // 3. Générer rapport Excel
        var reportPath = await _excelService.WriteExcelAsync(availableRoutes, "routes.xlsx");
        
        // 4. Envoyer notifications
        await _emailService.SendEmailAsync("rapport@entreprise.com", 
            "Rapport Routes", "Voir fichier joint", new[] { reportPath });
    }
}
```

#### StagingPlanJob

Génère les plans de staging quotidiens.

```csharp
public class StagingPlanJob
{
    public async Task ExecuteAsync(int taskId)
    {
        // 1. Récupérer données depuis CORTEX
        // 2. Générer plan Excel
        // 3. Convertir en image
        // 4. Envoyer via WhatsApp + Chime
    }
}
```

#### DNRUnitsJob

Extrait et traite les unités DNR.

### 6.3 Enregistrement des Jobs

```csharp
// Lors de la création/modification d'une tâche
var cronExpression = ScheduleService.ToCronExpression(schedule);

RecurringJob.AddOrUpdate(
    $"task-{task.Id}",
    () => ExecuteTaskJob(task.Id, task.TaskType),
    cronExpression
);
```

---

## 7. Frontend Architecture

### 7.1 Modules et Composants

#### Core Module
- Services globaux (auth, API)
- Models TypeScript
- Interceptors HTTP
- Guards de routing

#### Features Modules
Modules lazy-loaded par fonctionnalité :
- `AuthModule` : Login
- `DashboardModule` : Tableau de bord
- `TasksModule` : Gestion tâches
- `LogsModule` : Historique
- `RecipientsModule` : Destinataires

#### Shared Module
Composants réutilisables :
- `LayoutComponent` : Navigation
- `SchedulePickerComponent` : Sélecteur planification

### 7.2 State Management

Utilise des **Services** avec **BehaviorSubject** :

```typescript
@Injectable({ providedIn: 'root' })
export class TaskService {
  private tasksSubject = new BehaviorSubject<Task[]>([]);
  public tasks$ = this.tasksSubject.asObservable();

  loadTasks(): void {
    this.api.get<Task[]>('/tasks').subscribe(tasks => {
      this.tasksSubject.next(tasks);
    });
  }
}
```

### 7.3 Routing

```typescript
export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'tasks', component: TasksComponent },
      { path: 'logs', component: LogsComponent },
      { path: 'recipients', component: RecipientsComponent },
      { path: '', redirectTo: '/dashboard', pathMatch: 'full' }
    ]
  }
];
```

### 7.4 HTTP Interceptors

#### AuthInterceptor

Ajoute automatiquement le token JWT aux requêtes :

```typescript
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');
  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }
  return next(req);
};
```

---

## 8. Sécurité

### 8.1 Authentification JWT

#### Génération du Token

```csharp
private string GenerateJwtToken(User user)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
    };

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(60),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

#### Validation du Token

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
```

### 8.2 Hashage des Mots de Passe

Utilise **BCrypt.Net** :

```csharp
// Hash lors de la création
var passwordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);

// Vérification lors du login
var isValid = BCrypt.Net.BCrypt.Verify(plainPassword, storedHash);
```

### 8.3 CORS Configuration

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4300")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

### 8.4 Sécurité des Données

- Toutes les mots de passe sont hashés (BCrypt)
- Tokens JWT avec expiration (60 minutes par défaut)
- HTTPS en production
- Validation des inputs côté serveur
- Parameterized queries (EF Core) contre SQL Injection

---

## 9. Base de Données

### 9.1 Schema SQL

```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FullName NVARCHAR(256),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastLoginAt DATETIME2
);

CREATE TABLE ScheduledTasks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(256) NOT NULL,
    Description NVARCHAR(MAX),
    CronExpression NVARCHAR(100) NOT NULL,
    ScheduleJson NVARCHAR(MAX),
    TaskType NVARCHAR(50) NOT NULL,
    IsEnabled BIT NOT NULL DEFAULT 1,
    Configuration NVARCHAR(MAX),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastExecutionAt DATETIME2,
    NextExecutionAt DATETIME2
);

CREATE TABLE TaskExecutionLogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ScheduledTaskId INT NOT NULL FOREIGN KEY REFERENCES ScheduledTasks(Id),
    Status INT NOT NULL,  -- 0=Pending, 1=Running, 2=Success, 3=Failed
    StartedAt DATETIME2 NOT NULL,
    CompletedAt DATETIME2,
    ErrorMessage NVARCHAR(MAX),
    OutputData NVARCHAR(MAX)
);

CREATE TABLE NotificationRecipients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Type INT NOT NULL,  -- 0=Email, 1=WhatsApp, 2=Chime
    Identifier NVARCHAR(256) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE FileUploads (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FileName NVARCHAR(256) NOT NULL,
    OriginalFileName NVARCHAR(256) NOT NULL,
    FilePath NVARCHAR(512) NOT NULL,
    FileType NVARCHAR(50) NOT NULL,
    FileSize BIGINT NOT NULL,
    ProcessingStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    ProcessingMessage NVARCHAR(MAX),
    UploadedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
```

### 9.2 Indexes Recommandés

```sql
CREATE INDEX IX_TaskExecutionLogs_ScheduledTaskId ON TaskExecutionLogs(ScheduledTaskId);
CREATE INDEX IX_TaskExecutionLogs_StartedAt ON TaskExecutionLogs(StartedAt DESC);
CREATE INDEX IX_ScheduledTasks_IsEnabled ON ScheduledTasks(IsEnabled) WHERE IsEnabled = 1;
CREATE INDEX IX_Users_Email ON Users(Email);
```

### 9.3 Entity Framework Core

**DbContext** :

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ScheduledTask> ScheduledTasks { get; set; }
    public DbSet<TaskExecutionLog> TaskExecutionLogs { get; set; }
    public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
    public DbSet<FileUpload> FileUploads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<TaskExecutionLog>(entity =>
        {
            entity.HasOne(e => e.ScheduledTask)
                  .WithMany()
                  .HasForeignKey(e => e.ScheduledTaskId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
```

---

## 10. Déploiement

### 10.1 Docker Compose

```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Test@Password123
    ports:
      - "1444:1433"
    volumes:
      - sqldata:/var/opt/mssql
      - ./init-database.sql:/docker-entrypoint-initdb.d/init-database.sql

  backend:
    build:
      context: ./Backend
      dockerfile: Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:80
      - ConnectionStrings__SqlServer=Server=sqlserver;Database=AutomationSystem;User=sa;Password=Test@Password123;TrustServerCertificate=True
    ports:
      - "5555:80"
    depends_on:
      - sqlserver

  frontend:
    build:
      context: ./Frontend
      dockerfile: Dockerfile
    ports:
      - "4300:80"
    depends_on:
      - backend

volumes:
  sqldata:
```

### 10.2 Dockerfile Backend

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AutomationSystem.API/AutomationSystem.API.csproj", "AutomationSystem.API/"]
COPY ["AutomationSystem.Core/AutomationSystem.Core.csproj", "AutomationSystem.Core/"]
COPY ["AutomationSystem.Infrastructure/AutomationSystem.Infrastructure.csproj", "AutomationSystem.Infrastructure/"]
RUN dotnet restore "AutomationSystem.API/AutomationSystem.API.csproj"
COPY . .
WORKDIR "/src/AutomationSystem.API"
RUN dotnet build "AutomationSystem.API.csproj" -c Release -o /app/build
RUN dotnet publish "AutomationSystem.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN mkdir -p /app/uploads /app/logs
ENTRYPOINT ["dotnet", "AutomationSystem.API.dll"]
```

### 10.3 Déploiement Production

#### Azure App Service

1. Créer une App Service (Linux)
2. Configurer les variables d'environnement
3. Déployer via GitHub Actions ou Azure CLI

#### AWS Elastic Beanstalk

1. Créer une application Elastic Beanstalk
2. Configurer RDS (SQL Server)
3. Déployer le package Docker

#### Serveur Linux (VPS)

```bash
# Installer Docker et Docker Compose
curl -fsSL https://get.docker.com | sh
sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Cloner et démarrer
git clone https://github.com/thoumi/Automation.git
cd Automation
docker-compose up -d
```

---

## 11. Maintenance

### 11.1 Logs

#### Backend (Serilog)

```csharp
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day);
});
```

#### Consulter les logs Docker

```bash
# Backend logs
docker logs automation-backend -f

# SQL Server logs
docker logs automation-sqlserver -f

# Frontend logs
docker logs automation-frontend -f
```

### 11.2 Backup Base de Données

```bash
# Backup manuel
docker exec automation-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Test@Password123" -C -Q "BACKUP DATABASE AutomationSystem TO DISK='/var/opt/mssql/backup/AutomationSystem.bak'"

# Copier le backup
docker cp automation-sqlserver:/var/opt/mssql/backup/AutomationSystem.bak ./backup/
```

#### Backup automatique (cron)

```bash
# Ajouter au crontab
0 2 * * * /path/to/backup-script.sh
```

### 11.3 Monitoring

#### Health Checks

```csharp
app.MapHealthChecks("/health");
```

#### Hangfire Dashboard

Accéder à `http://localhost:5555/hangfire` pour :
- Voir les jobs en cours
- Consulter l'historique
- Réexécuter des jobs échoués

---

## 12. Performance

### 12.1 Optimisations Backend

#### Async/Await
Toutes les opérations I/O utilisent `async/await` :

```csharp
public async Task<List<Task>> GetTasksAsync()
{
    return await _context.ScheduledTasks.ToListAsync();
}
```

#### Caching

```csharp
builder.Services.AddMemoryCache();

// Dans un service
var tasks = await _cache.GetOrCreateAsync("all-tasks", async entry =>
{
    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
    return await _context.ScheduledTasks.ToListAsync();
});
```

#### Connection Pooling

EF Core gère automatiquement le pooling de connexions.

### 12.2 Optimisations Frontend

#### Lazy Loading des Modules

```typescript
{
  path: 'tasks',
  loadChildren: () => import('./features/tasks/tasks.module').then(m => m.TasksModule)
}
```

#### OnPush Change Detection

```typescript
@Component({
  selector: 'app-dashboard',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent { }
```

#### TrackBy dans les *ngFor

```typescript
trackByTaskId(index: number, task: Task): number {
  return task.id;
}
```

```html
<div *ngFor="let task of tasks; trackBy: trackByTaskId">
  {{ task.name }}
</div>
```

### 12.3 Optimisations Base de Données

- Indexes sur colonnes fréquemment filtrées
- Pagination des résultats
- Projection (Select specific columns)
- Avoid N+1 queries (use `.Include()`)

---

## Annexes

### A. Commandes Utiles

```bash
# Docker
docker-compose up -d                    # Démarrer
docker-compose down                     # Arrêter
docker-compose logs -f backend          # Logs backend
docker-compose restart backend          # Redémarrer backend
docker-compose build --no-cache         # Rebuild sans cache

# .NET
dotnet build                            # Compiler
dotnet run                              # Exécuter
dotnet test                             # Tests
dotnet ef migrations add [Name]         # Nouvelle migration
dotnet ef database update               # Appliquer migrations

# Angular
npm install                             # Installer dépendances
ng serve                                # Dev server
ng build                                # Build production
ng test                                 # Tests unitaires
ng lint                                 # Linter

# Git
git add .                               # Stager changements
git commit -m "message"                 # Commit
git push origin main                    # Pousser
git pull origin main                    # Récupérer
```

### B. Troubleshooting

| Problème | Solution |
|----------|----------|
| Port 5555 déjà utilisé | Changer le port dans `docker-compose.yml` |
| SQL Server ne démarre pas | Vérifier les ressources Docker (4GB RAM min) |
| Frontend ne se connecte pas | Vérifier `environment.ts` et CORS backend |
| Jobs Hangfire ne s'exécutent pas | Vérifier CRON expression et logs Hangfire |
| Email non envoyé | Vérifier config SMTP et firewall |

---

**Document mis à jour** : Octobre 2025  
**Version** : 2.0  
**Auteur** : Équipe Développement

