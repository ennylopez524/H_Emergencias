using Microsoft.AspNetCore.Mvc;
using H_Emergencias.Data;
using System;
using System.Linq;

namespace H_Emergencias.Controllers
{
    [ApiController]
    [Route("emergencias-upds/api/alertas")]
    public class AlertaController : ControllerBase
    {
        private readonly EmergenciaDbContext _context;

        public AlertaController(EmergenciaDbContext context)
        {
            _context = context;
        }

        // 🔷 1. Crear alerta desde Emergencias
        [HttpPost("emergencia")]
        public IActionResult CrearAlerta(Models.Alerta a)
        {
            var pacienteExiste = _context.Pacientes
                .Any(p => p.Codigo == a.CodigoPaciente && p.Estado);

            if (!pacienteExiste)
                return BadRequest("Paciente no existe");

            a.Estado = true;
            a.Fecha = DateTime.UtcNow;

            _context.Alertas.Add(a);
            _context.SaveChanges();

            return Ok(new { mensaje = "Alerta enviada a todos los servicios" });
        }

        // 🔷 2. Ver alertas activas
        [HttpGet]
        public IActionResult Get()
        {
            var data = from a in _context.Alertas
                       where a.Estado
                       select new
                       {
                           a.Codigo,
                           a.Tipo,
                           a.Mensaje,
                           a.Fecha
                       };

            return Ok(data);
        }

        // 🔷 3. Alertas por tipo (MIS)
        [HttpGet("tipo/{tipo}")]
        public IActionResult PorTipo(string tipo)
        {
            var data = from a in _context.Alertas
                       where a.Tipo == tipo && a.Estado
                       select new
                       {
                           a.Codigo,
                           a.Mensaje,
                           a.Fecha
                       };

            return Ok(data);
        }

        // 🔷 4. Conteo de alertas por tipo (MIS)
        [HttpGet("resumen")]
        public IActionResult Resumen()
        {
            var data = from a in _context.Alertas
                       where a.Estado
                       group a by a.Tipo into g
                       select new
                       {
                           Tipo = g.Key,
                           Total = g.Count()
                       };

            return Ok(data);
        }
    }
}