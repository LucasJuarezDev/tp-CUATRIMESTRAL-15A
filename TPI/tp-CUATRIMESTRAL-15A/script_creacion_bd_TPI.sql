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
	IMAGEN_URL VARCHAR(500) NULL,
    STOCK_MINIMO INT NOT NULL DEFAULT 0 CHECK (STOCK_MINIMO >= 0),
    ID_MARCA BIGINT NOT NULL,
    ID_CATEGORIA BIGINT NOT NULL,
    ACTIVO BIT DEFAULT 1,
    CONSTRAINT FK_PRODUCTO_MARCA FOREIGN KEY (ID_MARCA) REFERENCES MARCA(ID),
    CONSTRAINT FK_PRODUCTO_CATEGORIA FOREIGN KEY (ID_CATEGORIA) REFERENCES CATEGORIA(ID)
);
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


-- ===================================================================
-- ================	FUNCIONES DE SISTEMA	=========================
-- ===================================================================

--CREACION DE VISTA PARA LISTAR PRODUCTOS ACTIVOS
CREATE VIEW vw_ProductosActivos AS
SELECT
    prod.ID,
    prod.NOMBRE,
    prod.PRECIO,
	prod.DESCRIPCION_CORTA,
    prod.DESCRIPCION_EXTENDIDA,
    prod.STOCK,
    prod.STOCK_MINIMO,
	prod.IMAGEN_URL,
    marc.ID AS IdMarca,
    marc.NOMBRE AS NombreMarca,
    marc.DESCRIPCION AS DescripcionMarca,
    cat.ID AS IdCategoria,
    cat.NOMBRE AS NombreCategoria,
    cat.DESCRIPCION AS DescripcionCategoria,
    prod.ACTIVO
	FROM PRODUCTO prod
	INNER JOIN MARCA marc ON marc.ID = prod.ID_MARCA
	INNER JOIN CATEGORIA cat ON cat.ID = prod.ID_CATEGORIA
	WHERE prod.ACTIVO = 1;

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

