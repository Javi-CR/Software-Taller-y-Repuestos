

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

        -- Si la actualizaci�n se realiz�, establecer @Result como 1
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
        THROW 50001, 'El correo ya est� en uso. Por favor, elija otro.', 1;
    END

    -- Insertar el nuevo usuario
    INSERT INTO dbo.Usuarios (Nombre, Apellidos, Correo, Contrasenna, RolID, FechaIngreso, Estado)
    VALUES (@Nombre, @Apellidos, @Correo, @Contrasenna, @RolID, GETDATE(), @Estado);
    
    -- Confirmaci�n de �xito
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
('Lubricantes', 'Aceites y lubricantes para veh�culos y maquinaria, incluyendo aceites de motor y otros sistemas.'),
('Filtros', 'Filtros de aire, aceite, combustible y habit�culo para veh�culos.'),
('Frenos', 'Pastillas, discos y componentes del sistema de frenos de veh�culos.'),
('Encendido', 'Componentes del sistema de encendido, como buj�as y bobinas.'),
('Accesorios El�ctricos', 'Componentes el�ctricos como cables, fusibles, alternadores, y otros accesorios el�ctricos.'),
('Suspensi�n', 'Amortiguadores, resortes, y otros componentes para sistemas de suspensi�n de veh�culos.'),
('Motor', 'Repuestos y componentes para motores de veh�culos, incluyendo piezas principales y accesorios del motor.'),
('Neum�ticos', 'Neum�ticos y llantas de diferentes tama�os y modelos.'),
('Bater�as', 'Bater�as para veh�culos y maquinaria, incluyendo bater�as de arranque.'),
('Herramientas', 'Herramientas manuales y el�ctricas para reparaciones y mantenimiento de veh�culos.'),
('Repuestos de Carrocer�a', 'Piezas de carrocer�a como espejos, defensas, parachoques y paneles.'),
('Radiadores y Enfriamiento', 'Radiadores, mangueras y componentes del sistema de enfriamiento de veh�culos.'),
('Transmisi�n', 'Componentes del sistema de transmisi�n, incluyendo embragues y cajas de cambios.'),
('Sistemas de Direcci�n', 'Componentes del sistema de direcci�n, como bombas de direcci�n y barras de direcci�n.'),
('Iluminaci�n', 'Luces y focos para veh�culos y maquinaria industrial, incluyendo faros, bombillos y luces de se�alizaci�n.'),
('Climatizaci�n', 'Componentes del sistema de aire acondicionado y calefacci�n para veh�culos.'),
('Escapes y Catalizadores', 'Sistemas de escape, tubos de escape y catalizadores para veh�culos.'),
('Accesorios para Autom�viles', 'Accesorios varios como alfombrillas, asientos, tapicer�a y otros art�culos para autom�viles.'),
('Pl�sticos y Molduras', 'Piezas pl�sticas para interiores y exteriores de veh�culos.'),
('Cinturones de Seguridad', 'Cinturones de seguridad y componentes del sistema de seguridad vehicular.'),
('Buj�as y Componentes de Encendido', 'Buj�as, bobinas y otros componentes relacionados con el sistema de encendido de veh�culos.'),
('Repuestos para Sistemas de Suspensi�n', 'Componentes para suspensi�n y direcci�n, como amortiguadores y resortes.'),
('Repuestos para Veh�culos Comerciales', 'Repuestos y componentes para camiones, autobuses y veh�culos comerciales.'),
('Aceites y Fluidos', 'Aceites para motor, fluidos de frenos, de transmisi�n, y otros l�quidos automotrices.'),
('Repuestos de Transmisi�n Autom�tica', 'Componentes del sistema de transmisi�n autom�tica, como fluidos y partes internas.'),
('Sistemas de Frenos Hidr�ulicos', 'Componentes espec�ficos para sistemas de frenos hidr�ulicos de veh�culos.'),
('Filtros de Combustible', 'Filtros espec�ficos para el sistema de combustible de veh�culos.'),
('Componentes El�ctricos y Electr�nicos', 'Componentes como alternadores, motores el�ctricos, y sensores electr�nicos de veh�culos.'),
('Cables y Conectores', 'Cables, conectores y otros componentes para el sistema el�ctrico de veh�culos.');
GO
