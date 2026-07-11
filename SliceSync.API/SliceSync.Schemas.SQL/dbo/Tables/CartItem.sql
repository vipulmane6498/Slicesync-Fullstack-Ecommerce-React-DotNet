CREATE TABLE [dbo].[CartItem] (
    [CartItemId]      UNIQUEIDENTIFIER NOT NULL,
    [CartId]          UNIQUEIDENTIFIER NOT NULL,
    [PizzaId]         UNIQUEIDENTIFIER NOT NULL,
    [PriceAtThatTime] DECIMAL (18, 2)  NULL,
    [Quantity]        INT              DEFAULT ((0)) NOT NULL,
    [PizzaName]       NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_CartItem] PRIMARY KEY CLUSTERED ([CartItemId] ASC),
    CONSTRAINT [FK_CartItem_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [dbo].[Carts] ([CartId]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItem_Pizzas_PizzaId] FOREIGN KEY ([PizzaId]) REFERENCES [dbo].[Pizzas] ([PizzaId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_CartItem_PizzaId]
    ON [dbo].[CartItem]([PizzaId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CartItem_CartId]
    ON [dbo].[CartItem]([CartId] ASC);

