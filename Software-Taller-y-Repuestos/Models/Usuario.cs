using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Software_Taller_y_Repuestos.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = string.Empty;


    public string Apellidos { get; set; } = string.Empty;


    public string Correo { get; set; } = string.Empty;


    public string Contrasenna { get; set; } = string.Empty;

    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public int RolId { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public decimal? SalarioBase { get; set; }
    public string? Imagen { get; set; }


    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual ICollection<PagosEmpleado> PagosEmpleados { get; set; } = new List<PagosEmpleado>();

    public virtual Role Rol { get; set; } = null!;


}