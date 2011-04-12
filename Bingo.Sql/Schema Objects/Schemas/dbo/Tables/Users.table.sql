CREATE TABLE [dbo].[Users] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Kerb]  VARCHAR (16)   NOT NULL,
    [Name]  NVARCHAR (100) NOT NULL,
    [Board] VARCHAR (256)  NOT NULL
);

