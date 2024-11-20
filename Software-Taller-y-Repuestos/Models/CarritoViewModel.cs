namespace Software_Taller_y_Repuestos.Models
{
    public class CarritoViewModel
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }

        // Propiedad para calcular el subtotal
        public decimal Subtotal
        {
            get
            {
                if (Producto != null)
                {
                    return Producto.PrecioVenta * Cantidad; // Subtotal del producto
                }
                return 0; // Si Producto es null, retornar 0
            }
        }
    }

}
