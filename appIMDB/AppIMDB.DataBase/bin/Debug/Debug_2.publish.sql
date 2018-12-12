﻿/*
Deployment script for AppIMDB.DataBase

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "AppIMDB.DataBase"
:setvar DefaultFilePrefix "AppIMDB.DataBase"
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
PRINT N'Altering [dbo].[Actor]...';


GO
ALTER TABLE [dbo].[Actor] ALTER COLUMN [BirthDate] DATETIME NOT NULL;

ALTER TABLE [dbo].[Actor] ALTER COLUMN [Name] VARCHAR (50) NOT NULL;


GO
PRINT N'Altering [dbo].[Movie]...';


GO
ALTER TABLE [dbo].[Movie] ALTER COLUMN [CountryOfOrigin] VARCHAR (50) NULL;

ALTER TABLE [dbo].[Movie] ALTER COLUMN [Title] VARCHAR (50) NOT NULL;


GO
PRINT N'Altering [dbo].[MovieRole]...';


GO
ALTER TABLE [dbo].[MovieRole] ALTER COLUMN [Title] VARCHAR (50) NOT NULL;


GO
PRINT N'Update complete.';


GO
