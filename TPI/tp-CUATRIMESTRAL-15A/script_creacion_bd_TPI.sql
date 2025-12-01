-- BIENVENIDOS A LA CREACION DE NUESTRA BD PARA EL TPI FINAL DE PROGRAMACION 3. SE RECOMIENDA PRIMERO CREAR LA BASE DE DATOS, LUEGO
-- DARLE USE A ELLA. SIGUIENTE, EJECUTAR TODAS LAS TABLAS HASTA LA LEYENDA 'FUNCIONES DEL SISTEMA'. LUEGO DE HABER CREADO TODAS LAS 
-- TABLAS, EJECUTAR LOS STORE PROCEDURES Y VISTAS INDICADAS DEBAJO (RECOMENDADO EJECUTAR UNO POR UNO), Y POR ULTIMO EJECUTAR EL USUARIO ADMIN (UNICO EN EL SISTEMA).

USE master;
GO

CREATE DATABASE TPIcomercioBD
COLLATE Latin1_General_CS_AS;

USE TPIcomercioBD;
GO


-- ROL (Admin, Vendedor)
CREATE TABLE ROL (
    ID TINYINT IDENTITY(1,1) PRIMARY KEY,
    ROL VARCHAR(100) NOT NULL -- 1 ADMIN - 2 EMPLEADO - 3 CLIENTE
);
GO

INSERT INTO ROL (ROL) VALUES 
('ADMIN'),      
('EMPLEADO'),  
('CLIENTE');  
GO

-- MARCA
CREATE TABLE MARCA (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(100) NOT NULL UNIQUE,
    DESCRIPCION VARCHAR(250) NULL,
    ACTIVO BIT DEFAULT 1
);
GO

-- CATEGORIA
CREATE TABLE CATEGORIA (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(100) NOT NULL UNIQUE,
    DESCRIPCION VARCHAR(250) NULL,
    ACTIVO BIT DEFAULT 1
);
GO

-- TIPO_PAGO
CREATE TABLE TIPO_PAGO (
    ID TINYINT IDENTITY(1,1) PRIMARY KEY, 
    NOMBRE VARCHAR(100) NOT NULL UNIQUE, -- EFECTIVO , TRANSFERENCIA , TARJETA DE CREDITO, CHEQUE
    DESCRIPCION VARCHAR(250) NULL
);
GO

-- INSERCIÓN INMEDIATA DE TIPOS DE PAGO
INSERT INTO TIPO_PAGO (NOMBRE, DESCRIPCION) VALUES
('EFECTIVO', 'Pago en efectivo al momento de la entrega'),
('TRANSFERENCIA', 'Pago mediante transferencia bancaria'),
('CHEQUE', 'Pago mediante cheque nominativo'),
('DEBITO', 'Pago con tarjeta de débito'),
('CREDITO', 'Pago con tarjeta de crédito');
GO

-- =============================================
-- 2. PRODUCTO
-- =============================================

CREATE TABLE PRODUCTO (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(100) NOT NULL,
    PRECIO DECIMAL(10,2) CHECK (PRECIO > 0),
    DESCRIPCION_CORTA VARCHAR(250) NOT NULL,
	DESCRIPCION_EXTENDIDA VARCHAR(MAX) NOT NULL,
    STOCK INT NOT NULL DEFAULT 0 CHECK (STOCK >= 0),
    STOCK_MINIMO INT NOT NULL DEFAULT 0 CHECK (STOCK_MINIMO >= 0),
    ID_MARCA BIGINT NOT NULL,
    ID_CATEGORIA BIGINT NOT NULL,
    ACTIVO BIT DEFAULT 1,
    CONSTRAINT FK_PRODUCTO_MARCA FOREIGN KEY (ID_MARCA) REFERENCES MARCA(ID),
    CONSTRAINT FK_PRODUCTO_CATEGORIA FOREIGN KEY (ID_CATEGORIA) REFERENCES CATEGORIA(ID)
);
GO

-- =============================================
-- 2.1. IMAGENES DE UN PRODUCTO
-- =============================================
CREATE TABLE PRODUCTO_IMAGENES (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    ID_PRODUCTO BIGINT NOT NULL,
    URL_IMAGEN VARCHAR(500) NOT NULL,
    ES_PRINCIPAL BIT DEFAULT 0,
    ORDEN INT DEFAULT 0,
    CONSTRAINT FK_PRODUCTO_IMAGENES_PRODUCTO 
        FOREIGN KEY (ID_PRODUCTO) REFERENCES PRODUCTO(ID) 
        ON DELETE CASCADE
);
GO
CREATE INDEX IX_PRODUCTO_IMAGENES_PRODUCTO ON PRODUCTO_IMAGENES(ID_PRODUCTO);
GO
-- =============================================
-- 3. USUARIO 
-- =============================================

