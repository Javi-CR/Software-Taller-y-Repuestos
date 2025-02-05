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
    FROM 
        dbo.Usuarios U
    WHERE 
        U.UsuarioID = @UsuarioID;
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
    -- Verificar si el usuario existe
    IF EXISTS (SELECT 1 FROM dbo.Usuarios WHERE UsuarioID = @UsuarioId)
    BEGIN
        -- Actualizar el perfil del usuario
        UPDATE dbo.Usuarios
        SET Nombre = @Nombre,
            Apellidos = @Apellidos,
            Telefono = @Telefono,
            Direccion = @Direccion,
            Imagen = CASE 
                        WHEN @Imagen IS NOT NULL AND @Imagen != '' THEN @Imagen
                        ELSE Imagen 
                     END
        WHERE UsuarioID = @UsuarioId;
    END
    ELSE
    BEGIN
        -- Mensaje en caso de que no exista el usuario
        RAISERROR('El usuario especificado no existe.', 16, 1);
    END
END
GO




CREATE PROCEDURE CrearUsuario
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
    IF EXISTS (SELECT 1 FROM Usuarios WHERE Correo = @Correo)
    BEGIN
        THROW 50001, 'El correo ya está en uso. Por favor, elija otro.', 1;
    END

    -- Insertar el nuevo usuario
    INSERT INTO Usuarios (Nombre, Apellidos, Correo, Contrasenna, RolID, FechaIngreso, Estado)
    VALUES (@Nombre, @Apellidos, @Correo, @Contrasenna, @RolID, GETDATE(), @Estado);
    
    -- Confirmación de éxito
    SELECT SCOPE_IDENTITY() AS UsuarioId;
END;


CREATE PROCEDURE IniciarSesion
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
        U.Imagen
    FROM Usuarios U
    INNER JOIN Roles R ON U.RolID = R.RolId  -- Hacer el JOIN con la tabla Roles
    WHERE U.Correo = @Correo;
END;




CREATE PROCEDURE RegistrarUsuarioGoogle
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
    IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Correo = @Correo)
    BEGIN
        INSERT INTO Usuarios (Nombre, Apellidos, Correo, Imagen, RolID, Estado)
        VALUES (@Nombre, @Apellidos, @Correo, @Imagen, @RolID, @Estado);
    END
END





INSERT INTO [TallerRepuestosDB].[dbo].[Usuarios] 
    ([Nombre], [Correo], [Contrasenna], [Telefono], [Direccion], [RolID], [FechaIngreso], [SalarioBase], [Imagen], [Apellidos])
VALUES 
    ('Administrador', 'admin@taller.com', '$2a$11$2RSevFCMI5xO2TfltJQzseFbj4DR/NFFGJAQedoPkusJcNaEqxqWK', NULL, NULL, 1, GETDATE(), NULL, NULL, 'Admin');




CREATE PROCEDURE ObtenerFacturaPorUsuario
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
    FROM 
        TallerRepuestosDB.dbo.Facturas f
    INNER JOIN 
        TallerRepuestosDB.dbo.DetalleFacturas df
        ON f.FacturaId = df.FacturaId
    INNER JOIN 
        TallerRepuestosDB.dbo.Productos p
        ON df.ProductoId = p.ProductoId
    WHERE 
        f.UsuarioId = @UsuarioId;
END;




----------------------------------------------------
CREATE PROCEDURE ObtenerTodasLasFacturas
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
    FROM 
        TallerRepuestosDB.dbo.Facturas f
    INNER JOIN 
        TallerRepuestosDB.dbo.DetalleFacturas df
        ON f.FacturaId = df.FacturaId
    INNER JOIN 
        TallerRepuestosDB.dbo.Productos p
        ON df.ProductoId = p.ProductoId;
END;



CREATE PROCEDURE ActualizarEstadoFactura
    @FacturaId INT,
    @NuevoEstado NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE df
    SET df.EstadoPago = @NuevoEstado
    FROM TallerRepuestosDB.dbo.DetalleFacturas df
    WHERE df.FacturaId = @FacturaId;
END;