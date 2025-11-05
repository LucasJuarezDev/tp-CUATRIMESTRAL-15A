
-- =============================================
-- INICIO - ENTRAR A LA DB
-- =============================================
USE TPIcomercioBD;
GO

-- =============================================
-- 1. INSERCIÓN EN TABLA ROL
-- =============================================
-- 1: ADMIN, 2: EMPLEADO, 3: CLIENTE
INSERT INTO ROL (ROL) VALUES 
('ADMIN'),      -- ID 1
('EMPLEADO'),   -- ID 2
('CLIENTE');    -- ID 3
GO

-- =============================================
-- 2. INSERCIÓN EN TABLA TIPO_PAGO
-- =============================================
INSERT INTO TIPO_PAGO (NOMBRE, DESCRIPCION) VALUES
('EFECTIVO', 'Pago en efectivo al momento de la venta'),
('TRANSFERENCIA', 'Pago mediante transferencia bancaria'),
('TARJETA DE CREDITO', 'Pago con tarjeta de crédito (Visa, MasterCard, etc.)'),
('CHEQUE', 'Pago mediante cheque nominativo');
GO

-- =============================================
-- 3. INSERCIÓN EN TABLA MARCA
-- =============================================
INSERT INTO MARCA (NOMBRE, DESCRIPCION, ACTIVO) VALUES
('Samsung', 'Electrónicos y electrodomésticos', 1),
('Apple', 'Dispositivos móviles y computadoras', 1),
('Sony', 'Audio, video y entretenimiento', 1),
('Nike', 'Ropa y calzado deportivo', 1),
('Adidas', 'Ropa deportiva y accesorios', 1),
('Dell', 'Computadoras y laptops', 1),
('HP', 'Impresoras y computadoras', 0); -- Ejemplo de marca inactiva
GO

-- =============================================
-- 4. INSERCIÓN EN TABLA CATEGORIA
-- =============================================
INSERT INTO CATEGORIA (NOMBRE, DESCRIPCION, ACTIVO) VALUES
('Celulares', 'Smartphones y teléfonos móviles', 1),
('Laptops', 'Computadoras portátiles', 1),
('Audio', 'Auriculares, parlantes y sistemas de sonido', 1),
('Ropa Deportiva', 'Prendas para entrenamiento y uso diario', 1),
('Calzado', 'Zapatillas y zapatos deportivos', 1),
('Accesorios', 'Fundas, cargadores, cables, etc.', 1);
GO

-- =============================================
-- 5. INSERCIÓN EN TABLA USUARIO
-- =============================================
INSERT INTO USUARIO (NICKNAME, CONTRASENA, EMAIL, ROLE_ID, ACTIVO) VALUES
-- ADMIN (único)
('admin', 'admin123', 'admin@tienda.com', 1, 1),

-- EMPLEADOS (ROLE_ID = 2)
('juanperez', 'juan2025', 'juan.perez@tienda.com', 2, 1),
('mariagomez', 'maria2025', 'maria.gomez@tienda.com', 2, 1),

-- CLIENTES con cuenta (ROLE_ID = 3)
('carlos87', 'carlos123', 'carlos87@gmail.com', 3, 1),
('laura_92', 'laura123', 'laura.92@hotmail.com', 3, 1),
('pedroclient', 'pedro123', 'pedro.client@outlook.com', 3, 1),
INSERT INTO USUARIO (NICKNAME, CONTRASENA, EMAIL, ROLE_ID, ACTIVO) VALUES
('Ana', 'Rodríguez', 'Rodríguez.client@outlook.com', 3, 1),
('Empresa XYZ', 'Corp', 'Empresa XYZ@outlook.com', 3, 1);
GO

-- =============================================
-- 6. INSERCIÓN EN TABLA EMPLEADO
-- Vinculados a usuarios con ROLE_ID = 2
-- =============================================
INSERT INTO EMPLEADO (NOMBRE, APELLIDO, TELEFONO, FECHAINGRESO, SUELDO, ID_USUARIO, ACTIVO) VALUES
('Juan', 'Pérez', '3001234567', '2024-01-15', 2500000.00, 
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'juanperez'), 1),

