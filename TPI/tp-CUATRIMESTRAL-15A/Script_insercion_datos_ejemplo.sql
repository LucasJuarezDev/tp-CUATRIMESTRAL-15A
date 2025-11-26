USE TPIcomercioBD;
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
('HP', 'Impresoras y computadoras', 0); -- Inactiva
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
-- ADMIN
('ADMIN', 'ADMIN123', 'ADMIN@TPIPROGRA3.COM', 1, 1),
-- EMPLEADOS
('juanperez', 'juan2025', 'juan.perez@tienda.com', 2, 1),
('mariagomez', 'maria2025', 'maria.gomez@tienda.com', 2, 1),
-- CLIENTES
('carlos87', 'carlos123', 'carlos87@gmail.com', 3, 1),
('laura_92', 'laura123', 'laura.92@hotmail.com', 3, 1),
('pedroclient', 'pedro123', 'pedro.client@outlook.com', 3, 1),
('Ana', 'ana123', 'ana.rodriguez@outlook.com', 3, 1),
('EmpresaXYZ', 'corp123', 'xyz@empresa.com', 3, 1);
GO


-- =============================================
-- 6. INSERCIÓN EN TABLA EMPLEADO
-- =============================================
INSERT INTO EMPLEADO (NOMBRE, APELLIDO, TELEFONO, FECHAINGRESO, SUELDO, ID_USUARIO, ACTIVO) VALUES
('Juan', 'Pérez', '3001234567', '2024-01-15', 2500000.00,
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'juanperez'), 1),
('María', 'Gómez', '3007654321', '2023-11-20', 2800000.00,
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'mariagomez'), 1);
GO

-- =============================================
-- 7. INSERCIÓN EN TABLA CLIENTE
-- =============================================
INSERT INTO CLIENTE (NOMBRE, APELLIDO, TELEFONO, FECHA_REGISTRO, ROLE_ID, ID_USUARIO, RAZON_SOCIAL, ACTIVO) VALUES
('Carlos', 'López', '3109876543', '2025-01-10', 3,
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'carlos87'), NULL, 1),
('Laura', 'Martínez', '3151234567', '2025-02-05', 3,
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'laura_92'), 'Laura Moda SAS', 1),
('Pedro', 'Ramírez', '3204567890', '2025-03-01', 3,
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'pedroclient'), NULL, 1),
('Ana', 'Rodríguez', '3001112233', '2025-04-12', 3,
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'Ana'), 'Distribuidora Ana', 1),
('Empresa XYZ', 'Corp', '6012345678', '2025-05-01', 3,
    (SELECT ID FROM USUARIO WHERE NICKNAME = 'EmpresaXYZ'), 'XYZ Importaciones S.A.', 1);
GO

