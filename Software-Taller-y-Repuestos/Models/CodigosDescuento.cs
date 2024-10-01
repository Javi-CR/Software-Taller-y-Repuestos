using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class CodigosDescuento
{
    public int CodigoId { get; set; }

    public string Codigo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal ValorDescuento { get; set; }

    public string? TipoDescuento { get; set; }

    public DateTime? FechaExpiracion { get; set; }
}
