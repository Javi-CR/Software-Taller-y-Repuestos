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
        public IActionResult Register(Usuario usuario)
        {
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenna);

                using (var connection = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
                {
                    string sqlQuery = @"
                    INSERT INTO Usuarios (Nombre, Apellidos, Correo, Contrasenna, Telefono, Direccion, RolID, FechaIngreso)
                    VALUES (@Nombre, @Apellidos, @Correo, @Contrasenna, @Telefono, @Direccion, @RolID, @FechaIngreso)";

                    var parameters = new
                    {
                        Nombre = usuario.Nombre,
                        Apellidos = usuario.Apellidos,
                        Correo = usuario.Correo,
                        Contrasenna = hashedPassword,
                        Telefono = string.IsNullOrEmpty(usuario.Telefono) ? null : usuario.Telefono,
                        Direccion = string.IsNullOrEmpty(usuario.Direccion) ? null : usuario.Direccion,
                        RolID = 2,
                        FechaIngreso = DateTime.Now
                    };

                    int rowsAffected = connection.Execute(sqlQuery, parameters);

                    if (rowsAffected > 0)
                    {
                        ViewBag.Mensaje = "Usuario registrado exitosamente.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Mensaje = "Ocurrió un error al registrar el usuario.";
                    }
                }
            }
            catch
            {
                ViewBag.Mensaje = "Este Usuario ya existe.";
            }

            return View(usuario);
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string Correo, string Contrasenna)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == Correo);

            if (user != null && BCrypt.Net.BCrypt.Verify(Contrasenna, user.Contrasenna))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nombre),
                    new Claim(ClaimTypes.Email, user.Correo),
                    new Claim("UserId", user.UsuarioId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
            return View();
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
