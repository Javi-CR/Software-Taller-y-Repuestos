using System.ComponentModel.DataAnnotations;

namespace Software_Taller_y_Repuestos.Models
{
    public class EditEmpleado
    {
        public int UsuarioId { get; set; }

        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe contener solo 8 dígitos y deben ser numéricos.")]
        public string? Telefono { get; set; }


        [StringLength(120, ErrorMessage = "La dirección no puede exceder los 40 caracteres.")]
        public string? Direccion { get; set; }



        [Range(0, 9999999999999999999, ErrorMessage = "El salario debe ser un número positivo de máximo 20 dígitos.")]
        public decimal? SalarioBase { get; set; }


    }
}
