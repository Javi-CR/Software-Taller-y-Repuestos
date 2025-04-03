

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
    SELECT 
        df.DetalleFacturaId,
        f.FacturaId,
        df.ProductoId,
        df.Cantidad,
        df.PrecioUnitario,
        df.EstadoPago,
        df.ImagenFactura,
        f.FechaCompra,
        f.Total,
        u.Correo AS CorreoUsuario,
        p.Nombre AS NombreProducto
    FROM 
        DetalleFacturas df
    INNER JOIN 
        Facturas f ON df.FacturaId = f.FacturaId
    INNER JOIN 
        Usuarios u ON f.UsuarioId = u.UsuarioId
    LEFT JOIN 
        Productos p ON df.ProductoId = p.ProductoId
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


INSERT INTO [TallerRepuestosDB].[dbo].[Categorias] ([Nombre], [Descripcion])
VALUES
('Lubricantes', 'Aceites y lubricantes para vehículos y maquinaria, incluyendo aceites de motor y otros sistemas.'),
('Filtros', 'Filtros de aire, aceite, combustible y habitáculo para vehículos.'),
('Frenos', 'Pastillas, discos y componentes del sistema de frenos de vehículos.'),
('Encendido', 'Componentes del sistema de encendido, como bujías y bobinas.'),
('Accesorios Eléctricos', 'Componentes eléctricos como cables, fusibles, alternadores, y otros accesorios eléctricos.'),
('Suspensión', 'Amortiguadores, resortes, y otros componentes para sistemas de suspensión de vehículos.'),
('Motor', 'Repuestos y componentes para motores de vehículos, incluyendo piezas principales y accesorios del motor.'),
('Neumáticos', 'Neumáticos y llantas de diferentes tamaños y modelos.'),
('Baterías', 'Baterías para vehículos y maquinaria, incluyendo baterías de arranque.'),
('Herramientas', 'Herramientas manuales y eléctricas para reparaciones y mantenimiento de vehículos.'),
('Repuestos de Carrocería', 'Piezas de carrocería como espejos, defensas, parachoques y paneles.'),
('Radiadores y Enfriamiento', 'Radiadores, mangueras y componentes del sistema de enfriamiento de vehículos.'),
('Transmisión', 'Componentes del sistema de transmisión, incluyendo embragues y cajas de cambios.'),
('Sistemas de Dirección', 'Componentes del sistema de dirección, como bombas de dirección y barras de dirección.'),
('Iluminación', 'Luces y focos para vehículos y maquinaria industrial, incluyendo faros, bombillos y luces de señalización.'),
('Climatización', 'Componentes del sistema de aire acondicionado y calefacción para vehículos.'),
('Escapes y Catalizadores', 'Sistemas de escape, tubos de escape y catalizadores para vehículos.'),
('Accesorios para Automóviles', 'Accesorios varios como alfombrillas, asientos, tapicería y otros artículos para automóviles.'),
('Plásticos y Molduras', 'Piezas plásticas para interiores y exteriores de vehículos.'),
('Cinturones de Seguridad', 'Cinturones de seguridad y componentes del sistema de seguridad vehicular.'),
('Bujías y Componentes de Encendido', 'Bujías, bobinas y otros componentes relacionados con el sistema de encendido de vehículos.'),
('Repuestos para Sistemas de Suspensión', 'Componentes para suspensión y dirección, como amortiguadores y resortes.'),
('Repuestos para Vehículos Comerciales', 'Repuestos y componentes para camiones, autobuses y vehículos comerciales.'),
('Aceites y Fluidos', 'Aceites para motor, fluidos de frenos, de transmisión, y otros líquidos automotrices.'),
('Repuestos de Transmisión Automática', 'Componentes del sistema de transmisión automática, como fluidos y partes internas.'),
('Sistemas de Frenos Hidráulicos', 'Componentes específicos para sistemas de frenos hidráulicos de vehículos.'),
('Filtros de Combustible', 'Filtros específicos para el sistema de combustible de vehículos.'),
('Componentes Eléctricos y Electrónicos', 'Componentes como alternadores, motores eléctricos, y sensores electrónicos de vehículos.'),
('Cables y Conectores', 'Cables, conectores y otros componentes para el sistema eléctrico de vehículos.');
GO
