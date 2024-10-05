using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class Proveedore
{
    public int ProveedorId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Ruc { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
