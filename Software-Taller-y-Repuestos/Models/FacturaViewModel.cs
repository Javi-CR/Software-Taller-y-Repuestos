namespace Software_Taller_y_Repuestos.Models
{
    public class FacturaViewModel
    {
        public int FacturaId { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public string EstadoPago { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Total { get; set; }
        public string CorreoUsuario { get; set; }
        public string ImagenFactura { get; set; } // Agregado el campo de imagen
    }


}
