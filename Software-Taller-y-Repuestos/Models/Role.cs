using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class Role
{
    public int RolId { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
