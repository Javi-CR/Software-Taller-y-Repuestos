namespace Software_Taller_y_Repuestos.Models
{
    public class UsuarioFactura
    {

        public int Cantidad { get; set; }
        public string EstadoPago { get; set; }
        public string NombreProducto { get; set; }

        public int FacturaId { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal IVA { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

    }
}
