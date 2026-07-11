CREATE TABLE [dbo].[OrderStatusHistories] (
    [OrderStatusHistoryId] UNIQUEIDENTIFIER NOT NULL,
    [OrderId]              UNIQUEIDENTIFIER NOT NULL,
    [OrderStatus]          NVARCHAR (50)    NULL,
    [UserId]               UNIQUEIDENTIFIER NOT NULL,
    [Role]                 NVARCHAR (20)    NULL,
    [Note]                 NVARCHAR (250)   NULL,
    [CreatedAt]            DATETIME2 (7)    NULL,
    CONSTRAINT [PK_OrderStatusHistories] PRIMARY KEY CLUSTERED ([OrderStatusHistoryId] ASC),
    CONSTRAINT [FK_OrderStatusHistories_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_OrderStatusHistories_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([OrderId])
);


GO
CREATE NONCLUSTERED INDEX [IX_OrderStatusHistories_UserId]
    ON [dbo].[OrderStatusHistories]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OrderStatusHistories_OrderId]
    ON [dbo].[OrderStatusHistories]([OrderId] ASC);

