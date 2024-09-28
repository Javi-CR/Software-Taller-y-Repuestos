using System;
using System.Collections.Generic;

namespace Taller_y_Repuestos.Models;

public partial class Factura
{
    public int FacturaId { get; set; }

    public int ClienteId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    public decimal? Impuestos { get; set; }

    public decimal? Descuento { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<DetallesFactura> DetallesFacturas { get; set; } = new List<DetallesFactura>();
}
