using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Software_Taller_y_Repuestos.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

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

        // GET: Producto/Index
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            _logger.LogInformation("Accediendo a la lista de productos.");

            // Mantener los filtros actuales en la vista
            ViewData["CurrentFilter"] = searchString;
            ViewData["SortOrder"] = sortOrder;

            // Consulta inicial
            var productos = _context.Productos.Include(p => p.Categoria).AsQueryable();

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

            return View(await productos.ToListAsync());
        }


        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("ID nulo en Details.");
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);

            if (producto == null)
            {
                _logger.LogWarning($"Producto con ID {id} no encontrado.");
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
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(producto);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Producto creado con éxito: {producto.Nombre}");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error al crear el producto: {ex.Message}");
                    ModelState.AddModelError("", "No se pudo guardar el producto. Intente nuevamente.");
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Producto con ID {id} editado correctamente.");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Error de concurrencia al editar: {ex.Message}");
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
                    _logger.LogError($"Error al editar producto: {ex.Message}");
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
                _logger.LogWarning("ID nulo en Delete.");
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);

            if (producto == null)
            {
                _logger.LogWarning($"Producto con ID {id} no encontrado.");
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
                _logger.LogInformation($"Producto con ID {id} eliminado correctamente.");
            }
            else
            {
                _logger.LogWarning($"Producto con ID {id} no encontrado durante la eliminación.");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }


        // POST: Producto/Upload
        [HttpPost]
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
