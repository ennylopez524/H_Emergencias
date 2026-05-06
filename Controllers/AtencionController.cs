using Microsoft.AspNetCore.Mvc;
using H_Emergencias.Data;
using System;
using System.Linq;

namespace H_Emergencias.Controllers
{
    [ApiController]
    [Route("emergencias-upds/api/atenciones")]
    public class AtencionController : ControllerBase
    {
        private readonly EmergenciaDbContext _context;

        public AtencionController(EmergenciaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = from a in _context.Atenciones
                       where a.Estado
                       select new { a.Codigo, a.Fecha, a.Medico };

            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post(Models.Atencion a)
        {
            a.Estado = true;
            _context.Atenciones.Add(a);
            _context.SaveChanges();
            return Ok();
        }

        // MIS 2: Historial por paciente
        [HttpGet("historial/{codigo}")]
        public IActionResult Historial(string codigo)
        {
            var data = from a in _context.Atenciones
                       where a.CodigoPaciente == codigo && a.Estado
                       select new { a.Fecha, a.Medico };

            return Ok(data);
        }

        // MIS 3: Atenciones por médico (GROUP BY COUNT)
        [HttpGet("por-medico")]
        public IActionResult PorMedico()
        {
            var data = from a in _context.Atenciones
                       where a.Estado
                       group a by a.Medico into g
                       select new { Medico = g.Key, Total = g.Count() };

            return Ok(data);
        }

        // MIS 4: Pacientes atendidos hoy
        [HttpGet("hoy")]
        public IActionResult Hoy()
        {
            var hoy = DateTime.Today;

            var data = from a in _context.Atenciones
                       join p in _context.Pacientes
                       on a.CodigoPaciente equals p.Codigo
                       where a.Fecha.Date == hoy && a.Estado
                       select new { p.Nombre, a.Medico };

            return Ok(data);
        }

    }
}