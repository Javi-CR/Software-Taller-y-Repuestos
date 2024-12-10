using Microsoft.AspNetCore.Mvc;
using Software_Taller_y_Repuestos.Models;
using Software_Taller_y_Repuestos.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Software_Taller_y_Repuestos.Controllers
{
    public class CarritoController : Controller
    {
        private readonly TallerRepuestosDbContext _context;

        public CarritoController(TallerRepuestosDbContext context)
        {
            _context = context;
        }

        // Mostrar el carrito de compras
        public IActionResult Index()
        {
            var carrito = HttpContext.Session.Get<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

            var carritoViewModel = carrito.Select(item => new CarritoViewModel
            {
                Producto = item.Producto,
                Cantidad = item.Cantidad
            }).ToList();

            // Calcular el total del carrito
            var total = carritoViewModel.Sum(item => item.Subtotal);

            ViewData["Total"] = total;

            return View(carritoViewModel);
        }

        // Agregar producto al carrito
        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int productoId)
        {
            var producto = await _context.Productos.FindAsync(productoId);

            if (producto == null)
            {
                return NotFound();
            }

            var carrito = HttpContext.Session.Get<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

            var carritoItem = carrito.FirstOrDefault(item => item.Producto.ProductoId == productoId);

            if (carritoItem != null)
            {
                // Si el producto ya está en el carrito, incrementar la cantidad
                carritoItem.Cantidad++;
            }
            else
            {
                // Si el producto no está, agregarlo
                carrito.Add(new CarritoItem { Producto = producto, Cantidad = 1 });
            }

            HttpContext.Session.Set("Carrito", carrito);

            TempData["Message"] = "Producto agregado exitosamente al carrito.";

            return RedirectToAction("Index", "Producto");
        }

        // Eliminar un producto del carrito
        [HttpPost]
        public IActionResult EliminarDelCarrito(int productoId)
        {
            var carrito = HttpContext.Session.Get<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

            var item = carrito.FirstOrDefault(p => p.Producto.ProductoId == productoId);
            if (item != null)
            {
                carrito.Remove(item);
                HttpContext.Session.Set("Carrito", carrito);
            }

            return RedirectToAction("Index");
        }

        // Vaciar el carrito completo
        [HttpPost]
        public IActionResult VaciarCarrito()
        {
            HttpContext.Session.Remove("Carrito");
            return RedirectToAction("Index");
        }

        // Modificar la cantidad de un producto en el carrito
        [HttpPost]
        public IActionResult ModificarCantidad(int productoId, int cantidad)
        {
            var carrito = HttpContext.Session.Get<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

            var item = carrito.FirstOrDefault(p => p.Producto.ProductoId == productoId);
            if (item != null && cantidad > 0)
            {
                item.Cantidad = cantidad;  // Actualizar la cantidad
            }

            HttpContext.Session.Set("Carrito", carrito);

            return RedirectToAction("Index");
        }

        // Acción para mostrar el popup de opciones de pago
        public IActionResult ProcesoPago()
        {
            return PartialView("_PopupPago");
        }

        // Acción para manejar el pago con Sinpe Móvil
        [HttpPost]
        public async Task<IActionResult> PagoSinpe(IFormFile recibo)
        {
            if (recibo != null && recibo.Length > 0)
            {
                var fileName = Path.GetFileName(recibo.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                // Crear carpeta si no existe
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await recibo.CopyToAsync(stream);
                }

                TempData["Message"] = "El recibo se subió correctamente.";
            }
            else
            {
                TempData["Error"] = "Por favor, sube un archivo válido.";
            }

            return RedirectToAction("Index");
        }

        // Acción para manejar el pago presencial
        [HttpPost]
        public IActionResult PagoPresencial()
        {
            TempData["Message"] = "Gracias. Continúe con su pago en la sucursal.";
            return RedirectToAction("Index");
        }

        // Acción para ir a la facturación
        public IActionResult Facturacion()
        {
            var carrito = HttpContext.Session.Get<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

            if (!carrito.Any())
            {
                return RedirectToAction("Index");
            }

            var carritoViewModel = carrito.Select(item => new CarritoViewModel
            {
                Producto = item.Producto,
                Cantidad = item.Cantidad
            }).ToList();

            var total = carritoViewModel.Sum(item => item.Subtotal);

            ViewData["Total"] = total;

            return View(carritoViewModel);
        }
    }

    public class CarritoItem
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