CREATE TABLE USUARIO (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    NICKNAME VARCHAR(100) NOT NULL UNIQUE,
    CONTRASENA VARCHAR(250) NOT NULL,
    EMAIL VARCHAR(250) NULL,
    ROLE_ID TINYINT NOT NULL,
    ACTIVO BIT DEFAULT 1,
    CONSTRAINT FK_USUARIO_ROL FOREIGN KEY (ROLE_ID) REFERENCES ROL(ID)
);
GO

-- =============================================
-- 4. CLIENTE
-- =============================================

CREATE TABLE CLIENTE (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(100) NOT NULL,
    APELLIDO VARCHAR(100) NOT NULL,
    TELEFONO VARCHAR(100) NULL,
    FECHA_REGISTRO DATE NOT NULL DEFAULT GETDATE(),
    ROLE_ID TINYINT NULL,
	ID_USUARIO BIGINT NULL UNIQUE,
    RAZON_SOCIAL VARCHAR(250) NULL,
	ACTIVO BIT DEFAULT 1,
    CONSTRAINT FK_CLIENTE_ROL FOREIGN KEY (ROLE_ID) REFERENCES ROL(ID),
	CONSTRAINT FK_CLIENTE_USUARIO FOREIGN KEY (ID_USUARIO) REFERENCES USUARIO(ID),
	CONSTRAINT CK_CLIENTE_ROL CHECK (ROLE_ID = 3 OR ROLE_ID IS NULL)
);
GO

-- =============================================
-- 5. EMPLEADO
-- =============================================

CREATE TABLE EMPLEADO (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(100) NOT NULL,
    APELLIDO VARCHAR(100) NOT NULL,
    TELEFONO VARCHAR(100) NULL,
    FECHAINGRESO DATE NOT NULL DEFAULT GETDATE(),
    SUELDO MONEY NOT NULL DEFAULT 0 CHECK (SUELDO >= 0),
    ID_USUARIO BIGINT NOT NULL UNIQUE,
	ACTIVO BIT DEFAULT 1,
    CONSTRAINT FK_EMPLEADO_USUARIO FOREIGN KEY (ID_USUARIO) REFERENCES USUARIO(ID)
);
GO

-- =============================================
-- 9. VENTA
-- =============================================

CREATE TABLE VENTA (
    ID BIGINT IDENTITY(1,1) PRIMARY KEY,
    FECHAVENTA DATE NOT NULL DEFAULT GETDATE(),
    MONTOTOTAL MONEY NOT NULL CHECK (MONTOTOTAL > 0),
    ID_TIPO_PAGO TINYINT NOT NULL,
    ID_CLIENTE BIGINT NOT NULL,
    NUM_FACTURA VARCHAR(30) NULL UNIQUE, -- Se genera en app o trigger
    CONSTRAINT FK_VENTA_TIPO_PAGO FOREIGN KEY (ID_TIPO_PAGO) REFERENCES TIPO_PAGO(ID),
    CONSTRAINT FK_VENTA_CLIENTE FOREIGN KEY (ID_CLIENTE) REFERENCES CLIENTE(ID),
);
GO

-- =============================================
-- 10. DETALLE_VENTA
-- =============================================

CREATE TABLE DETALLE_VENTA (
	ID_VENTA BIGINT NOT NULL,
    ID_PRODUCTO BIGINT NOT NULL,
    CANTIDAD SMALLINT NOT NULL CHECK (CANTIDAD > 0),
    PRECIO_UNITARIO MONEY NOT NULL CHECK (PRECIO_UNITARIO > 0),
    PRIMARY KEY (ID_VENTA, ID_PRODUCTO),  -- PK compuesta
    CONSTRAINT FK_DETALLE_VENTA_VENTA FOREIGN KEY (ID_VENTA) REFERENCES VENTA(ID) ON DELETE CASCADE,
    CONSTRAINT FK_DETALLE_VENTA_PRODUCTO FOREIGN KEY (ID_PRODUCTO) REFERENCES PRODUCTO(ID)
);
GO

-- =============================================
-- ESTADOS DE PAGO
-- =============================================
CREATE TABLE ESTADO_PAGO (
    ID TINYINT PRIMARY KEY,
    NOMBRE VARCHAR(30) NOT NULL UNIQUE
);
GO
INSERT INTO ESTADO_PAGO VALUES
(1, 'Pendiente'),
(2, 'Aprobado'),
(3, 'Rechazado'),
(4, 'Pendiente de comprobante'); -- Transferencia esperando comprobante
GO