('María', 'Gómez', '3007654321', '2023-11-20', 2800000.00, 
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'mariagomez'), 1);
GO

-- =============================================
-- 7. INSERCIÓN EN TABLA CLIENTE
-- clientes con cuenta (vinculados a USUARIO)
-- =============================================
INSERT INTO CLIENTE (NOMBRE, APELLIDO, TELEFONO, FECHA_REGISTRO, ROLE_ID, ID_USUARIO, RAZON_SOCIAL, ACTIVO) VALUES
-- Clientes con cuenta (vinculados)
('Carlos', 'López', '3109876543', '2025-01-10', 3, 
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'carlos87'), NULL, 1),

('Laura', 'Martínez', '3151234567', '2025-02-05', 3, 
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'laura_92'), 'Laura Moda SAS', 1),

('Pedro', 'Ramírez', '3204567890', '2025-03-01', 3, 
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'pedroclient'), NULL, 1),

('Ana', 'Rodríguez', '3001112233', '2025-04-12', 3, (SELECT ID FROM USUARIO WHERE NICKNAME = 'Ana'), 'Distribuidora Ana', 1),
('Empresa XYZ', 'Corp', '6012345678', '2025-05-01', 3, (SELECT ID FROM USUARIO WHERE NICKNAME = 'Empresa XYZ'), 'XYZ Importaciones S.A.', 1);
GO

select * from CLIENTE

-- =============================================
-- 8. INSERCIÓN EN TABLA PRODUCTO
-- Usamos marcas y categorías ya insertadas
-- =============================================
INSERT INTO PRODUCTO (NOMBRE, PRECIO, DESCRIPCION, STOCK, STOCK_MINIMO, ID_MARCA, ID_CATEGORIA, ACTIVO) VALUES
('iPhone 15 Pro', 5500000.00, '256GB, Titanio Negro', 15, 5, 
    (SELECT ID FROM MARCA WHERE NOMBRE = 'Apple'), 
    (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Celulares'), 1),

('Galaxy S24 Ultra', 6200000.00, '512GB, 12GB RAM', 8, 3, 
    (SELECT ID FROM MARCA WHERE NOMBRE = 'Samsung'), 
    (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Celulares'), 1),

('MacBook Air M2', 5200000.00, '13", 8GB RAM, 256GB SSD', 10, 2, 
    (SELECT ID FROM MARCA WHERE NOMBRE = 'Apple'), 
    (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Laptops'), 1),

('Laptop Inspiron 15', 3200000.00, 'Intel i5, 16GB RAM, 512GB SSD', 20, 5, 
    (SELECT ID FROM MARCA WHERE NOMBRE = 'Dell'), 
    (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Laptops'), 1),

('Auriculares WH-1000XM5', 1350000.00, 'Cancelación de ruido, 30h batería', 25, 10, 
    (SELECT ID FROM MARCA WHERE NOMBRE = 'Sony'), 
    (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Audio'), 1),

('Zapatillas Air Max 270', 650000.00, 'Talla 40, color negro', 30, 8, 
    (SELECT ID FROM MARCA WHERE NOMBRE = 'Nike'), 
    (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Calzado'), 1),

('Camiseta Dri-FIT', 120000.00, 'Talla M, transpirable', 50, 15, 
    (SELECT ID FROM MARCA WHERE NOMBRE = 'Nike'), 
    (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Ropa Deportiva'), 1),

('Funda iPhone 15', 85000.00, 'Silicona negra, MagSafe', 100, 20, 
    (SELECT ID FROM MARCA WHERE NOMBRE = 'Apple'), 
    (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Accesorios'), 1),

('Cargador 65W USB-C', 180000.00, 'Carga rápida PD', 80, 10, 
    (SELECT ID FROM MARCA WHERE NOMBRE = 'Samsung'), 
    (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Accesorios'), 1);
GO
