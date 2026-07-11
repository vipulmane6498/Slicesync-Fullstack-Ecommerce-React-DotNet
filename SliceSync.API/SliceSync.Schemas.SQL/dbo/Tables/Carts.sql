CREATE TABLE [dbo].[Carts] (
    [CartId]            UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]         DATETIME2 (7)    NULL,
    [IsActive]          BIT              NULL,
    [UpdatedAt]         DATETIME2 (7)    NULL,
    [CartPrice]         DECIMAL (18, 2)  NULL,
    [ApplicationUserId] UNIQUEIDENTIFIER NULL,
    [UserId]            UNIQUEIDENTIFIER DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY CLUSTERED ([CartId] ASC),
    CONSTRAINT [FK_Carts_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Carts_ApplicationUserId]
    ON [dbo].[Carts]([ApplicationUserId] ASC);

