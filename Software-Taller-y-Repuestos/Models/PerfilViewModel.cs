namespace Software_Taller_y_Repuestos.Models
{
    public class PerfilViewModel
    {
        public Usuario Usuario { get; set; } // Modelo principal del usuario
        public List<UsuarioFactura> Facturas { get; set; } // Modelo de facturas personalizadas
    }
}
