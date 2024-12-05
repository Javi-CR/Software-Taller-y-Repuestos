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


        // Método para iniciar sesión con Google
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Home", new { ReturnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }


        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/");

                // Obtener la información de autenticación del proveedor externo
                var info = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (info == null)
                {
                    throw new Exception("Hubo un problema al autenticar con el proveedor externo.");
                }

                // Obtener las claims de email, nombre y foto
                var emailClaim = info.Principal?.FindFirst(ClaimTypes.Email);
                var nameClaim = info.Principal?.FindFirst(ClaimTypes.Name);
                var pictureClaim = info.Principal?.FindFirst("picture");

                if (emailClaim == null)
                {
                    throw new Exception("No se pudo obtener el correo del proveedor externo.");
                }

                // Buscar si el usuario ya existe
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == emailClaim.Value);

                // Si el usuario no existe, crearlo automáticamente
                if (user == null)
                {
                    user = new Usuario
                    {
                        Nombre = nameClaim?.Value ?? "Usuario Externo",
                        Correo = emailClaim.Value,
                        Contrasenna = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()), // Generar una contraseña aleatoria
                        RolId = 2, 
                        FechaIngreso = DateTime.Now,
                        Imagen = pictureClaim?.Value 
                    };

                    // Agregar el nuevo usuario a la base de datos
                    _context.Usuarios.Add(user);
                    await _context.SaveChangesAsync();
                }

                // Crear las claims para iniciar sesión
                var claimsIdentity = new ClaimsIdentity(info.Principal.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Redirigir a la página principal (Index)
                return RedirectToAction("Index", "Home");
            }
            catch (AuthenticationFailureException ex)
            {
                // Si el usuario cancela la autenticación, lo redirigimos a la página de login
                return RedirectToAction(nameof(Login), new { errorMessage = "El acceso fue denegado o se canceló el proceso de autenticación." });
            }
            catch (Exception ex)
            {
                // Manejar cualquier otro error
                return RedirectToAction(nameof(Login), new { errorMessage = ex.Message });
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
