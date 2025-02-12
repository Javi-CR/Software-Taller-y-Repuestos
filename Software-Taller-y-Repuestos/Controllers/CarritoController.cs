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
            try
            {
                var carrito = HttpContext.Session.Get<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

                if (!carrito.Any())
                {
                    TempData["Message"] = "El carrito está vacío.";
                    return RedirectToAction("Index");
                }

                if (recibo == null || recibo.Length == 0)
                {
                    TempData["Message"] = "Por favor, sube un archivo válido.";
                    return RedirectToAction("ProcesoPago");
                }

                // Verificar stock antes de procesar la venta
                foreach (var item in carrito)
                {
                    var producto = await _context.Productos.FindAsync(item.Producto.ProductoId);

                    if (producto == null || !producto.Activo || producto.Cantidad < item.Cantidad)
                    {
                        TempData["Message"] = $"No hay suficiente stock para el producto {item.Producto.Nombre}.";
                        return RedirectToAction("Index");
                    }
                }

                // Guardar el archivo recibido
                string imagenPath;
                var fileName = Path.GetFileName(recibo.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await recibo.CopyToAsync(stream);
                }

                imagenPath = $"/uploads/{fileName}"; // Ruta relativa para guardar en la base de datos

                // Obtener el ID del usuario autenticado desde los claims
                var userIdClaim = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    TempData["Message"] = "No se pudo obtener el ID del usuario autenticado.";
                    return RedirectToAction("Index");
                }

                // Crear y guardar factura
                var factura = new Factura
                {
                    UsuarioId = userIdClaim,
                    FechaCompra = DateTime.Now,
                    Subtotal = carrito.Sum(item => item.Producto.PrecioVenta * item.Cantidad),
                    IVA = carrito.Sum(item => item.Producto.PrecioVenta * item.Cantidad) * 0.13m,
                    Total = carrito.Sum(item => item.Producto.PrecioVenta * item.Cantidad) * 1.13m
                };

                _context.Facturas.Add(factura);
                await _context.SaveChangesAsync();

                // Crear detalles de factura y actualizar el stock
                foreach (var item in carrito)
                {
                    var detalleFactura = new DetalleFactura
                    {
                        FacturaId = factura.FacturaId,
                        ProductoId = item.Producto.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.Producto.PrecioVenta,
                        ImagenFactura = imagenPath,
                        EstadoPago = "Pendiente"
                    };

                    _context.DetalleFacturas.Add(detalleFactura);

                    // Actualizar el stock
                    var producto = await _context.Productos.FindAsync(item.Producto.ProductoId);
                    producto.Cantidad -= item.Cantidad;
                    _context.Update(producto);
                }

                await _context.SaveChangesAsync();

                // Limpiar el carrito
                HttpContext.Session.Remove("Carrito");

                TempData["Message"] = "El pago con Sinpe Móvil fue registrado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ocurrió un error al procesar el pago";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PagoPresencial()
        {
            try
            {
                // Obtener el carrito de la sesión
                var carrito = HttpContext.Session.Get<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();

                if (!carrito.Any())
                {
                    TempData["Message"] = "El carrito está vacío.";
                    return RedirectToAction("Index");
                }

                // Verificar stock antes de procesar la venta
                foreach (var item in carrito)
                {
                    var producto = await _context.Productos.FindAsync(item.Producto.ProductoId);

                    if (producto == null || !producto.Activo || producto.Cantidad < item.Cantidad)
                    {
                        TempData["Message"] = $"No hay suficiente stock para el producto {item.Producto.Nombre}.";
                        return RedirectToAction("Index");
                    }
                }

                // Obtener el ID del usuario autenticado desde los claims
                var userIdClaim = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    TempData["Message"] = "No se pudo obtener el ID del usuario autenticado.";
                    return RedirectToAction("Index");
                }

                // Crear y guardar la factura
                var factura = new Factura
                {
                    UsuarioId = userIdClaim,
                    FechaCompra = DateTime.Now,
                    Subtotal = carrito.Sum(item => item.Producto.PrecioVenta * item.Cantidad),
                    IVA = carrito.Sum(item => item.Producto.PrecioVenta * item.Cantidad) * 0.13m,
                    Total = carrito.Sum(item => item.Producto.PrecioVenta * item.Cantidad) * 1.13m
                };

                _context.Facturas.Add(factura);
                await _context.SaveChangesAsync();

                // Crear y guardar los detalles de factura
                foreach (var item in carrito)
                {
                    var detalleFactura = new DetalleFactura
                    {
                        FacturaId = factura.FacturaId,
                        ProductoId = item.Producto.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.Producto.PrecioVenta,
                        ImagenFactura = "/img/default.png", // No aplica para pagos presenciales
                        EstadoPago = "Presencial"
                    };

                    _context.DetalleFacturas.Add(detalleFactura);

                    // Actualizar el stock
                    var producto = await _context.Productos.FindAsync(item.Producto.ProductoId);
                    producto.Cantidad -= item.Cantidad;
                    _context.Update(producto);
                }

                await _context.SaveChangesAsync();

                // Limpiar el carrito
                HttpContext.Session.Remove("Carrito");

                TempData["Message"] = "El pago presencial fue registrado correctamente. Por favor, continúe con su pago en la sucursal.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ocurrió un error al procesar el pago";
                return RedirectToAction("Index");
            }
        }




    }

    public class CarritoItem
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
