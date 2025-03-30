USE [master]
GO

USE [TallerRepuestosDB]
GO

CREATE PROCEDURE [dbo].[ConsultarPerfilUsuario]
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.Nombre,
        U.Apellidos,
        U.Correo,
        U.Telefono,
        U.Direccion,
        U.Imagen
    FROM dbo.Usuarios U
    WHERE U.UsuarioID = @UsuarioID;
END
GO

CREATE PROCEDURE [dbo].[ActualizarPerfilUsuario]
    @UsuarioId      INT,
    @Nombre         NVARCHAR(100),
    @Apellidos      NVARCHAR(MAX),
    @Telefono       NVARCHAR(50),
    @Direccion      NVARCHAR(255),
    @Imagen         NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Result INT;

    -- Inicializar la variable @Result
    SET @Result = 0;

    -- Verificar si el usuario existe
    IF EXISTS (SELECT 1 FROM dbo.Usuarios WHERE UsuarioID = @UsuarioId)
    BEGIN
        -- Actualizar el perfil del usuario
        UPDATE dbo.Usuarios
        SET Nombre    = @Nombre,
            Apellidos = @Apellidos,
            Telefono  = @Telefono,
            Direccion = @Direccion,
            Imagen    = CASE 
                            WHEN @Imagen IS NOT NULL AND @Imagen != '' THEN @Imagen
                            ELSE Imagen 
                        END
        WHERE UsuarioID = @UsuarioId;

        -- Si la actualización se realizó, establecer @Result como 1
        SET @Result = 1;
    END
    ELSE
    BEGIN
        -- Mensaje en caso de que no exista el usuario
        RAISERROR('El usuario especificado no existe.', 16, 1);
    END

    -- Devolver el valor de @Result
    RETURN @Result;
END
GO


CREATE PROCEDURE [dbo].[CrearUsuario]
    @Nombre NVARCHAR(50),
    @Apellidos NVARCHAR(50),
    @Correo NVARCHAR(100),
    @Contrasenna NVARCHAR(100),
    @RolID INT,
    @Estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si ya existe un usuario con el mismo correo
    IF EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Correo = @Correo)
    BEGIN
        THROW 50001, 'El correo ya está en uso. Por favor, elija otro.', 1;
    END

    -- Insertar el nuevo usuario
    INSERT INTO dbo.Usuarios (Nombre, Apellidos, Correo, Contrasenna, RolID, FechaIngreso, Estado)
    VALUES (@Nombre, @Apellidos, @Correo, @Contrasenna, @RolID, GETDATE(), @Estado);
    
    -- Confirmación de éxito
    SELECT SCOPE_IDENTITY() AS UsuarioId;
END
GO

CREATE PROCEDURE [dbo].[IniciarSesion]
    @Correo NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.UsuarioId,
        U.Nombre,
        U.Apellidos,
        U.Correo,
        U.Contrasenna,
        U.Telefono,
        U.Direccion,
        U.RolID,
        R.NombreRol,  -- Incluir el nombre del rol
        U.FechaIngreso,
        U.SalarioBase,
        U.Imagen,
        U.Estado  -- Agregar el estado del usuario
    FROM dbo.Usuarios U
    INNER JOIN dbo.Roles R ON U.RolID = R.RolId
    WHERE U.Correo = @Correo;
END
GO


CREATE PROCEDURE [dbo].[RegistrarUsuarioGoogle]
    @Nombre NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @Correo NVARCHAR(100),
    @Imagen NVARCHAR(MAX),
    @RolID INT,
    @Estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    -- Solo insertar si el usuario no existe
    IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Correo = @Correo)
    BEGIN
        INSERT INTO dbo.Usuarios (Nombre, Apellidos, Correo, Imagen, RolID, Estado)
        VALUES (@Nombre, @Apellidos, @Correo, @Imagen, @RolID, @Estado);
    END
END
GO

INSERT INTO dbo.Usuarios 
    ([Nombre], [Correo], [Contrasenna], [Telefono], [Direccion], [RolID], [FechaIngreso], [SalarioBase], [Imagen], [Apellidos], [Estado])
VALUES 
    ('Administrador', 'admin@taller.com', '$2a$11$2RSevFCMI5xO2TfltJQzseFbj4DR/NFFGJAQedoPkusJcNaEqxqWK', 
    NULL, NULL, 1, GETDATE(), NULL, NULL, 'Admin', 1);
GO

CREATE PROCEDURE [dbo].[ObtenerFacturaPorUsuario]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        f.FacturaId,
        f.FechaCompra,
        f.IVA,
        f.Subtotal,  
        f.Total,    
        df.Cantidad,
        df.EstadoPago,
        p.Nombre AS NombreProducto
    FROM dbo.Facturas f
    INNER JOIN dbo.DetalleFacturas df ON f.FacturaId = df.FacturaId
    INNER JOIN dbo.Productos p ON df.ProductoId = p.ProductoId
    WHERE f.UsuarioId = @UsuarioId;
END
GO

CREATE PROCEDURE [dbo].[ObtenerTodasLasFacturas]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        f.FacturaId,
        f.FechaCompra,
        f.IVA,
        f.Subtotal,  
        f.Total,    
        df.Cantidad,
        df.EstadoPago,
        p.Nombre AS NombreProducto
    FROM dbo.Facturas f
    INNER JOIN dbo.DetalleFacturas df ON f.FacturaId = df.FacturaId
    INNER JOIN dbo.Productos p ON df.ProductoId = p.ProductoId;
END
GO

CREATE PROCEDURE [dbo].[ActualizarEstadoFactura]
    @FacturaId INT,
    @NuevoEstado NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE df
    SET df.EstadoPago = @NuevoEstado
    FROM dbo.DetalleFacturas df
    WHERE df.FacturaId = @FacturaId;
END
GO
