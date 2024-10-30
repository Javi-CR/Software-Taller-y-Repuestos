using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Software_Taller_y_Repuestos.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Software_Taller_y_Repuestos.Controllers
{
    public class FacturaController : Controller
    {
        private readonly TallerRepuestosDbContext _context;
        private readonly ILogger<FacturaController> _logger;

        public FacturaController(TallerRepuestosDbContext context, ILogger<FacturaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Factura
        public async Task<IActionResult> Index()
        {
            var facturas = await _context.Facturas.Include(f => f.Usuario).ToListAsync();
            return View(facturas);
        }

        // GET: Factura/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Factura ID nulo en Details.");
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.DetallesFacturas)
                .ThenInclude(df => df.Producto)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(m => m.FacturaId == id);

            if (factura == null)
            {
                _logger.LogWarning("Factura no encontrada con ID {FacturaId}.", id);
                return NotFound();
            }

            return View(factura);
        }

        // GET: Factura/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nombre");
            return View();
        }

        // POST: Factura/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FacturaId,UsuarioId,Fecha,Total,Impuestos,Descuento,CorreoEnviado,CodigoDescuentoId")] Factura factura)
        {
            _logger.LogInformation("Iniciando la creación de una nueva factura.");

            if (ModelState.IsValid)
            {
                _context.Add(factura);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Factura creada correctamente con ID {FacturaId}.", factura.FacturaId);
                return RedirectToAction(nameof(Index));
            }

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nombre", factura.UsuarioId);
            return View(factura);
        }

        // GET: Factura/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Factura ID nulo en Edit.");
                return NotFound();
            }

            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                _logger.LogWarning("Factura no encontrada con ID {FacturaId}.", id);
                return NotFound();
            }

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nombre", factura.UsuarioId);
            return View(factura);
        }

        // POST: Factura/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacturaId,UsuarioId,Fecha,Total,Impuestos,Descuento,CorreoEnviado,CodigoDescuentoId")] Factura factura)
        {
            if (id != factura.FacturaId)
            {
                _logger.LogWarning("El ID proporcionado no coincide con la factura.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(factura);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Factura editada correctamente.");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError("Error de concurrencia al editar la factura: {Message}", ex.Message);
                    if (!FacturaExists(factura.FacturaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nombre", factura.UsuarioId);
            return View(factura);
        }

        // GET: Factura/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Factura ID nulo en Delete.");
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(m => m.FacturaId == id);
            if (factura == null)
            {
                _logger.LogWarning("Factura no encontrada con ID {FacturaId}.", id);
                return NotFound();
            }

            return View(factura);
        }

        // POST: Factura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura != null)
            {
                _context.Facturas.Remove(factura);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Factura eliminada con éxito con ID {FacturaId}.", id);
            }
            else
            {
                _logger.LogWarning("Factura no encontrada al intentar eliminar.");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FacturaExists(int id)
        {
            return _context.Facturas.Any(e => e.FacturaId == id);
        }

    }
}
