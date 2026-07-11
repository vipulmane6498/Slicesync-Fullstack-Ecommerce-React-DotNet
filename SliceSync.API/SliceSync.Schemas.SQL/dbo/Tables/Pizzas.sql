CREATE TABLE [dbo].[Pizzas] (
    [PizzaId]         UNIQUEIDENTIFIER NOT NULL,
    [PizzaName]       NVARCHAR (MAX)   NOT NULL,
    [Unitprice]       DECIMAL (10, 2)  NOT NULL,
    [Image]           NVARCHAR (MAX)   NULL,
    [PizzaDesciption] NVARCHAR (MAX)   NULL,
    [IsSoldOut]       BIT              NULL,
    [IsActive]        BIT              NULL,
    [CreateAt]        DATETIME2 (7)    NULL,
    CONSTRAINT [PK_Pizzas] PRIMARY KEY CLUSTERED ([PizzaId] ASC)
);

