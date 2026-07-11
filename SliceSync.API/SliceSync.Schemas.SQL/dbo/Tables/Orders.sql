CREATE TABLE [dbo].[Orders] (
    [OrderId]         UNIQUEIDENTIFIER NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt]       DATETIME2 (7)    NULL,
    [UpdatedAt]       DATETIME2 (7)    NULL,
    [OrderStatus]     NVARCHAR (50)    NULL,
    [TotalOrderPrice] DECIMAL (18, 2)  NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderId] ASC),
    CONSTRAINT [FK_Orders_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Orders_UserId]
    ON [dbo].[Orders]([UserId] ASC);

