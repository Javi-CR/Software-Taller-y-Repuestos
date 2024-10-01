using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class Factura
{
    public int FacturaId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    public decimal? Impuestos { get; set; }

    public decimal? Descuento { get; set; }

    public bool CorreoEnviado { get; set; }

    public int? CodigoDescuentoId { get; set; }

    public virtual ICollection<DetallesFactura> DetallesFacturas { get; set; } = new List<DetallesFactura>();

    public virtual Usuario Usuario { get; set; } = null!;
}
