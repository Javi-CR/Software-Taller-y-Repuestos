using Microsoft.AspNetCore.Mvc;
using Software_Taller_y_Repuestos.Models;
using System.Diagnostics;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;


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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario model)
        {
            return View();
        }

    }
}
