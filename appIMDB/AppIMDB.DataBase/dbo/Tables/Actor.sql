CREATE TABLE [dbo].[Actor]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Name] VARCHAR(50) NOT NULL, 
    [BirthDate] DATETIME NOT NULL, 
    [Nationality] VARCHAR(50) NULL
)