-- =============================================
-- ESTADOS DE PREPARACIÓN (ARMADO)
-- =============================================
CREATE TABLE ESTADO_PREPARACION (
    ID TINYINT PRIMARY KEY,
    NOMBRE VARCHAR(30) NOT NULL UNIQUE
);
GO
INSERT INTO ESTADO_PREPARACION VALUES
(1, 'No iniciado'),
(2, 'En preparación'),
(3, 'Listo para envío'),
(4, 'Cancelado');
GO

-- =============================================
-- ESTADOS DE ENVÍO
-- =============================================
CREATE TABLE ESTADO_ENVIO (
    ID TINYINT PRIMARY KEY,
    NOMBRE VARCHAR(30) NOT NULL UNIQUE
);
GO
INSERT INTO ESTADO_ENVIO VALUES
(1, 'No iniciado'),
(2, 'En camino'),
(3, 'Entregado'),
(4, 'Devuelto'),
(5, 'Cancelado');
GO

-- =============================================
-- 11. MODIFICAR VENTA PARA OBTENCION DE ESTADOS
-- =============================================
ALTER TABLE VENTA ADD 
    ID_ESTADO_PAGO      TINYINT NOT NULL DEFAULT 1      CONSTRAINT FK_VENTA_ESTADO_PAGO      FOREIGN KEY REFERENCES ESTADO_PAGO(ID),
    ID_ESTADO_PREPARACION TINYINT NOT NULL DEFAULT 1   CONSTRAINT FK_VENTA_ESTADO_PREPARACION FOREIGN KEY REFERENCES ESTADO_PREPARACION(ID),
    ID_ESTADO_ENVIO     TINYINT NOT NULL DEFAULT 1      CONSTRAINT FK_VENTA_ESTADO_ENVIO     FOREIGN KEY REFERENCES ESTADO_ENVIO(ID);
GO

-- ===================================================================
-- ================	FUNCIONES DE SISTEMA	=========================
-- ===================================================================

--CREACION DE VISTA PARA LISTAR PRODUCTOS ACTIVOS
CREATE VIEW vw_ProductosActivos AS
SELECT 
    p.ID,
    p.NOMBRE,
    p.PRECIO,
    p.DESCRIPCION_CORTA,
    p.DESCRIPCION_EXTENDIDA,
    p.STOCK,
    p.STOCK_MINIMO,
	COALESCE(
		(SELECT TOP 1 URL_IMAGEN 
		 FROM PRODUCTO_IMAGENES 
		 WHERE ID_PRODUCTO = p.ID 
		 ORDER BY ES_PRINCIPAL DESC, ORDEN),
		'https://via.placeholder.com/400x300/cccccc/666666?text=Sin+Imagen'
	) AS ImagenPrincipal,

    m.ID AS IdMarca,
    m.NOMBRE AS NombreMarca,
    m.DESCRIPCION AS DescripcionMarca,
    
    c.ID AS IdCategoria,
    c.NOMBRE AS NombreCategoria,
    c.DESCRIPCION AS DescripcionCategoria,
    
    p.ACTIVO
FROM PRODUCTO p
INNER JOIN MARCA m ON m.ID = p.ID_MARCA
INNER JOIN CATEGORIA c ON c.ID = p.ID_CATEGORIA
WHERE p.ACTIVO = 1;
GO

-------------------------------------------------------------------------------
-- CREACION DE VISTA PARA CHEQUEAR SI EL USUARIO QUE SE LOGUEA TIENE UNA CUENTA

CREATE VIEW vw_LoginUsuario AS
SELECT 
    u.ID AS UsuarioId,
    u.NICKNAME,
	u.CONTRASENA AS Contrasena,
    u.EMAIL,
    u.ACTIVO,
    r.ID AS RolId,
    r.ROL AS RolNombre,
    
    -- Cliente
    c.ID AS ClienteId,
    c.NOMBRE AS ClienteNombre,
    c.APELLIDO AS ClienteApellido,
    c.TELEFONO AS ClienteTelefono,
    c.RAZON_SOCIAL AS ClienteRazonSocial,
    c.FECHA_REGISTRO AS ClienteFechaRegistro,
    
    -- Empleado
    e.ID AS EmpleadoId,
    e.NOMBRE AS EmpleadoNombre,
    e.APELLIDO AS EmpleadoApellido,
    e.TELEFONO AS EmpleadoTelefono,
    e.SUELDO AS EmpleadoSueldo

