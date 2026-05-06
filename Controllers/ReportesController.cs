using Microsoft.AspNetCore.Mvc;
using H_Emergencias.Data;
using System;
using System.Linq;

namespace H_Emergencias.Controllers
{
    [ApiController]
    [Route("emergencias-upds/api/reportes")]
    public class ReportesController : ControllerBase
    {
        private readonly EmergenciaDbContext _context;

        public ReportesController(EmergenciaDbContext context)
        {
            _context = context;
        }

        // 1. Atenciones por médico
        [HttpGet("atenciones-por-medico")]
        public IActionResult AtencionesPorMedico()
        {
            var data = from a in _context.Atenciones
                       where a.Estado
                       group a by a.Medico into g
                       select new { Medico = g.Key, Total = g.Count() };

            return Ok(data);
        }

        // 2. Pacientes atendidos hoy
        [HttpGet("hoy")]
        public IActionResult Hoy()
        {
            var hoy = DateTime.Today;

            var data = from a in _context.Atenciones
                       join p in _context.Pacientes
                       on a.CodigoPaciente equals p.Codigo
                       where a.Estado && a.Fecha.Date == hoy
                       select new { p.Nombre, a.Medico };

            return Ok(data);
        }

        // 3. Pacientes sin atención
        [HttpGet("sin-atencion")]
        public IActionResult SinAtencion()
        {
            var data = from p in _context.Pacientes
                       where !_context.Atenciones
                       .Any(a => a.CodigoPaciente == p.Codigo && a.Estado)
                       select new { p.Codigo, p.Nombre };

            return Ok(data);
        }

        // 4. Historial por paciente
        [HttpGet("historial/{codigo}")]
        public IActionResult Historial(string codigo)
        {
            var data = from a in _context.Atenciones
                       where a.CodigoPaciente == codigo && a.Estado
                       select new { a.Fecha, a.Medico };

            return Ok(data);
        }

        // 5. Atenciones recientes
        [HttpGet("recientes")]
        public IActionResult Recientes()
        {
            var data = from a in _context.Atenciones
                       where a.Estado
                       orderby a.Fecha descending
                       select new { a.Codigo, a.Medico, a.Fecha };

            return Ok(data);
        }

        // 6. Atenciones por día
        [HttpGet("por-dia")]
        public IActionResult PorDia()
        {
            var data = from a in _context.Atenciones
                       where a.Estado
                       group a by a.Fecha.Date into g
                       select new { Fecha = g.Key, Total = g.Count() };

            return Ok(data);
        }

        // 7. Alertas activas
        [HttpGet("alertas")]
        public IActionResult Alertas()
        {
            var data = from a in _context.Alertas
                       where a.Estado
                       select new { a.Codigo, a.Tipo, a.Mensaje, a.Fecha };

            return Ok(data);
        }

        // 8. Resumen de alertas
        [HttpGet("alertas-resumen")]
        public IActionResult AlertasResumen()
        {
            var data = from a in _context.Alertas
                       where a.Estado
                       group a by a.Tipo into g
                       select new { Tipo = g.Key, Total = g.Count() };

            return Ok(data);
        }

        // 9. Misiones de ambulancia
        [HttpGet("misiones")]
        public IActionResult Misiones()
        {
            var data = from m in _context.MisionesAmbulancia
                       join a in _context.Atenciones
                       on m.CodigoAtencion equals a.Codigo
                       where m.Estado && a.Estado
                       select new
                       {
                           m.Codigo,
                           m.NivelGravedad,
                           m.ETA,
                           a.Medico
                       };

            return Ok(data);
        }

        // 10. Misiones sin ETA
        [HttpGet("sin-eta")]
        public IActionResult SinETA()
        {
            var data = from m in _context.MisionesAmbulancia
                       where m.ETA == null && m.Estado
                       select new { m.Codigo, m.NivelGravedad };

            return Ok(data);
        }

        // 11. Carga por médico
        [HttpGet("carga-medico")]
        public IActionResult CargaMedico()
        {
            var data = from a in _context.Atenciones
                       where a.Estado
                       group a by a.Medico into g
                       select new { Medico = g.Key, Total = g.Count() };

            return Ok(data);
        }

        // 12. Atenciones por gravedad
        [HttpGet("por-gravedad")]
        public IActionResult PorGravedad()
        {
            var data = from m in _context.MisionesAmbulancia
                       where m.Estado
                       group m by m.NivelGravedad into g
                       select new { Nivel = g.Key, Total = g.Count() };

            return Ok(data);
        }

        // 13. Buscar atención por código
        [HttpGet("buscar/{codigo}")]
        public IActionResult Buscar(string codigo)
        {
            var data = from a in _context.Atenciones
                       where a.Codigo == codigo && a.Estado
                       select new { a.Codigo, a.Medico, a.Fecha };

            return Ok(data.FirstOrDefault());
        }

        // 14. Alertas recientes
        [HttpGet("alertas-recientes")]
        public IActionResult AlertasRecientes()
        {
            var data = from a in _context.Alertas
                       where a.Estado
                       orderby a.Fecha descending
                       select new { a.Tipo, a.Mensaje, a.Fecha };

            return Ok(data);
        }

        // 15. Pacientes con alertas
        [HttpGet("pacientes-alertas")]
        public IActionResult PacientesAlertas()
        {
            var data = from p in _context.Pacientes
                       join a in _context.Alertas
                       on p.Codigo equals a.CodigoPaciente
                       where p.Estado && a.Estado
                       select new { p.Nombre, a.Tipo, a.Fecha };

            return Ok(data);
        }
    }
}