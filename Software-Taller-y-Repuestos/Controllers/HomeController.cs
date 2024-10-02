using Microsoft.AspNetCore.Mvc;
using Software_Taller_y_Repuestos.Models;
using System.Diagnostics;

namespace Software_Taller_y_Repuestos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

       

    }
}
