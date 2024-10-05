using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Software_Taller_y_Repuestos.Controllers
{
    public class ModelosAutoController : Controller
    {
        // GET: ModelosAutoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ModelosAutoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModelosAutoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModelosAutoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ModelosAutoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModelosAutoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ModelosAutoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModelosAutoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
