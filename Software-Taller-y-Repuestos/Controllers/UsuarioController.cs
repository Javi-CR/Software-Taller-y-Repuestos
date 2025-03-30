using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Software_Taller_y_Repuestos.Models;
using System.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Software_Taller_y_Repuestos.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IConfiguration _conf;

        public UsuarioController(IConfiguration conf)
        {
            _conf = conf;
        }



        [HttpGet]
        public IActionResult Perfil()
        {
            // Recuperar el ID del usuario desde los claims
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return RedirectToAction("Logout", "Home");
            }

            var usuarioId = int.Parse(userIdClaim.Value);

            using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                // Obtener el modelo Usuario desde la base de datos
                var usuario = connection.QueryFirstOrDefault<Usuario>(
                    "ConsultarPerfilUsuario",
                    new { UsuarioID = usuarioId },
                    commandType: CommandType.StoredProcedure);

                if (usuario == null)
                {
                    ViewBag.ErrorMessage = "Usuario no encontrado.";
                    return View();
                }

                // Obtener las facturas del usuario (modelo UsuarioFactura)
                var facturas = connection.Query<UsuarioFactura>(
                    "ObtenerFacturaPorUsuario",
                    new { UsuarioId = usuarioId },
                    commandType: CommandType.StoredProcedure).ToList();

                // Crear el ViewModel
                var viewModel = new PerfilViewModel
                {
                    Usuario = usuario,
                    Facturas = facturas
                };

                return View(viewModel);
            }
        }


        [HttpGet]
        public IActionResult EditarPerfil()
        {
            // Recuperar el UsuarioID desde los claims
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return RedirectToAction("Logout", "Home"); // Redirigir si no está autenticado
            }

            var usuarioId = int.Parse(userIdClaim.Value);

            using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var usuario = connection.QueryFirstOrDefault<UsuarioPerfil>(
                    "ConsultarPerfilUsuario",
                    new { UsuarioID = usuarioId },
                    commandType: System.Data.CommandType.StoredProcedure);

                if (usuario == null)
                {
                    return View();
                }

                return View(usuario);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil(UsuarioPerfil model, IFormFile Imagen)
        {
            try
            {
                // Recuperar el UsuarioID desde los claims
                var userIdClaim = User.FindFirst("UserId");
                var username = User.FindFirst(ClaimTypes.Name)?.Value;

                if (userIdClaim == null)
                {
                    return RedirectToAction("Logout", "Home");
                }

                var usuarioId = int.Parse(userIdClaim.Value);

                string imagenAnterior;

                // Obtener datos del usuario desde la base de datos
                using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    var usuario = connection.QueryFirstOrDefault<UsuarioPerfil>(
                        "ConsultarPerfilUsuario",
                        new { UsuarioID = usuarioId },
                        commandType: CommandType.StoredProcedure);

                    if (usuario == null)
                    {
                        ViewBag.ErrorMessage = "Usuario no encontrado.";
                        return View(model ?? new UsuarioPerfil()); // Devuelve un modelo inicializado
                    }

                    imagenAnterior = usuario.Imagen;
                }

                // Procesar la nueva imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    // Validar que el archivo sea una imagen
                    var formatosValidos = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extensionArchivo = Path.GetExtension(Imagen.FileName).ToLowerInvariant();

                    if (!formatosValidos.Contains(extensionArchivo))
                    {
                        ViewBag.ErrorMessage = "El archivo debe ser una imagen en formato JPG, JPEG, PNG o GIF.";
                        return View(model);
                    }

                    // Validar tipo MIME (opcional pero recomendable)
                    if (!Imagen.ContentType.StartsWith("image/"))
                    {
                        ViewBag.ErrorMessage = "El archivo subido no es un tipo de imagen válido.";
                        return View(model);
                    }

                    // Generar un nombre único para la nueva imagen
                    var nombreArchivo = $"{Guid.NewGuid()}{extensionArchivo}";
                    var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagen");
                    if (!Directory.Exists(rutaCarpeta))
                    {
                        Directory.CreateDirectory(rutaCarpeta);
                    }

                    var rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        Imagen.CopyTo(stream);
                    }

                    // Eliminar la imagen anterior si existe
                    if (!string.IsNullOrEmpty(imagenAnterior))
                    {
                        var rutaImagenAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagenAnterior.TrimStart('/'));
                        if (System.IO.File.Exists(rutaImagenAnterior))
                        {
                            System.IO.File.Delete(rutaImagenAnterior);
                        }
                    }

                    // Asignar la nueva ruta de la imagen
                    model.Imagen = $"/imagen/{nombreArchivo}";
                }
                else
                {
                    // Mantener la imagen anterior si no se carga una nueva
                    model.Imagen = imagenAnterior ?? string.Empty; // Valor predeterminado
                }

                // Crear los parámetros para el procedimiento almacenado de actualización
                var parametros = new
                {
                    UsuarioId = usuarioId,
                    model.Nombre,
                    model.Apellidos,
                    model.Telefono,
                    model.Direccion,
                    model.Imagen
                };

                // Actualizar el perfil en la base de datos
                using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    var result = connection.ExecuteScalar<int>("ActualizarPerfilUsuario", parametros, commandType: CommandType.StoredProcedure);

                    if (result == 0)
                    {
                        // Si el nombre cambió, agregar un mensaje a ViewBag
                        if (!string.Equals(username, model.Nombre, StringComparison.OrdinalIgnoreCase))
                        {
                            ViewBag.NameChangedMessage = "Tu nombre ha cambiado, se cerrará sesión en 3 segundos";
                            return View(model);
                        }

                        return RedirectToAction("Perfil", "Usuario");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se pudo actualizar el perfil. Intente nuevamente.";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                ViewBag.ErrorMessage = $"Ocurrió un error inesperado, Vuelva a intentarlo más tarde";
                return View(model ?? new UsuarioPerfil()); // Asegurar que el modelo no sea null
            }
        }


    }
}
