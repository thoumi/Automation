-- Créer les tables de l'application
USE AutomationSystem;
GO

-- Table Users
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Email] NVARCHAR(256) NOT NULL UNIQUE,
        [PasswordHash] NVARCHAR(MAX) NOT NULL,
        [FullName] NVARCHAR(256) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [LastLoginAt] DATETIME2 NULL
    );
END
GO

-- Table ScheduledTasks
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScheduledTasks]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ScheduledTasks] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(256) NOT NULL,
        [Description] NVARCHAR(MAX) NULL,
        [JobType] NVARCHAR(256) NOT NULL,
        [CronExpression] NVARCHAR(128) NOT NULL,
        [IsEnabled] BIT NOT NULL DEFAULT 1,
        [LastExecutionTime] DATETIME2 NULL,
        [NextExecutionTime] DATETIME2 NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- Table TaskExecutionLogs
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskExecutionLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[TaskExecutionLogs] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [TaskId] INT NULL,
        [TaskName] NVARCHAR(256) NOT NULL,
        [ExecutionTime] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [Status] INT NOT NULL,
        [DurationMs] BIGINT NOT NULL DEFAULT 0,
        [Message] NVARCHAR(MAX) NULL,
        [ErrorDetails] NVARCHAR(MAX) NULL,
        FOREIGN KEY ([TaskId]) REFERENCES [dbo].[ScheduledTasks]([Id]) ON DELETE SET NULL
    );
END
GO

-- Table NotificationRecipients
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationRecipients]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[NotificationRecipients] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(256) NOT NULL,
        [Type] INT NOT NULL,
        [Identifier] NVARCHAR(512) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- Table FileUploads
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FileUploads]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[FileUploads] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FileName] NVARCHAR(512) NOT NULL,
        [FilePath] NVARCHAR(1024) NOT NULL,
        [FileSize] BIGINT NOT NULL,
        [ContentType] NVARCHAR(256) NULL,
        [UploadedBy] INT NULL,
        [UploadedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [Description] NVARCHAR(MAX) NULL,
        FOREIGN KEY ([UploadedBy]) REFERENCES [dbo].[Users]([Id]) ON DELETE SET NULL
    );
END
GO

-- Insérer un utilisateur admin par défaut
-- Email: admin@example.com
-- Mot de passe: admin123
IF NOT EXISTS (SELECT * FROM [dbo].[Users] WHERE [Email] = 'admin@example.com')
BEGIN
    INSERT INTO [dbo].[Users] ([Email], [PasswordHash], [FullName], [IsActive], [CreatedAt])
    VALUES 
    (
        'admin@example.com',
        'admin123',
        'Administrator',
        1,
        GETUTCDATE()
    );
    
    PRINT 'Admin user created successfully';
    PRINT 'Email: admin@example.com';
    PRINT 'Password: admin123';
END
ELSE
BEGIN
    PRINT 'Admin user already exists';
END
GO

-- Insérer quelques tâches planifiées par défaut
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduledTasks] WHERE [Name] = 'Routenverfuegbarkeit Check')
BEGIN
    INSERT INTO [dbo].[ScheduledTasks] ([Name], [Description], [JobType], [CronExpression], [IsEnabled])
    VALUES 
    (
        'Routenverfuegbarkeit Check',
        'Vérifie la disponibilité des routes quotidiennement',
        'RoutenverfuegbarkeitJob',
        '0 8 * * *',
        1
    );
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduledTasks] WHERE [Name] = 'Staging Plan Generation')
BEGIN
    INSERT INTO [dbo].[ScheduledTasks] ([Name], [Description], [JobType], [CronExpression], [IsEnabled])
    VALUES 
    (
        'Staging Plan Generation',
        'Génère le plan de staging chaque semaine',
        'StagingPlanJob',
        '0 9 * * 1',
        1
    );
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduledTasks] WHERE [Name] = 'DNR Units Processing')
BEGIN
    INSERT INTO [dbo].[ScheduledTasks] ([Name], [Description], [JobType], [CronExpression], [IsEnabled])
    VALUES 
    (
        'DNR Units Processing',
        'Traite les unités DNR toutes les heures',
        'DNRUnitsJob',
        '0 * * * *',
        1
    );
END
GO

PRINT 'Database initialization completed successfully!';
GO

