namespace Software_Taller_y_Repuestos.Models
{
    public class DetalleFactura
    {
        public int DetalleFacturaId { get; set; }
        public int FacturaId { get; set; } // FK hacia Factura
        public int ProductoId { get; set; } // FK hacia Producto
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public virtual Factura Factura { get; set; }
        public virtual Producto Producto { get; set; }
    }

}
