using System.ComponentModel.DataAnnotations;

namespace Software_Taller_y_Repuestos.Models
{
    public class UsuarioPerfil
    {
        [Required]
        [StringLength(70, ErrorMessage = "El nombre no puede exceder los 40 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(70, ErrorMessage = "El apellido no puede exceder los 40 caracteres.")]
        public string Apellidos { get; set; } = string.Empty;

        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe contener solo 8 dígitos y deben ser numéricos.")]
        public string? Telefono { get; set; }

        [StringLength(120, ErrorMessage = "La dirección no puede exceder los 40 caracteres.")]
        public string? Direccion { get; set; }

        public string? Imagen { get; set; }
    }
}
