CREATE TABLE [dbo].[Movie]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Title] VARCHAR(50) NOT NULL, 
    [CountryOfOrigin] VARCHAR(50) NULL, 
    [ReleaseDate] DATETIME NULL, 
    [Poster] IMAGE NULL
)
