using System.ComponentModel.DataAnnotations.Schema;

namespace Software_Taller_y_Repuestos.Models
{

    [NotMapped]
    public class Login
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        public string NombreRol { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;


    }
}
