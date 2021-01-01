CREATE TABLE [dbo].[Task]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [InitiationDate] DATETIME NULL, 
    [FinishDate] DATETIME NULL, 
    [SaleId] INT NULL, 
    CONSTRAINT [FK_Task_ToSaleId] FOREIGN KEY ([SaleId]) REFERENCES [Sale]([Id]) 
)
