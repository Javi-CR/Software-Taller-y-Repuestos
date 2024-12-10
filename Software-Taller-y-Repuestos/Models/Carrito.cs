namespace Software_Taller_y_Repuestos.Models
{
    public class Carrito
    {
        public int CarritoId { get; set; }
        public string UsuarioId { get; set; } // Asociado al usuario autenticado
        public virtual ICollection<CarritoItem> CarritoItems { get; set; }
        public string ReciboPath { get; set; } // Ruta para almacenar el recibo
    }
}