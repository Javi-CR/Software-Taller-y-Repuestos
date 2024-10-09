using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Software_Taller_y_Repuestos.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Software_Taller_y_Repuestos.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TallerRepuestosDbContext _context;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(TallerRepuestosDbContext context, ILogger<ProductoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Producto
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["SortOrder"] = sortOrder;

            var productos = _context.Productos.Include(p => p.Categoria).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Nombre.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    productos = productos.OrderByDescending(p => p.Nombre);
                    break;
                case "price":
                    productos = productos.OrderBy(p => p.PrecioVenta);
                    break;
                case "price_desc":
                    productos = productos.OrderByDescending(p => p.PrecioVenta);
                    break;
                default:
                    productos = productos.OrderBy(p => p.Nombre);
                    break;
            }

            return View(await productos.ToListAsync());
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Producto ID nulo en Details.");
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                _logger.LogWarning("Producto no encontrado con ID {ProductoId}.", id);
                return NotFound();
            }

            return View(producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Codigo,CategoriaId,Descripcion,Cantidad,PrecioCompra,PrecioVenta,Marca,Imagen")] Producto producto)
        {
            _logger.LogInformation("Iniciando la creación de un nuevo producto.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(producto);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Producto creado correctamente con ID {ProductoId}.", producto.ProductoId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error al crear el producto: {Message}", ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("El modelo de Producto no es válido.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Error en el modelo: {ErrorMessage}", error.ErrorMessage);
                }
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Producto ID nulo en Edit.");
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                _logger.LogWarning("Producto no encontrado con ID {ProductoId}.", id);
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Codigo,CategoriaId,Descripcion,Cantidad,PrecioCompra,PrecioVenta,Marca,Imagen")] Producto producto)
        {
            if (id != producto.ProductoId)
            {
                _logger.LogWarning("El ID proporcionado no coincide con el producto.");
                return NotFound();
            }

            _logger.LogInformation("Iniciando la edición del producto con ID {ProductoId}.", producto.ProductoId);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Producto editado correctamente.");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError("Error de concurrencia al editar el producto: {Message}", ex.Message);
                    if (!ProductoExists(producto.ProductoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error inesperado al editar el producto: {Message}", ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("El modelo de Producto no es válido.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Error en el modelo: {ErrorMessage}", error.ErrorMessage);
                }
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Producto ID nulo en Delete.");
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                _logger.LogWarning("Producto no encontrado con ID {ProductoId}.", id);
                return NotFound();
            }

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Producto eliminado con éxito con ID {ProductoId}.", id);
            }
            else
            {
                _logger.LogWarning("Producto no encontrado al intentar eliminar.");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }
    }
}
