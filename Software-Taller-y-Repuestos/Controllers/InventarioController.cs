using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Software_Taller_y_Repuestos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Software_Taller_y_Repuestos.Controllers
{
    [Authorize(Roles = "Admin,Empleado")]
    public class InventarioController : Controller
    {
        private readonly TallerRepuestosDbContext _context;

        public InventarioController(TallerRepuestosDbContext context)
        {
            _context = context;
        }

        // GET: Inventario/Index
        public async Task<IActionResult> Index(int? page)
        {
            await NotificarStockBajo();

            // Consulta inicial
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .AsQueryable();

            // Configurar la paginación
            int pageSize = 9; // Número de elementos por página
            int pageNumber = (page ?? 1); // Número de página actual (si no se especifica, es 1)

            // Obtener los productos para la página actual
            var productosPaginados = await productos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Calcular el número total de páginas
            int totalProductos = await productos.CountAsync();
            int totalPages = (int)Math.Ceiling(totalProductos / (double)pageSize);

            // Pasar los datos a la vista
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            return View(productosPaginados);
        }

        // GET: Inventario/RegistrarSalida
        public IActionResult RegistrarSalida()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos.Where(p => p.Activo), "ProductoId", "Nombre");
            return View();
        }

        // POST: Inventario/RegistrarSalida
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarSalida(int productoId, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(productoId);

            if (producto == null || !producto.Activo)
            {
                ModelState.AddModelError("", "Producto no encontrado o inactivo.");
                return View();
            }

            if (producto.Cantidad < cantidad)
            {
                ModelState.AddModelError("", "No hay suficiente stock para realizar la salida.");
                return View();
            }

            producto.Cantidad -= cantidad;
            _context.Update(producto);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Salida de inventario registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Inventario/Auditoria
        public async Task<IActionResult> Auditoria()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .ToListAsync();

            return View(productos);
        }

        // POST: Inventario/Auditoria
        [HttpPost]
        public async Task<IActionResult> Auditoria(List<Producto> productos)
        {
            foreach (var item in productos)
            {
                var productoDb = await _context.Productos.FindAsync(item.ProductoId);

                if (productoDb != null)
                {
                    productoDb.Cantidad = item.Cantidad;
                    _context.Update(productoDb);
                }
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Auditoría de inventario completada.";
            return RedirectToAction(nameof(Index));
        }

        // Notificar stock bajo
        private async Task NotificarStockBajo()
        {
            const int stockMinimo = 25; // Umbral de stock mínimo

            var productosBajoStock = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Cantidad < stockMinimo && p.Activo)
                .ToListAsync();

            var mensajesStockBajo = new List<string>();

            foreach (var item in productosBajoStock)
            {
                mensajesStockBajo.Add($"El producto {item.Nombre} tiene stock bajo: {item.Cantidad} unidades.");
            }

            if (mensajesStockBajo.Any())
            {
                TempData["StockBajo"] = string.Join("<br/>", mensajesStockBajo);
            }
        }

        // Cambiar estado activo/inactivo
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, int activo)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            producto.Activo = activo == 1; // Convertir 1 a true y 0 a false
            _context.Update(producto);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Producto {(producto.Activo ? "activado" : "desactivado")} correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
