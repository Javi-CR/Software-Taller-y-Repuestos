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