-- =============================================
-- 8. INSERCIÓN EN TABLA PRODUCTO (ACTUALIZADO)
-- Sin DESCRIPCION → con DESCRIPCION_CORTA, DESCRIPCION_EXTENDIDA e IMAGEN_URL
-- =============================================
INSERT INTO PRODUCTO (
    NOMBRE, PRECIO, 
    DESCRIPCION_CORTA, DESCRIPCION_EXTENDIDA,
    STOCK, STOCK_MINIMO, 
    ID_MARCA, ID_CATEGORIA, ACTIVO
) VALUES
-- 1. iPhone 15 Pro
('iPhone 15 Pro', 5500000.00,
 '256GB, 8GB RAM, A17 Pro',
 'El iPhone 15 Pro redefine la experiencia móvil con su potente chip A17 Pro, pantalla Super Retina XDR de 6.1" con ProMotion y cámara triple de 48MP que captura detalles impresionantes incluso en condiciones de poca luz. Su diseño en titanio lo hace más ligero y resistente, con protección IP68 contra agua y polvo. Incluye carga rápida, Face ID avanzado y el nuevo botón de acción personalizable.',
 15, 5,
 (SELECT ID FROM MARCA WHERE NOMBRE = 'Apple'),
 (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Celulares'), 1),

-- 2. Galaxy S24 Ultra
('Galaxy S24 Ultra', 6200000.00,
 '512GB, 12GB RAM, 200MP',
 'El Galaxy S24 Ultra es la máxima expresión de tecnología Android. Su cámara principal de 200MP con zoom óptico 10x y 100x digital captura fotos de calidad profesional. La pantalla Dynamic AMOLED 2X de 6.8" con 120Hz ofrece colores vibrantes y fluidez total. Incluye S-Pen integrado, batería de 5000mAh con carga ultrarrápida de 45W y el procesador Snapdragon 8 Gen 3 para un rendimiento sin igual.',
 8, 3,
 (SELECT ID FROM MARCA WHERE NOMBRE = 'Samsung'),
 (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Celulares'), 1),

-- 3. MacBook Air M2
('MacBook Air M2', 5200000.00,
 '256GB SSD, 8GB RAM, M2',
 'La MacBook Air con chip M2 combina potencia y portabilidad. Su pantalla Liquid Retina de 13.6" ofrece colores brillantes y nitidez excepcional. El chip M2 con CPU de 8 núcleos y GPU de 8 núcleos maneja edición de video, diseño gráfico y multitarea sin esfuerzo. Batería de hasta 18 horas, Touch ID, carga MagSafe y un diseño ultradelgado de solo 1.13 kg la hacen perfecta para trabajar desde cualquier lugar.',
 10, 2,
 (SELECT ID FROM MARCA WHERE NOMBRE = 'Apple'),
 (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Laptops'), 1),

-- 4. Laptop Inspiron 15
('Laptop Inspiron 15', 3200000.00,
 '512GB SSD, 16GB RAM, i5',
 'La Dell Inspiron 15 es ideal para trabajo y estudio. Equipada con procesador Intel Core i5 de 12ª generación, 16GB de RAM y SSD de 512GB, ofrece arranque rápido y multitarea fluida. Su pantalla Full HD de 15.6" con tasa de refresco de 120Hz reduce el desenfoque en movimiento. Incluye teclado retroiluminado, Windows 11 Home y puertos USB-C, HDMI y lector de tarjetas.',
 20, 5,
 (SELECT ID FROM MARCA WHERE NOMBRE = 'Dell'),
 (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Laptops'), 1),

-- 5. Auriculares WH-1000XM5
('Auriculares WH-1000XM5', 1350000.00,
 'Bluetooth, ANC, 30h batería',
 'Los Sony WH-1000XM5 son la referencia en cancelación de ruido. Su tecnología ANC líder bloquea distracciones del entorno, mientras el sonido Hi-Res Audio ofrece calidad de estudio. Con 30 horas de batería, detección automática de uso, micrófono con reducción de ruido por IA y conexión multipunto, son perfectos para viajes, oficina o escuchar música con total inmersión.',
 25, 10,
 (SELECT ID FROM MARCA WHERE NOMBRE = 'Sony'),
 (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Audio'), 1),

-- 6. Zapatillas Air Max 270
('Zapatillas Air Max 270', 650000.00,
 'Talle 42, Negro/Rojo',
 'Las Nike Air Max 270 combinan estilo urbano con máxima comodidad. Su unidad Air 360 en el talón ofrece amortiguación reactiva en cada paso. La malla transpirable mantiene tus pies frescos, mientras la suela de goma con diseño waffle garantiza tracción en cualquier superficie. Ideales para uso diario, caminatas o entrenamientos ligeros con un look moderno y atrevido.',
 30, 8,
 (SELECT ID FROM MARCA WHERE NOMBRE = 'Nike'),
 (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Calzado'), 1),

-- 7. Camiseta Dri-FIT
('Camiseta Dri-FIT', 120000.00,
 'Talle M, Dry-Fit, Negra',
 'La camiseta Nike Dri-FIT está diseñada para mantenerte seco y cómodo durante el ejercicio. Su tecnología absorbe el sudor y lo evapora rápidamente. Fabricada con poliéster 100% reciclado, es suave al tacto y ecológica. Costuras planas evitan irritaciones, y el ajuste atlético permite libertad de movimiento. Perfecta para running, gym o uso casual deportivo.',
 50, 15,
 (SELECT ID FROM MARCA WHERE NOMBRE = 'Nike'),
 (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Ropa Deportiva'), 1),

-- 8. Funda iPhone 15
('Funda iPhone 15', 85000.00,
 'Silicona, MagSafe, Negra',
 'Protege tu iPhone 15 con esta funda oficial de silicona con MagSafe. Su exterior suave al tacto y forro de microfibra protegen contra rayones y golpes. Los imanes integrados aseguran alineación perfecta con cargadores MagSafe y accesorios. Bordes elevados resguardan pantalla y cámara. Antideslizante, elegante y con ajuste preciso a todos los botones.',
 100, 20,
 (SELECT ID FROM MARCA WHERE NOMBRE = 'Apple'),
 (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Accesorios'), 1),

-- 9. Cargador 65W USB-C
('Cargador 65W USB-C', 180000.00,
 '65W, PD 3.0, 2 puertos',
 'Cargador GaN de 65W ultracompacto con tecnología de nitruro de galio para mayor eficiencia y menos calor. Incluye puerto USB-C con Power Delivery 3.0 (hasta 65W) y USB-A con Quick Charge 4.0. Carga laptops, tablets y smartphones a máxima velocidad. Diseño plegable, protección contra sobrecarga, sobrecalentamiento y cortocircuito. Ideal para viajes y uso diario.',
 80, 10,
 (SELECT ID FROM MARCA WHERE NOMBRE = 'Samsung'),
 (SELECT ID FROM CATEGORIA WHERE NOMBRE = 'Accesorios'), 1);
GO

-- =============================================
-- VERIFICACIÓN FINAL
-- =============================================
SELECT 'PRODUCTOS INSERTADOS:' AS Info, COUNT(*) AS Total FROM PRODUCTO;
SELECT 'USUARIOS:' AS Info, COUNT(*) AS Total FROM USUARIO;
SELECT 'CLIENTES:' AS Info, COUNT(*) AS Total FROM CLIENTE;
GO

SELECT * FROM USUARIO

SELECT * FROM CLIENTE

SELECT * FROM EMPLEADO

SELECT * FROM PRODUCTO

SELECT * FROM MARCA

SELECT * FROM CATEGORIA