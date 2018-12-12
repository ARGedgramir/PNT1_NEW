﻿/*
Deployment script for DBTest

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "DBTest"
:setvar DefaultFilePrefix "DBTest"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367)) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
PRINT N'Rename refactoring operation with key 03103ace-b973-4b4a-a509-c5f93153c49b is skipped, element [dbo].[MovieRole].[Ttle] (SqlSimpleColumn) will not be renamed to Title';


GO
PRINT N'Dropping [dbo].[FK_StudentsCourses_CourseId]...';


GO
ALTER TABLE [dbo].[StudentsCourses] DROP CONSTRAINT [FK_StudentsCourses_CourseId];


GO
PRINT N'Dropping [dbo].[FK_Courses_DepartmentId]...';


GO
ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [FK_Courses_DepartmentId];


GO
PRINT N'Dropping [dbo].[FK_StudentsCourses_StudentId]...';


GO
ALTER TABLE [dbo].[StudentsCourses] DROP CONSTRAINT [FK_StudentsCourses_StudentId];


GO
PRINT N'Dropping [dbo].[Courses]...';


GO
DROP TABLE [dbo].[Courses];


GO
PRINT N'Dropping [dbo].[Departments]...';


GO
DROP TABLE [dbo].[Departments];


GO
PRINT N'Dropping [dbo].[Students]...';


GO
DROP TABLE [dbo].[Students];


GO
PRINT N'Dropping [dbo].[StudentsCourses]...';


GO
DROP TABLE [dbo].[StudentsCourses];


GO
PRINT N'Creating [dbo].[Actor]...';


GO
CREATE TABLE [dbo].[Actor] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50) NULL,
    [BirthDate]   DATETIME     NULL,
    [Nationality] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Movie]...';


GO
CREATE TABLE [dbo].[Movie] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [Title]           VARCHAR (50) NULL,
    [CountryOfOrigin] NCHAR (10)   NULL,
    [ReleaseDate]     DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[MovieRole]...';


GO
CREATE TABLE [dbo].[MovieRole] (
    [Id]      INT          NOT NULL,
    [Title]   VARCHAR (50) NULL,
    [MovieId] INT          NOT NULL,
    [ActorId] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[FK_MovieMovieRole]...';


GO
ALTER TABLE [dbo].[MovieRole] WITH NOCHECK
    ADD CONSTRAINT [FK_MovieMovieRole] FOREIGN KEY ([MovieId]) REFERENCES [dbo].[Movie] ([Id]);


GO
PRINT N'Creating [dbo].[FK_MovieRole_Actor]...';


GO
ALTER TABLE [dbo].[MovieRole] WITH NOCHECK
    ADD CONSTRAINT [FK_MovieRole_Actor] FOREIGN KEY ([ActorId]) REFERENCES [dbo].[Actor] ([Id]);


GO
-- Refactoring step to update target server with deployed transaction logs

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '03103ace-b973-4b4a-a509-c5f93153c49b')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('03103ace-b973-4b4a-a509-c5f93153c49b')

GO

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[MovieRole] WITH CHECK CHECK CONSTRAINT [FK_MovieMovieRole];

ALTER TABLE [dbo].[MovieRole] WITH CHECK CHECK CONSTRAINT [FK_MovieRole_Actor];


GO
PRINT N'Update complete.';


GO
