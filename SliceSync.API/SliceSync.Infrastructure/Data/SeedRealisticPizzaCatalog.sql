SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Hide legacy pizzas from the customer menu
    UPDATE Pizzas
    SET IsActive = 0
    WHERE IsActive = 1;

    -- Ingredient categories used to render menu ingredients in the frontend
    MERGE Categories AS target
    USING (VALUES
        ('11111111-1111-1111-1111-111111111101', 'Mozzarella', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111102', 'Fresh Basil', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111103', 'San Marzano Tomato', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111104', 'Pepperoni', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111105', 'Italian Sausage', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111106', 'Mushrooms', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111107', 'Red Onion', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111108', 'Black Olives', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111109', 'Green Peppers', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111110', 'Jalapenos', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111111', 'Smoked Chicken', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111112', 'BBQ Sauce', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111113', 'Paneer', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111114', 'Sweet Corn', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111115', 'Pineapple', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111116', 'Parmesan', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111117', 'Ricotta', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111118', 'Spinach', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111119', 'Garlic', 'Ingredient', 1),
        ('11111111-1111-1111-1111-111111111120', 'Cherry Tomatoes', 'Ingredient', 1)
    ) AS source(CategoryId, CategoryName, CategoryType, IsActive)
    ON target.CategoryId = CAST(source.CategoryId AS uniqueidentifier)
    WHEN MATCHED THEN
        UPDATE SET
            target.CategoryName = source.CategoryName,
            target.CategoryType = source.CategoryType,
            target.IsActive = source.IsActive
    WHEN NOT MATCHED THEN
        INSERT (CategoryId, CategoryName, CategoryType, IsActive)
        VALUES (CAST(source.CategoryId AS uniqueidentifier), source.CategoryName, source.CategoryType, source.IsActive);

    -- Realistic pizza catalog with production-like names/pricing/descriptions and valid image URLs
    MERGE Pizzas AS target
    USING (VALUES
        ('22222222-2222-2222-2222-222222222201', 'Margherita Classica', 11.99, 'https://images.pexels.com/photos/315755/pexels-photo-315755.jpeg', 'San Marzano tomato sauce, mozzarella and fresh basil on a thin artisan crust.', 0, 1),
        ('22222222-2222-2222-2222-222222222202', 'Pepperoni Feast', 13.49, 'https://images.pexels.com/photos/825661/pexels-photo-825661.jpeg', 'Classic hand-tossed base topped with mozzarella and generous spicy pepperoni.', 0, 1),
        ('22222222-2222-2222-2222-222222222203', 'BBQ Chicken Supreme', 14.99, 'https://images.pexels.com/photos/2619967/pexels-photo-2619967.jpeg', 'Smoked chicken, red onion and mozzarella finished with bold BBQ sauce.', 0, 1),
        ('22222222-2222-2222-2222-222222222204', 'Veggie Garden Deluxe', 12.99, 'https://images.pexels.com/photos/2147491/pexels-photo-2147491.jpeg', 'Bell peppers, mushrooms, olives and onions with rich tomato base.', 0, 1),
        ('22222222-2222-2222-2222-222222222205', 'Paneer Tikka Fusion', 13.99, 'https://images.pexels.com/photos/845811/pexels-photo-845811.jpeg', 'Indian-inspired paneer tikka, onions and peppers over creamy mozzarella.', 0, 1),
        ('22222222-2222-2222-2222-222222222206', 'Spicy Diablo', 13.79, 'https://images.pexels.com/photos/803290/pexels-photo-803290.jpeg', 'Pepperoni, jalapenos and hot tomato sauce for serious heat lovers.', 0, 1),
        ('22222222-2222-2222-2222-222222222207', 'Mediterranean Olive', 12.79, 'https://images.pexels.com/photos/905847/pexels-photo-905847.jpeg', 'Black olives, cherry tomatoes, onions and basil on olive-oil brushed crust.', 0, 1),
        ('22222222-2222-2222-2222-222222222208', 'Four Cheese Indulgence', 14.29, 'https://images.pexels.com/photos/1435907/pexels-photo-1435907.jpeg', 'Mozzarella, parmesan, ricotta and cheddar for a rich cheesy bite.', 0, 1),
        ('22222222-2222-2222-2222-222222222209', 'Smoky Sausage & Mushroom', 14.19, 'https://images.pexels.com/photos/1566837/pexels-photo-1566837.jpeg', 'Italian sausage, mushrooms and caramelized onion on signature sauce.', 0, 1),
        ('22222222-2222-2222-2222-222222222210', 'Hawaiian Sunset', 12.89, 'https://images.pexels.com/photos/708587/pexels-photo-708587.jpeg', 'Sweet pineapple, smoked chicken and mozzarella balanced with tomato sauce.', 0, 1),
        ('22222222-2222-2222-2222-222222222211', 'Spinach Ricotta White', 13.59, 'https://images.pexels.com/photos/1260968/pexels-photo-1260968.jpeg', 'Creamy garlic white sauce with spinach, ricotta and parmesan.', 0, 1),
        ('22222222-2222-2222-2222-222222222212', 'Corn & Jalapeno Crunch', 12.49, 'https://images.pexels.com/photos/1146760/pexels-photo-1146760.jpeg', 'Sweet corn, jalapenos and onion over mozzarella and tangy tomato base.', 0, 1)
    ) AS source(PizzaId, PizzaName, Unitprice, Image, PizzaDesciption, IsSoldOut, IsActive)
    ON target.PizzaId = CAST(source.PizzaId AS uniqueidentifier)
    WHEN MATCHED THEN
        UPDATE SET
            target.PizzaName = source.PizzaName,
            target.Unitprice = source.Unitprice,
            target.Image = source.Image,
            target.PizzaDesciption = source.PizzaDesciption,
            target.IsSoldOut = source.IsSoldOut,
            target.IsActive = source.IsActive,
            target.CreateAt = COALESCE(target.CreateAt, GETUTCDATE())
    WHEN NOT MATCHED THEN
        INSERT (PizzaId, PizzaName, Unitprice, Image, PizzaDesciption, IsSoldOut, IsActive, CreateAt)
        VALUES (CAST(source.PizzaId AS uniqueidentifier), source.PizzaName, source.Unitprice, source.Image, source.PizzaDesciption, source.IsSoldOut, source.IsActive, GETUTCDATE());

    -- Replace mappings for this seeded catalog
    DELETE pcm
    FROM PizzaCategoryMappings pcm
    WHERE pcm.PizzaId IN (
        '22222222-2222-2222-2222-222222222201',
        '22222222-2222-2222-2222-222222222202',
        '22222222-2222-2222-2222-222222222203',
        '22222222-2222-2222-2222-222222222204',
        '22222222-2222-2222-2222-222222222205',
        '22222222-2222-2222-2222-222222222206',
        '22222222-2222-2222-2222-222222222207',
        '22222222-2222-2222-2222-222222222208',
        '22222222-2222-2222-2222-222222222209',
        '22222222-2222-2222-2222-222222222210',
        '22222222-2222-2222-2222-222222222211',
        '22222222-2222-2222-2222-222222222212'
    );

    INSERT INTO PizzaCategoryMappings (PizzaId, CategoryId)
    VALUES
        ('22222222-2222-2222-2222-222222222201','11111111-1111-1111-1111-111111111101'),
        ('22222222-2222-2222-2222-222222222201','11111111-1111-1111-1111-111111111102'),
        ('22222222-2222-2222-2222-222222222201','11111111-1111-1111-1111-111111111103'),

        ('22222222-2222-2222-2222-222222222202','11111111-1111-1111-1111-111111111101'),
        ('22222222-2222-2222-2222-222222222202','11111111-1111-1111-1111-111111111104'),
        ('22222222-2222-2222-2222-222222222202','11111111-1111-1111-1111-111111111103'),

        ('22222222-2222-2222-2222-222222222203','11111111-1111-1111-1111-111111111111'),
        ('22222222-2222-2222-2222-222222222203','11111111-1111-1111-1111-111111111112'),
        ('22222222-2222-2222-2222-222222222203','11111111-1111-1111-1111-111111111107'),

        ('22222222-2222-2222-2222-222222222204','11111111-1111-1111-1111-111111111106'),
        ('22222222-2222-2222-2222-222222222204','11111111-1111-1111-1111-111111111107'),
        ('22222222-2222-2222-2222-222222222204','11111111-1111-1111-1111-111111111109'),
        ('22222222-2222-2222-2222-222222222204','11111111-1111-1111-1111-111111111108'),

        ('22222222-2222-2222-2222-222222222205','11111111-1111-1111-1111-111111111113'),
        ('22222222-2222-2222-2222-222222222205','11111111-1111-1111-1111-111111111107'),
        ('22222222-2222-2222-2222-222222222205','11111111-1111-1111-1111-111111111109'),

        ('22222222-2222-2222-2222-222222222206','11111111-1111-1111-1111-111111111104'),
        ('22222222-2222-2222-2222-222222222206','11111111-1111-1111-1111-111111111110'),
        ('22222222-2222-2222-2222-222222222206','11111111-1111-1111-1111-111111111103'),

        ('22222222-2222-2222-2222-222222222207','11111111-1111-1111-1111-111111111108'),
        ('22222222-2222-2222-2222-222222222207','11111111-1111-1111-1111-111111111120'),
        ('22222222-2222-2222-2222-222222222207','11111111-1111-1111-1111-111111111102'),

        ('22222222-2222-2222-2222-222222222208','11111111-1111-1111-1111-111111111101'),
        ('22222222-2222-2222-2222-222222222208','11111111-1111-1111-1111-111111111116'),
        ('22222222-2222-2222-2222-222222222208','11111111-1111-1111-1111-111111111117'),

        ('22222222-2222-2222-2222-222222222209','11111111-1111-1111-1111-111111111105'),
        ('22222222-2222-2222-2222-222222222209','11111111-1111-1111-1111-111111111106'),
        ('22222222-2222-2222-2222-222222222209','11111111-1111-1111-1111-111111111107'),

        ('22222222-2222-2222-2222-222222222210','11111111-1111-1111-1111-111111111115'),
        ('22222222-2222-2222-2222-222222222210','11111111-1111-1111-1111-111111111111'),
        ('22222222-2222-2222-2222-222222222210','11111111-1111-1111-1111-111111111101'),

        ('22222222-2222-2222-2222-222222222211','11111111-1111-1111-1111-111111111118'),
        ('22222222-2222-2222-2222-222222222211','11111111-1111-1111-1111-111111111117'),
        ('22222222-2222-2222-2222-222222222211','11111111-1111-1111-1111-111111111119'),

        ('22222222-2222-2222-2222-222222222212','11111111-1111-1111-1111-111111111114'),
        ('22222222-2222-2222-2222-222222222212','11111111-1111-1111-1111-111111111110'),
        ('22222222-2222-2222-2222-222222222212','11111111-1111-1111-1111-111111111107');

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