FROM USUARIO u
INNER JOIN ROL r ON u.ROLE_ID = r.ID
LEFT JOIN CLIENTE c ON u.ID = c.ID_USUARIO AND (c.ACTIVO = 1 OR c.ACTIVO IS NULL)
LEFT JOIN EMPLEADO e ON u.ID = e.ID_USUARIO AND (e.ACTIVO = 1 OR e.ACTIVO IS NULL)
WHERE u.ACTIVO = 1
GO
----------------------------------------------------------

-- SP PARA LA CREACION DE USUARIOS
CREATE PROCEDURE sp_AgregarUsuario
    @Nickname NVARCHAR(50),
    @Contrasena NVARCHAR(255),
    @Email NVARCHAR(100),
    @RolId INT,
    @Activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoUsuarioId BIGINT;

    BEGIN TRY
        BEGIN TRANSACTION;

		-- VALIDACIÓN ESPECIAL PARA ADMIN (RolId = 1)
        IF @RolId = 1
        BEGIN
            IF EXISTS (SELECT 1 FROM USUARIO WHERE ROLE_ID = 1 AND ACTIVO = 1)
                THROW 50004, 'Ya existe un usuario administrador. Solo se permite uno.', 1;
        END

        -- Validar nickname único
        IF EXISTS (SELECT 1 FROM USUARIO WHERE NICKNAME = @Nickname)
            THROW 50001, 'El nickname ya existe', 1;
       
        -- Validar email único
        IF EXISTS (SELECT 1 FROM USUARIO WHERE EMAIL = @Email)
            THROW 50002, 'El email ya está registrado', 1;

        -- Insertar en USUARIO
        INSERT INTO USUARIO (NICKNAME, CONTRASENA, EMAIL, ROLE_ID, ACTIVO)
        VALUES (@Nickname, @Contrasena, @Email, @RolId, @Activo);

        -- Capturar ID del usuario recién creado
        SET @NuevoUsuarioId = SCOPE_IDENTITY();

        -- Insertar en tabla correspondiente según rol
        IF @RolId = 2 -- EMPLEADO
        BEGIN
            INSERT INTO EMPLEADO (NOMBRE, APELLIDO, TELEFONO, FECHAINGRESO, SUELDO, ID_USUARIO, ACTIVO)
            VALUES (@Nickname, '', '', GETDATE(), 700, @NuevoUsuarioId, 1);
        END
        ELSE IF @RolId = 3 -- CLIENTE
        BEGIN
            INSERT INTO CLIENTE (NOMBRE, APELLIDO, TELEFONO, FECHA_REGISTRO, ROLE_ID, ID_USUARIO, RAZON_SOCIAL, ACTIVO)
            VALUES (@Nickname, '', '', GETDATE(), 3, @NuevoUsuarioId, '', 1);
        END

        COMMIT TRANSACTION;

        -- Devolver el ID del usuario
        SELECT @NuevoUsuarioId AS NuevoId;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
----------------------------------------------------------

-- SP PARA LA MODIFICACION DE USUARIOS
CREATE PROCEDURE sp_ModificarUsuario
    @Id BIGINT,
    @Nickname NVARCHAR(50),
    @Email NVARCHAR(100),
    @RolId INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @RolActual INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Obtener rol actual
        SELECT @RolActual = ROLE_ID 
        FROM USUARIO 
        WHERE ID = @Id;

        -- REGLA 1: Admin NO puede cambiar de rol
        IF @RolActual = 1 AND @RolId != 1
            THROW 50006, 'El administrador no puede cambiar de rol.', 1;

        -- Validar nickname único
        IF EXISTS (SELECT 1 FROM USUARIO WHERE NICKNAME = @Nickname AND ID != @Id)
            THROW 50001, 'El nickname ya existe', 1;

        -- Validar email único
        IF EXISTS (SELECT 1 FROM USUARIO WHERE EMAIL = @Email AND ID != @Id)
            THROW 50002, 'El email ya está registrado', 1;

        IF @RolActual IN (2, 3) AND @RolId IN (2, 3) AND @RolActual != @RolId
        BEGIN
            IF @RolActual = 2 AND @RolId = 3
            BEGIN
                -- EMPLEADO → CLIENTE
                INSERT INTO CLIENTE (NOMBRE, APELLIDO, TELEFONO, FECHA_REGISTRO, ROLE_ID, ID_USUARIO, RAZON_SOCIAL, ACTIVO)
                SELECT NOMBRE, APELLIDO, TELEFONO, GETDATE(), 3, ID_USUARIO, '', 1
                FROM EMPLEADO 
                WHERE ID_USUARIO = @Id;

                DELETE FROM EMPLEADO WHERE ID_USUARIO = @Id;
            END
            ELSE IF @RolActual = 3 AND @RolId = 2
            BEGIN
                -- CLIENTE → EMPLEADO
                INSERT INTO EMPLEADO (NOMBRE, APELLIDO, TELEFONO, FECHAINGRESO, SUELDO, ID_USUARIO, ACTIVO)
                SELECT NOMBRE, APELLIDO, TELEFONO, GETDATE(), 700, ID_USUARIO, 1
                FROM CLIENTE 
                WHERE ID_USUARIO = @Id;

                DELETE FROM CLIENTE WHERE ID_USUARIO = @Id;
            END
        END

        -- Actualizar USUARIO
        UPDATE USUARIO SET
            NICKNAME = @Nickname,
            EMAIL = @Email,
            ROLE_ID = @RolId
        WHERE ID = @Id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
