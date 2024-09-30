using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class Empleado
{
    public int EmpleadoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Puesto { get; set; }

    public DateTime FechaIngreso { get; set; }

    public decimal SalarioBase { get; set; }

    public virtual ICollection<PagosEmpleado> PagosEmpleados { get; set; } = new List<PagosEmpleado>();
}
