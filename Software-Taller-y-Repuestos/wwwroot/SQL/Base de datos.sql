-- Crear la base de datos
CREATE DATABASE GestionRepuestosDB;
GO

-- Usar la base de datos creada
USE GestionRepuestosDB;
GO

-- Tabla de Categorías
CREATE TABLE Categorias (
    CategoriaID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255)
);
GO

-- Tabla de Productos
CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Codigo NVARCHAR(50) NOT NULL,
    CategoriaID INT NOT NULL,
    Descripcion NVARCHAR(255),
    Cantidad INT NOT NULL,
    PrecioCompra DECIMAL(10, 2) NOT NULL,
    PrecioVenta DECIMAL(10, 2) NOT NULL,
    Imagen NVARCHAR(MAX),
    FOREIGN KEY (CategoriaID) REFERENCES Categorias(CategoriaID)
);
GO

-- Tabla de Modelos de Autos
CREATE TABLE ModelosAutos (
    ModeloID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Año INT NOT NULL
);
GO

-- Tabla de Asociación Producto-Modelo (Productos asociados a varios modelos de autos)
CREATE TABLE ProductosModelos (
    ProductoID INT NOT NULL,
    ModeloID INT NOT NULL,
    PRIMARY KEY (ProductoID, ModeloID),
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID),
    FOREIGN KEY (ModeloID) REFERENCES ModelosAutos(ModeloID)
);
GO

-- Tabla de Clientes
CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100),
    Telefono NVARCHAR(50)
);
GO

-- Tabla de Facturas
CREATE TABLE Facturas (
    FacturaID INT PRIMARY KEY IDENTITY,
    ClienteID INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(10, 2) NOT NULL,
    Impuestos DECIMAL(10, 2),
    Descuento DECIMAL(10, 2),
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);
GO

-- Tabla de Detalles de Factura (Productos incluidos en las facturas)
CREATE TABLE DetallesFactura (
    DetalleID INT PRIMARY KEY IDENTITY,
    FacturaID INT NOT NULL,
    ProductoID INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (FacturaID) REFERENCES Facturas(FacturaID),
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);
GO

/*
-- Tabla de Mensajes de Interacción (WhatsApp/Chat)
CREATE TABLE MensajesInteraccion (
    MensajeID INT PRIMARY KEY IDENTITY,
    ClienteID INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Mensaje NVARCHAR(MAX) NOT NULL,
    Tipo NVARCHAR(50), -- Ej: WhatsApp, Chat
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);
GO
*/

-- Tabla de Empleados
CREATE TABLE Empleados (
    EmpleadoID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100),
    Telefono NVARCHAR(50),
    Direccion NVARCHAR(255),
    Puesto NVARCHAR(100),
    FechaIngreso DATETIME NOT NULL,
    SalarioBase DECIMAL(10, 2) NOT NULL
);
GO

-- Tabla de Pagos de Empleados
CREATE TABLE PagosEmpleados (
    PagoID INT PRIMARY KEY IDENTITY,
    EmpleadoID INT NOT NULL,
    FechaPago DATETIME NOT NULL DEFAULT GETDATE(),
    SalarioPagado DECIMAL(10, 2) NOT NULL,
    HorasExtras DECIMAL(5, 2),
    Deducciones DECIMAL(10, 2),
    Bonificaciones DECIMAL(10, 2),
    FOREIGN KEY (EmpleadoID) REFERENCES Empleados(EmpleadoID)
);
GO
/*
-- Tabla de Bancos
CREATE TABLE Bancos (
    BancoID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    NumeroCuenta NVARCHAR(50) NOT NULL
);
GO

-- Tabla de Integración de Pagos con Bancos
CREATE TABLE IntegracionBancos (
    IntegracionID INT PRIMARY KEY IDENTITY,
    EmpleadoID INT NOT NULL,
    BancoID INT NOT NULL,
    DetallePago NVARCHAR(255),
    FechaIntegracion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (EmpleadoID) REFERENCES Empleados(EmpleadoID),
    FOREIGN KEY (BancoID) REFERENCES Bancos(BancoID)
);
GO
*/