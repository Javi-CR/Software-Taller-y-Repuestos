using Microsoft.AspNetCore.Mvc;
using Software_Taller_y_Repuestos.Models;
using System.Diagnostics;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Authentication.OAuth; // Para OAuth
using Microsoft.AspNetCore.Authentication.Google; // Google provider
using System.Threading.Tasks;
using System.Data;

namespace Software_Taller_y_Repuestos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TallerRepuestosDbContext _context;
        private readonly IConfiguration _conf;

        public HomeController(ILogger<HomeController> logger, TallerRepuestosDbContext context, IConfiguration conf)
        {
            _logger = logger;
            _context = context;
            _conf = conf;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(CuentaUsuario usuario)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, se regresa a la vista con los mensajes de error.
                return View(usuario);
            }

            try
            {
                using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    
                    // Definir el ID del rol predeterminado.
                    int rolID = 2;

                    // Hashear la contraseña usando BCrypt.
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenna);

                    // Ejecutar el procedimiento almacenado "CrearUsuario".
                    var result = connection.Execute(
                        "CrearUsuario",
                        new
                        {
                            usuario.Nombre,
                            usuario.Apellidos,
                            usuario.Correo,
                            Contrasenna = hashedPassword,
                            RolID = rolID
                        },
                        commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {

                        // En caso de que algo falle sin excepción.
                        ViewBag.Mensaje = "No se pudo crear la cuenta. Intente de nuevo.";
                        return View(usuario);

                    }
                    else
                    {
                        // Si la cuenta se creó correctamente.
                        ViewBag.Mensaje = "Cuenta creada exitosamente. Por favor, inicie sesión.";
                        //return RedirectToAction("Index");
                        return View(usuario);

                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50001) // Error personalizado lanzado por el procedimiento almacenado.
                {
                    ViewBag.Mensaje = ex.Message;
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrió un error inesperado al crear la cuenta.";
                }

                return View(usuario);
            }
        }


        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            try
            {
                using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    // Llamar al procedimiento almacenado
                    var usuario = connection.QueryFirstOrDefault<Login>(
                        "IniciarSesion",
                        new { model.Correo },
                        commandType: CommandType.StoredProcedure);

                    if (usuario == null)
                    {
                        ViewBag.Mensaje = "El usuario no existe.";
                        return View(model);
                    }

                    // Validar contraseña
                    if (!BCrypt.Net.BCrypt.Verify(model.Contrasenna, usuario.Contrasenna))
                    {
                        ViewBag.Mensaje = "La contraseña es incorrecta.";
                        return View(model);
                    }

                    // Crear claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Nombre),
                        new Claim(ClaimTypes.Email, usuario.Correo),
                        new Claim(ClaimTypes.Role, usuario.NombreRol),  // Usar el NombreRol del usuario
                        new Claim("UserId", usuario.UsuarioId.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Registrar autenticación
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Mensaje = "Ocurrió un error inesperado al iniciar sesión.";
                return View(model);
            }
        }


        public async Task LoginWithGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);


                // Extraer información del usuario de Google
                var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var firstName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                var lastName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                var picture = claims?.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;

                if (lastName == null)
                {
                    lastName = "Agrega un apellido, editando el perfil";
                }

                Login? usuario;

                // Registrar o actualizar al usuario en la base de datos
                using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    connection.Execute(
                        "RegistrarUsuarioGoogle",
                        new
                        {
                            Nombre = firstName,
                            Apellidos = lastName,
                            Correo = email,
                            Imagen = picture,
                            RolID = 2 // Rol predeterminado
                        },
                        commandType: CommandType.StoredProcedure
                    );


                    // Usar el procedimiento almacenado IniciarSesion para obtener los datos completos
                    usuario = connection.QueryFirstOrDefault<Login>("IniciarSesion",
                        new { Correo = email }, commandType: CommandType.StoredProcedure);

                    }

                    if (usuario == null)
                    {
                        ViewBag.Mensaje = "No se pudo autenticar al usuario.";
                        return RedirectToAction("Login");
                    }

                    // Crear claims
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Nombre),
                        new Claim(ClaimTypes.Email, usuario.Correo),
                        new Claim(ClaimTypes.Role, usuario.NombreRol),  // Usar el NombreRol del usuario
                        new Claim("UserId", usuario.UsuarioId.ToString())
                    };

                    var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Registrar autenticación
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el inicio de sesión con Google");
                ViewBag.Mensaje = "Ocurrió un error al iniciar sesión.";
                return RedirectToAction("Login");
            }
        }




        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
