using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class ModelosAuto
{
    public int ModeloId { get; set; }

    public string Nombre { get; set; } = null!;

    public int Año { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
