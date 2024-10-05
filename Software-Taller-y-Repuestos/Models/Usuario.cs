using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;
    public string Contrasenna { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public int RolId { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public decimal? SalarioBase { get; set; }

    public string? Imagen { get; set; }

    public virtual ICollection<CuentaBancarium> CuentaBancaria { get; set; } = new List<CuentaBancarium>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual ICollection<PagosEmpleado> PagosEmpleados { get; set; } = new List<PagosEmpleado>();

    public virtual Role Rol { get; set; } = null!;
}