--------------------------------------------------------------
-- SP PARA LA BAJA DE USUARIOS
CREATE PROCEDURE sp_bajaUsuario
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @RolId INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Obtener rol del usuario
        SELECT @RolId = ROLE_ID 
        FROM USUARIO 
        WHERE ID = @Id AND ACTIVO = 1;

        -- REGLA: No se puede eliminar al Admin
        IF @RolId = 1
            THROW 50008, 'No se puede eliminar al administrador.', 1;

        -- Soft-delete en USUARIO
        UPDATE USUARIO SET ACTIVO = 0 WHERE ID = @Id;

        -- Soft-delete en tabla correspondiente
        IF @RolId = 2
        BEGIN
            UPDATE EMPLEADO SET ACTIVO = 0 WHERE ID_USUARIO = @Id;
        END
        ELSE IF @RolId = 3
        BEGIN
            UPDATE CLIENTE SET ACTIVO = 0 WHERE ID_USUARIO = @Id;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
---------------------------------------------------------------
-- SP PARA REGISTRAR UN CLIENTE

CREATE PROCEDURE sp_RegistrarCliente
    @Nickname     NVARCHAR(100),
    @Contrasena   NVARCHAR(250),
    @Email        NVARCHAR(250),
    @Nombre       VARCHAR(100),
    @Apellido     VARCHAR(100),
    @Telefono     VARCHAR(100) = NULL,
    @EsEmpresa    BIT = 0,
    @RazonSocial  VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @RolCliente TINYINT = 3;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validaciones
        IF EXISTS (SELECT 1 FROM USUARIO WHERE NICKNAME = @Nickname)
            THROW 50001, 'El nombre de usuario ya existe', 1;

        IF @Email IS NOT NULL AND LTRIM(RTRIM(@Email)) <> ''
            IF EXISTS (SELECT 1 FROM USUARIO WHERE EMAIL = @Email)
                THROW 50002, 'El email ya está registrado', 1;

        -- Insertar en USUARIO
        INSERT INTO USUARIO (NICKNAME, CONTRASENA, EMAIL, ROLE_ID, ACTIVO)
        VALUES (@Nickname, @Contrasena, @Email, @RolCliente, 1);

        DECLARE @NuevoUsuarioId BIGINT = SCOPE_IDENTITY();

        -- Insertar en CLIENTE
        INSERT INTO CLIENTE (NOMBRE, APELLIDO, TELEFONO, FECHA_REGISTRO, ROLE_ID, ID_USUARIO, RAZON_SOCIAL, ACTIVO)
        VALUES (@Nombre, @Apellido, @Telefono, GETDATE(), @RolCliente, @NuevoUsuarioId,
               CASE WHEN @EsEmpresa = 1 THEN @RazonSocial ELSE NULL END, 1);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

---------------------------------------------------------------
-- SP PARA restar el stock en una venta

CREATE OR ALTER PROCEDURE SP_ActualizarStockPorVenta
    @IdVenta BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1 Validar que exista la venta
        IF NOT EXISTS (SELECT 1 FROM VENTA WHERE ID = @IdVenta)
            THROW 51000, 'La venta no existe.', 1;

        -- 2 Recorrer cada item vendido y actualizar el stock
        UPDATE p
        SET p.STOCK = p.STOCK - dv.CANTIDAD
        FROM PRODUCTO p
        INNER JOIN DETALLE_VENTA dv ON dv.ID_PRODUCTO = p.ID
        WHERE dv.ID_VENTA = @IdVenta;

        -- 3 Validar que ningun producto quede con stock negativo
        IF EXISTS (SELECT 1 FROM PRODUCTO WHERE STOCK < 0)
            THROW 51001, 'Stock insuficiente para uno o más productos.', 1;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO










