CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProductName] NCHAR(100) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [CreateDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [LastModified] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [RetailPrice] MONEY NOT NULL
)
