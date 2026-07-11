CREATE TABLE [dbo].[OrderItem] (
    [OrderItemId]     UNIQUEIDENTIFIER NOT NULL,
    [OrderId]         UNIQUEIDENTIFIER NOT NULL,
    [PizzaId]         UNIQUEIDENTIFIER NOT NULL,
    [Quantity]        INT              NOT NULL,
    [PriceAtThatTime] DECIMAL (18, 2)  NULL,
    [PizzaName]       NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED ([OrderItemId] ASC),
    CONSTRAINT [FK_OrderItem_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([OrderId]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderItem_Pizzas_PizzaId] FOREIGN KEY ([PizzaId]) REFERENCES [dbo].[Pizzas] ([PizzaId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_PizzaId]
    ON [dbo].[OrderItem]([PizzaId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OrderItem_OrderId]
    ON [dbo].[OrderItem]([OrderId] ASC);

