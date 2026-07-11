CREATE TABLE [dbo].[PizzaCategoryMappings] (
    [PizzaId]    UNIQUEIDENTIFIER NOT NULL,
    [CategoryId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PizzaCategoryMappings] PRIMARY KEY CLUSTERED ([PizzaId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_PizzaCategoryMappings_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([CategoryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PizzaCategoryMappings_Pizzas_PizzaId] FOREIGN KEY ([PizzaId]) REFERENCES [dbo].[Pizzas] ([PizzaId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_PizzaCategoryMappings_CategoryId]
    ON [dbo].[PizzaCategoryMappings]([CategoryId] ASC);

