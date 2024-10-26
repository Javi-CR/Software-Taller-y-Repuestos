using Microsoft.AspNetCore.Mvc;

namespace Software_Taller_y_Repuestos.Controllers
{
    public class FacturaController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
