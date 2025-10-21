USE AutomationSystem;
GO

-- Corriger la table FileUploads
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FileUploads]') AND name = 'ContentType')
BEGIN
    -- Supprimer les anciennes colonnes et en ajouter de nouvelles
    ALTER TABLE [dbo].[FileUploads] DROP CONSTRAINT IF EXISTS FK_FileUploads_Users;
    ALTER TABLE [dbo].[FileUploads] DROP COLUMN IF EXISTS ContentType;
    ALTER TABLE [dbo].[FileUploads] DROP COLUMN IF EXISTS Description;
    ALTER TABLE [dbo].[FileUploads] DROP COLUMN IF EXISTS UploadedBy;
    
    -- Ajouter les nouvelles colonnes
    ALTER TABLE [dbo].[FileUploads] ADD OriginalFileName NVARCHAR(512) NOT NULL DEFAULT '';
    ALTER TABLE [dbo].[FileUploads] ADD FileType NVARCHAR(256) NOT NULL DEFAULT '';
    ALTER TABLE [dbo].[FileUploads] ADD UploadedBy NVARCHAR(256) NOT NULL DEFAULT '';
    ALTER TABLE [dbo].[FileUploads] ADD ProcessingStatus INT NOT NULL DEFAULT 0;
    ALTER TABLE [dbo].[FileUploads] ADD ProcessingMessage NVARCHAR(MAX) NULL;
    
    PRINT 'FileUploads table updated successfully';
END
GO

-- Corriger la table TaskExecutionLogs pour ajouter TaskId si manquant
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TaskExecutionLogs]') AND name = 'TaskId')
BEGIN
    ALTER TABLE [dbo].[TaskExecutionLogs] ADD TaskId INT NULL;
    PRINT 'TaskId column added to TaskExecutionLogs';
END
GO

PRINT 'Database schema fixed successfully!';
GO

