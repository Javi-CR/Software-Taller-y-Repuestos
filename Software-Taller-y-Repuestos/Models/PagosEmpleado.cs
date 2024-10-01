using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class PagosEmpleado
{
    public int PagoId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaPago { get; set; }

    public decimal SalarioPagado { get; set; }

    public decimal? HorasExtras { get; set; }

    public decimal? Deducciones { get; set; }

    public decimal? Bonificaciones { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
