using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Software_Taller_y_Repuestos.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Software_Taller_y_Repuestos.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Software_Taller_y_Repuestos.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly TallerRepuestosDbContext _context;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(TallerRepuestosDbContext context, ILogger<ProductoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Producto/Index
        [Authorize(Roles = "Admin,Empleado,Cliente")]
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? page)
        {
            _logger.LogInformation("Accediendo a la lista de productos.");

            // Mantener los filtros actuales en la vista
            ViewData["CurrentFilter"] = searchString;
            ViewData["SortOrder"] = sortOrder;

            // Consulta inicial (solo productos activos)
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Activo) // Solo productos activos
                .AsQueryable();

            // Filtrar por nombre
            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Nombre.Contains(searchString));
            }

            // Ordenar resultados
            productos = sortOrder switch
            {
                "name_desc" => productos.OrderByDescending(p => p.Nombre),
                "price" => productos.OrderBy(p => p.PrecioVenta),
                "price_desc" => productos.OrderByDescending(p => p.PrecioVenta),
                _ => productos.OrderBy(p => p.Nombre),
            };

            // Configurar la paginación
            int pageSize = 15; // Número de elementos por página
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

        // GET: Producto/Details/5
        [Authorize(Roles = "Admin,Empleado,Cliente")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("ID nulo en Details.");
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id && m.Activo); // Solo productos activos

            if (producto == null)
            {
                _logger.LogWarning($"Producto con ID {id} no encontrado.");
                return NotFound();
            }

            return View(producto);
        }

        // GET: Producto/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Nombre,Codigo,CategoriaId,Descripcion,Cantidad,PrecioCompra,PrecioVenta,Marca,Activo")] Producto producto, IFormFile imagen)
        {
            if (ModelState.IsValid)
            {
                // Subir imagen si se proporciona
                if (imagen != null && imagen.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(imagen.FileName);
                    var extension = Path.GetExtension(imagen.FileName);
                    var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    producto.Imagen = $"/images/{newFileName}";
                }

                producto.Activo = true; // el producto está activo
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Producto/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("ID nulo en Edit.");
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                _logger.LogWarning($"Producto con ID {id} no encontrado.");
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Codigo,CategoriaId,Descripcion,Cantidad,PrecioCompra,PrecioVenta,Marca,Imagen,Activo")] Producto producto, IFormFile imagen)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Subir nueva imagen si se proporciona
                    if (imagen != null && imagen.Length > 0)
                    {
                        // Eliminar la imagen anterior si existe
                        if (!string.IsNullOrEmpty(producto.Imagen))
                        {
                            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", producto.Imagen.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        var fileName = Path.GetFileNameWithoutExtension(imagen.FileName);
                        var extension = Path.GetExtension(imagen.FileName);
                        var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imagen.CopyToAsync(stream);
                        }

                        producto.Imagen = $"/images/{newFileName}";
                    }

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // Desactivar producto (en lugar de eliminar)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            producto.Activo = false; // Desactivar el producto
            _context.Update(producto);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Producto desactivado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Producto/Upload
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Por favor, seleccione un archivo.");
                return RedirectToAction(nameof(Index));
            }

            if (!file.FileName.EndsWith(".csv") && !file.FileName.EndsWith(".xlsx"))
            {
                ModelState.AddModelError("", "Formato no válido. Solo archivos CSV o Excel.");
                return RedirectToAction(nameof(Index));
            }

            var productos = await ProcesarArchivo(file);

            if (productos.Count > 0)
            {
                foreach (var producto in productos)
                {
                    var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == producto.CategoriaNombre)
                                   ?? new Categoria { Nombre = producto.CategoriaNombre };

                    if (categoria.CategoriaId == 0)
                    {
                        _context.Categorias.Add(categoria);
                        await _context.SaveChangesAsync();
                    }

                    producto.CategoriaId = categoria.CategoriaId;

                    if (!_context.Productos.Any(p => p.Codigo == producto.Codigo))
                    {
                        _context.Productos.Add(producto);
                    }
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Archivo procesado correctamente.");
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<Producto>> ProcesarArchivo(IFormFile file)
        {
            var productos = new List<Producto>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (worksheet == null)
                    {
                        _logger.LogWarning("El archivo no contiene hojas.");
                        return productos;
                    }

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        productos.Add(new Producto
                        {
                            Nombre = worksheet.Cells[row, 1].Text,
                            Codigo = worksheet.Cells[row, 2].Text,
                            CategoriaNombre = worksheet.Cells[row, 3].Text,
                            Descripcion = worksheet.Cells[row, 4].Text,
                            Cantidad = int.TryParse(worksheet.Cells[row, 5].Text, out var cantidad) ? cantidad : 0,
                            PrecioCompra = decimal.TryParse(worksheet.Cells[row, 6].Text, out var compra) ? compra : 0,
                            PrecioVenta = decimal.TryParse(worksheet.Cells[row, 7].Text, out var venta) ? venta : 0,
                            Marca = worksheet.Cells[row, 8].Text,
                            Imagen = worksheet.Cells[row, 9].Text
                        });
                    }
                }
            }

            return productos;
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }
    }
}