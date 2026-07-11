CREATE TABLE [dbo].[Categories] (
    [CategoryId]   UNIQUEIDENTIFIER NOT NULL,
    [CategoryType] NVARCHAR (MAX)   NULL,
    [CategoryName] NVARCHAR (MAX)   NULL,
    [IsActive]     BIT              NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);

