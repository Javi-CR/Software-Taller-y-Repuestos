using Microsoft.AspNetCore.Mvc;
using Software_Taller_y_Repuestos.Models;
using System.Diagnostics;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace Software_Taller_y_Repuestos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TallerRepuestosDbContext _context;

        public HomeController(ILogger<HomeController> logger, TallerRepuestosDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SubmitForm(Usuario usuario)
        {

            if (usuario != null)
            {
                // Asignar la fecha de ingreso
                usuario.FechaIngreso = DateTime.Now;
                usuario.Contrasenna = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenna);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return View("Login");
            }
            return Content("<a>Error en la operacion</a>");
        }


        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string Correo, string Contrasenna)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == Correo);

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
