CREATE TABLE [dbo].[User]
( 
    [Id] NVARCHAR(128) NOT NULL Primary Key, 
    [FirstName] NCHAR(50) NOT NULL, 
    [LastName] NCHAR(50) NOT NULL, 
    [EmailAddress] NCHAR(256) NOT NULL, 
    [CreateDate] DATETIME2 NOT NULL DEFAULT getutcdate()
)
