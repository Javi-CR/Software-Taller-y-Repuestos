namespace Software_Taller_y_Repuestos.Models
{
    public class CarritoItem
    {
        public int CarritoItemId { get; set; }
        public int CarritoId { get; set; } // FK hacia Carrito
        public int ProductoId { get; set; } // FK hacia Producto
        public int Cantidad { get; set; }

        public virtual Carrito Carrito { get; set; }
        public virtual Producto Producto { get; set; }
    }

}
