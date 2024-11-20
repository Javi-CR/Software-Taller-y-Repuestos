using Microsoft.AspNetCore.Mvc;
using Software_Taller_y_Repuestos.Models;
using Software_Taller_y_Repuestos.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Software_Taller_y_Repuestos.Controllers
{
    public class FacturaController : Controller
    {
        private readonly TallerRepuestosDbContext _context;

        public FacturaController(TallerRepuestosDbContext context)
        {
            _context = context;
        }

        public IActionResult ConfirmarCompra()
        {
            var carrito = HttpContext.Session.Get<List<Producto>>("Carrito");

            if (carrito == null || !carrito.Any())
            {
                // Si el carrito está vacío o no existe, redirigir al carrito
                return RedirectToAction("Index", "Carrito");
            }

            // Verificar que el carrito tenga productos con los datos necesarios
            var carritoViewModel = carrito.Select(p => new CarritoViewModel
            {
                Producto = p,
                Cantidad = 1 // Asegúrate de que esta cantidad esté bien establecida
            }).ToList();

            // Calcular el subtotal
            var subtotal = carritoViewModel.Where(p => p.Producto != null && p.Cantidad > 0)
                                            .Sum(p => p.Cantidad * p.Producto.PrecioVenta);

            var iva = subtotal * 0.13m; //IVA (13%)
            var total = subtotal + iva;

            // Crear la factura y asegurarse de que los detalles de factura estén correctamente inicializados
            var factura = new Factura
            {
                FechaCompra = DateTime.Now,
                Subtotal = subtotal,
                IVA = iva,
                Total = total,
                DetalleFacturas = carritoViewModel.Select(p => new DetalleFactura
                {
                    ProductoId = p.Producto.ProductoId,
                    Cantidad = p.Cantidad,
                    PrecioUnitario = p.Producto.PrecioVenta
                }).ToList() ?? new List<DetalleFactura>()
            };

            _context.Facturas.Add(factura);
            _context.SaveChanges();

            HttpContext.Session.Remove("Carrito"); // Limpiar el carrito de la sesión

            return View(factura); // Mostrar la factura generada
        }




        public IActionResult Historial()
        {
            var usuarioId = User.Identity.Name; // Obtener el usuario autenticado
            var facturas = _context.Facturas
                                   .Where(f => f.UsuarioId == usuarioId)
                                   .OrderByDescending(f => f.FechaCompra)
                                   .ToList();
            return View(facturas); // Mostrar el historial de facturas del usuario
        }

        public IActionResult VerDetalleFactura(int facturaId)
        {
            var factura = _context.Facturas
                                .Where(f => f.FacturaId == facturaId)
                                .Include(f => f.DetalleFacturas)
                                .ThenInclude(d => d.Producto)
                                .FirstOrDefault();

            if (factura == null)
            {
                return NotFound();
            }

            return View(factura); // Mostrar los detalles de la factura seleccionada
        }
    }
}
