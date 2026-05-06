using Microsoft.AspNetCore.Mvc;
using H_Emergencias.Data;
using System.Linq;

namespace H_Emergencias.Controllers
{
    [ApiController]
    [Route("emergencias-upds/api/pacientes")]
    public class PacienteController : ControllerBase
    {
        private readonly EmergenciaDbContext _context;

        public PacienteController(EmergenciaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = from p in _context.Pacientes
                       where p.Estado
                       select new { p.Codigo, p.Nombre, p.Edad };

            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post(Models.Paciente p)
        {
            if (_context.Pacientes.Any(x => x.Codigo == p.Codigo))
                return BadRequest("Código duplicado");

            p.Estado = true;
            _context.Pacientes.Add(p);
            _context.SaveChanges();

            return Ok();
        }

        // MIS 1: Pacientes sin atención
        [HttpGet("sin-atencion")]
        public IActionResult SinAtencion()
        {
            var data = from p in _context.Pacientes
                       where !_context.Atenciones
                           .Any(a => a.CodigoPaciente == p.Codigo && a.Estado)
                       select new { p.Codigo, p.Nombre };

            return Ok(data);
        }
    }
}