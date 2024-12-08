using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Software_Taller_y_Repuestos.Models;
using System.Data;
using System.Security.Claims;

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
            // Recuperar el UsuarioID desde los claims
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return RedirectToAction("Logout", "Home"); // Redirigir si no está autenticado
            }

            var usuarioId = int.Parse(userIdClaim.Value);

            using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var usuario = connection.QueryFirstOrDefault<Usuario>(
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
                var usuario = connection.QueryFirstOrDefault<Usuario>(
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
        public IActionResult EditarPerfil(Usuario model, IFormFile Imagen)
        {
            try
            {
                // Recuperar el UsuarioID desde los claims
                var userIdClaim = User.FindFirst("UserId");
                if (userIdClaim == null)
                {
                    return RedirectToAction("Logout", "Home"); 
                }

                var usuarioId = int.Parse(userIdClaim.Value);

                string imagenAnterior;

              
                using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    var usuario = connection.QueryFirstOrDefault<Usuario>(
                        "ConsultarPerfilUsuario",
                        new { UsuarioID = usuarioId },
                        commandType: CommandType.StoredProcedure);

                    if (usuario == null)
                    {
                        ViewBag.ErrorMessage = "Usuario no encontrado.";
                        return View(model);
                    }

                
                    imagenAnterior = usuario.Imagen;
                }

                // Procesar la nueva imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    // Generar un nombre único para la nueva imagen
                    var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(Imagen.FileName)}";

                    // Ruta para guardar la nueva imagen
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

                    // Actualizar el modelo con la nueva ruta de la imagen
                    model.Imagen = $"/imagen/{nombreArchivo}";
                }
                else
                {
                    // Mantener la imagen anterior si no se carga una nueva
                    model.Imagen = imagenAnterior;
                }

                // Crear los parámetros para el procedimiento almacenado de actualización
                var parametros = new
                {
                    usuarioId,
                    model.Nombre,
                    model.Apellidos,
                    model.Telefono,
                    model.Direccion,
                    model.Imagen 
                };


                // Actualizar los claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Nombre),
                    new Claim(ClaimTypes.Email, User.FindFirst(ClaimTypes.Email)?.Value),
                    new Claim("UserId", usuarioId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties).Wait();


                // Llamar al procedimiento almacenado para actualizar el perfil
                using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    var result = connection.Execute("ActualizarPerfilUsuario", parametros, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
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
                // Manejar errores y registrar detalles
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult PerfilUI()
        {
            return View();

        }



    }
}
