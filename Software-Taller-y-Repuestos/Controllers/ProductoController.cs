using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Software_Taller_y_Repuestos.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;

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
        public async Task<IActionResult> Index(string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["PriceSortParam"] = sortOrder == "price" ? "price_desc" : "price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var productos = from p in _context.Productos.Include(p => p.Categoria)
                            select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Nombre.Contains(searchString));
            }

            productos = sortOrder switch
            {
                "name_desc" => productos.OrderByDescending(p => p.Nombre),
                "price" => productos.OrderBy(p => p.PrecioVenta),
                "price_desc" => productos.OrderByDescending(p => p.PrecioVenta),
                _ => productos.OrderBy(p => p.Nombre),
            };

            int pageSize = 10; // Cantidad de productos por página
            return View(await PaginatedList<Producto>.CreateAsync(productos.AsNoTracking(), pageNumber ?? 1, pageSize));
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
        public async Task<IActionResult> Create(Producto producto)
        {
            _logger.LogInformation("Iniciando la creación de un nuevo producto.");

            // Registra el valor de CategoriaId recibido en el modelo
            _logger.LogInformation("Valor de CategoriaId recibido: {CategoriaId}", producto.CategoriaId);

            // Asignar la relación de categoría explícitamente
            producto.Categoria = await _context.Categorias.FindAsync(producto.CategoriaId);

            // Verificación del código único
            if (_context.Productos.Any(p => p.Codigo == producto.Codigo))
            {
                ModelState.AddModelError("Codigo", "El código ingresado ya está en uso. Por favor ingrese uno nuevo.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Producto creado exitosamente.");
                return RedirectToAction(nameof(Index));
            }


            // Si el modelo no es válido, registrar un mensaje de advertencia
            _logger.LogWarning("El modelo de Producto no es válido.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning("Error en el modelo: {ErrorMessage}", error.ErrorMessage);
            }

            // Recargar las categorías en caso de que la creación falle
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }


        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
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
                return NotFound();
            }

            // Verificación del código único para evitar duplicados
            if (_context.Productos.Any(p => p.Codigo == producto.Codigo && p.ProductoId != producto.ProductoId))
            {
                ModelState.AddModelError("Codigo", "El código ingresado ya está en uso. Por favor ingrese uno nuevo.");
            }

            if (!ModelState.IsValid)
            {
                // Cargar las categorías de nuevo para la lista desplegable en caso de error
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
                return View(producto); // Regresa a la vista Edit con el mensaje de error
            }

            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Producto actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.ProductoId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
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
                TempData["Mensaje"] = "Producto eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }

        [HttpGet]
        public async Task<IActionResult> VerificarCodigoUnico(string codigo, int? productoId)
        {
            // Verifica si el código ya existe en otro producto
            bool existeCodigo = await _context.Productos.AnyAsync(p => p.Codigo == codigo && p.ProductoId != productoId);
            return Json(!existeCodigo); // Retorna false si existe (para que muestre el mensaje de error)
        }

        public class PaginatedList<T> : List<T>
        {
            public int PageIndex { get; private set; }
            public int TotalPages { get; private set; }

            public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
            {
                PageIndex = pageIndex;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                AddRange(items);
            }

            public bool HasPreviousPage => PageIndex > 1;
            public bool HasNextPage => PageIndex < TotalPages;

            public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
            {
                var count = await source.CountAsync();
                var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
        }


    }
}
