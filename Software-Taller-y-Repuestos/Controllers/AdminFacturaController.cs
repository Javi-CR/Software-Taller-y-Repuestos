using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Software_Taller_y_Repuestos.Models;

[Authorize(Roles = "Admin")]
public class AdminFacturaController : Controller
{
    private readonly IConfiguration _config;

    public AdminFacturaController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<FacturaViewModel> facturas;

        using (var connection = new SqlConnection(_config.GetSection("ConnectionStrings:DefaultConnection").Value))
        {
            facturas = await connection.QueryAsync<FacturaViewModel>(
                "ObtenerTodasLasFacturas",
                commandType: CommandType.StoredProcedure);
        }

        return View(facturas);
    }

    [HttpPost]
    public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
    {
        if (string.IsNullOrEmpty(nuevoEstado) || (nuevoEstado != "En revisión" && nuevoEstado != "Aprobado" && nuevoEstado != "Rechazado"))
        {
            TempData["Error"] = "Estado inválido.";
            return RedirectToAction("Index");
        }

        using (var connection = new SqlConnection(_config.GetSection("ConnectionStrings:DefaultConnection").Value))
        {
            var affectedRows = await connection.ExecuteAsync(
                "ActualizarEstadoFactura",
                new { FacturaId = id, NuevoEstado = nuevoEstado },
                commandType: CommandType.StoredProcedure);

            if (affectedRows == 0)
            {
                TempData["Error"] = "No se pudo actualizar el estado de la factura.";
            }
        }

        TempData["Success"] = "Estado actualizado correctamente.";
        return RedirectToAction("Index");
    }
}
