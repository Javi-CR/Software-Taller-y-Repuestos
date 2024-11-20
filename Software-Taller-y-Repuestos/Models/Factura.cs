
namespace Software_Taller_y_Repuestos.Models
{
    public class Factura
    {
        public int FacturaId { get; set; }
        public string? UsuarioId { get; set; } // Asociado al usuario autenticado
        public DateTime FechaCompra { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }

        public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; }
        public List<Producto> Productos { get; internal set; }
    }

}
