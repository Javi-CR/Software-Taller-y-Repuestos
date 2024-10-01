using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class CuentaBancarium
{
    public int CuentaId { get; set; }

    public int UsuarioId { get; set; }

    public string Banco { get; set; } = null!;

    public string NumeroCuenta { get; set; } = null!;

    public string? TipoCuenta { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
