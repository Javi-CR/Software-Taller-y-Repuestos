using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Software_Taller_y_Repuestos.Models
{

    [NotMapped]
    public class CuentaUsuario
    {

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [MaxLength(50, ErrorMessage = "Los apellidos no pueden exceder los 50 caracteres.")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z]).{8,}$",
        ErrorMessage = "La contraseña debe incluir al menos una mayúscula, una minúscula y tener al menos 8 caracteres.")]
        public string Contrasenna { get; set; } = string.Empty;


    }
}
