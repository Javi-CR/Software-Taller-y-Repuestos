using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Software_Taller_y_Repuestos.Models;

namespace Software_Taller_y_Repuestos.Controllers
{
   [Authorize(Roles = "Admin")]
    public class HR : Controller
    {

        private readonly TallerRepuestosDbContext _context;
        private readonly IConfiguration _conf;

        public HR(TallerRepuestosDbContext context, IConfiguration conf)
        {
            _context = context;
            _conf = conf;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Empleados()
        {
            // Obtén todos los usuarios con el rol de "Empleado"
            var empleados = _context.Usuarios
                              .Include(u => u.Rol)
                              .Where(u => u.Rol.NombreRol == "Empleado")
                              .ToList();

            return View(empleados);
        }


        [HttpGet]
        public IActionResult AgregarEmpleados()
        {
            // Filtra los usuarios con el rol de "Cliente"
            var clientes = _context.Usuarios
                           .Where(u => u.Rol.NombreRol == "Cliente")
                           .ToList();

            return View(clientes);
        }


        [HttpPost]
        public IActionResult AsignarRolEmpleado(int usuarioId)
        {
            // Busca al usuario por su ID
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

                
                usuario.RolId = 3; // Asigna el rol que quiera
                _context.SaveChanges();

            return RedirectToAction("Empleados");
        }


        [HttpPost]
        public IActionResult AsignarRolCliente(int usuarioId)
        {
            // Busca al usuario por su ID
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

           
            usuario.RolId = 2; // Asigna el rol que quiera
            _context.SaveChanges();

            return RedirectToAction("AgregarEmpleados");

        }

        [HttpGet]
        public IActionResult EditarInformacion(int usuarioId)
        {
            // Busca el usuario en la base de datos
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

            if (usuario == null)
            {
                // Si no encuentra el usuario, retorna a empleados
                ViewBag.ErrorMessage = "Usuario no encontrado en la base de datos.";
                return RedirectToAction("Empleados");
            }

            // Carga el usuario en la vista
            return View(usuario);

        }

        [HttpPost]
        public IActionResult EditarInformacion(Usuario usuario, int usuarioId)
        {
            
            if (usuario.UsuarioId == 0)
            {
                usuario.UsuarioId = usuarioId;
            }

            var usuarioDb = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuario.UsuarioId);

            if (usuarioDb == null)
            {
                ViewBag.ErrorMessage = "Usuario no encontrado en la base de datos.";
                return View("DatosPersonales", usuario);
            }

            // Actualiza los datos del usuario
            usuarioDb.Telefono = usuario.Telefono;
            usuarioDb.Direccion = usuario.Direccion;
            usuarioDb.SalarioBase = usuario.SalarioBase;

            _context.SaveChanges();

            return RedirectToAction("Empleados");
        }


        [HttpGet]
        public IActionResult GestionHorarios(int usuarioId)
        {
            // Busca al empleado por su ID, incluyendo sus horarios
            var empleado = _context.Usuarios
                                   .Include(u => u.Horarios) 
                                   .FirstOrDefault(u => u.UsuarioId == usuarioId);

            if (empleado == null)
            {
                ViewBag.ErrorMessage = "Empleado no encontrado.";
                return RedirectToAction("Empleados");
            }

            return View(empleado);
        }


        [HttpGet]
        public IActionResult EditarHorario(int usuarioId, int? horarioId = null)
        {
            Horario horario;

            if (horarioId.HasValue)
            {
                // Si se proporciona un horarioId, busca el horario existente
                horario = _context.Horarios.FirstOrDefault(h => h.HorarioId == horarioId.Value);

                if (horario == null)
                {
                    ViewBag.ErrorMessage = "Horario no encontrado.";
                    return RedirectToAction("GestionHorarios", new { usuarioId = usuarioId });
                }
            }
            else
            {
                // Si no se proporciona horarioId, crea un nuevo horario para el usuario
                horario = new Horario { UsuarioId = usuarioId };
            }

            return View(horario);
        }

        [HttpPost]
        public IActionResult EditarHorario(Horario horario)
        {
            try
            {
                if (horario.HorarioId == 0)
                {
                    // Si el horario no tiene ID, es nuevo y se agrega
                    _context.Horarios.Add(horario);
                }
                else
                {
                    // Si ya existe el horario, se actualiza
                    var horarioDb = _context.Horarios.FirstOrDefault(h => h.HorarioId == horario.HorarioId);
                    if (horarioDb != null)
                    {
                        horarioDb.Fecha = horario.Fecha;
                        horarioDb.HorasTrabajadas = horario.HorasTrabajadas;
                        horarioDb.HorasExtras = horario.HorasExtras;
                        horarioDb.Ausencias = horario.Ausencias;
                        horarioDb.Permisos = horario.Permisos;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se encontró el horario en la base de datos.";
                        return View(horario); 
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("GestionHorarios", new { usuarioId = horario.UsuarioId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al guardar el horario: " + ex.Message;
                return RedirectToAction("GestionHorarios", new { usuarioId = horario.UsuarioId });
            }
        }



    }
}
