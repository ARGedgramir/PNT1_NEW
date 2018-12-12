CREATE TABLE [dbo].[MovieRole]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Title] VARCHAR(50) NOT NULL, 
    [MovieId] INT NOT NULL, 
    [ActorId] INT NOT NULL, 
    CONSTRAINT FK_MovieMovieRole FOREIGN KEY (MovieId) REFERENCES Movie (Id), 
    CONSTRAINT FK_MovieRole_Actor FOREIGN KEY (ActorId) REFERENCES Actor(Id)
	
)
