using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class Horario
{
    public int HorarioId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal HorasTrabajadas { get; set; }

    public decimal HorasExtras { get; set; }

    public decimal Ausencias { get; set; }

    public decimal Permisos { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
