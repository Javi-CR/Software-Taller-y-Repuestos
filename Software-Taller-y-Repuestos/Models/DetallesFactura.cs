using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class DetallesFactura
{
    public int DetalleId { get; set; }

    public int FacturaId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public virtual Factura Factura { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
