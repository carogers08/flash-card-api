IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'TestTable') 
BEGIN
	CREATE TABLE [dbo].[TestTable] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[SampleText] VARCHAR(100),
		[TimeStamp] DATETIME
	);

	INSERT INTO TestTable ([SampleText],[TimeStamp]) VALUES ('Powershell test!', GETDATE());
END