namespace Software_Taller_y_Repuestos.Models
{
    public class DetalleFactura
    {
        public int DetalleFacturaId { get; set; }
        public int FacturaId { get; set; } // FK hacia Factura
        public int ProductoId { get; set; } // FK hacia Producto
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Nuevos campos
        public string ImagenFactura { get; set; } // Ruta de la imagen de la factura
        public string EstadoPago { get; set; } = "Pendiente"; // Estado del pago por defecto

        // Relaciones de navegación
        public virtual Factura Factura { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
