using System;
using System.Collections.Generic;

namespace Software_Taller_y_Repuestos.Models;

public partial class Producto
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = null!;
    public string Codigo { get; set; } = null!;
    public string? CategoriaNombre { get; set; }
    public int CategoriaId { get; set; }
    public string? Descripcion { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public string? Marca { get; set; }
    public string? Imagen { get; set; }
    public virtual Categoria? Categoria { get; set; } = null!;
    public virtual ICollection<DetallesFactura> DetallesFacturas { get; set; } = new List<DetallesFactura>();
    public virtual ICollection<ModelosAuto> Modelos { get; set; } = new List<ModelosAuto>();
    public virtual ICollection<Proveedore> Proveedors { get; set; } = new List<Proveedore>();
}

