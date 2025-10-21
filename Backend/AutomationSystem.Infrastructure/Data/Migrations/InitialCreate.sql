-- Create TaskExecutionLogs Table
CREATE TABLE TaskExecutionLogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TaskName NVARCHAR(200) NOT NULL,
    ExecutionTime DATETIME2 NOT NULL,
    Status INT NOT NULL,
    Message NVARCHAR(MAX),
    ErrorDetails NVARCHAR(MAX),
    DurationMs INT NOT NULL,
    INDEX IX_TaskExecutionLogs_TaskName_ExecutionTime (TaskName, ExecutionTime)
);

-- Create ScheduledTasks Table
CREATE TABLE ScheduledTasks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    CronExpression NVARCHAR(100) NOT NULL,
    IsEnabled BIT NOT NULL,
    TaskType NVARCHAR(100) NOT NULL,
    Configuration NVARCHAR(MAX),
    LastExecutionTime DATETIME2,
    NextExecutionTime DATETIME2,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);

-- Create NotificationRecipients Table
CREATE TABLE NotificationRecipients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Type INT NOT NULL,
    Identifier NVARCHAR(200) NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    IsActive BIT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    INDEX IX_NotificationRecipients_Type_IsActive (Type, IsActive)
);

-- Create FileUploads Table
CREATE TABLE FileUploads (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FileName NVARCHAR(500) NOT NULL,
    OriginalFileName NVARCHAR(500) NOT NULL,
    FilePath NVARCHAR(1000) NOT NULL,
    FileType NVARCHAR(50) NOT NULL,
    FileSize BIGINT NOT NULL,
    UploadedBy NVARCHAR(200) NOT NULL,
    UploadedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ProcessingStatus INT NOT NULL,
    ProcessingMessage NVARCHAR(MAX),
    INDEX IX_FileUploads_UploadedAt (UploadedAt)
);

-- Insert default scheduled tasks
INSERT INTO ScheduledTasks (Name, Description, CronExpression, IsEnabled, TaskType, CreatedAt)
VALUES 
    ('Routenverfügbarkeit', 'Envoi quotidien du Routenverfügbarkeit à 08h25', '25 8 * * *', 1, 'Routenverfuegbarkeit', GETUTCDATE()),
    ('Staging Plan', 'Traitement du Staging Plan à 09h15', '15 9 * * *', 1, 'StagingPlan', GETUTCDATE()),
    ('DNR Units Processing', 'Vérification des emails DNR Units toutes les 15 minutes', '*/15 * * * *', 1, 'DNRUnits', GETUTCDATE());

