using Microsoft.AspNetCore.Mvc;
using H_Emergencias.Data;
using System;
using System.Linq;

namespace H_Emergencias.Controllers
{
    [ApiController]
    [Route("emergencias-upds/api/ambulancia")]
    public class AmbulanciaController : ControllerBase
    {
        private readonly EmergenciaDbContext _context;

        public AmbulanciaController(EmergenciaDbContext context)
        {
            _context = context;
        }

        // 🔷 Crear Orden de Misión (desde Emergencias)
        [HttpPost("orden")]
        public IActionResult CrearOrden(Models.MisionAmbulancia m)
        {
            var existeAtencion = _context.Atenciones
                .Any(a => a.Codigo == m.CodigoAtencion && a.Estado);

            if (!existeAtencion)
                return BadRequest("La atención no existe");

            m.Estado = true;
            m.FechaSalida = DateTime.UtcNow;

            _context.MisionesAmbulancia.Add(m);
            _context.SaveChanges();

            return Ok(new { mensaje = "Orden de misión creada" });
        }

        // 🔷 Registrar ETA (Ambulancia → Emergencias)
        [HttpPut("eta/{codigo}")]
        public IActionResult RegistrarETA(string codigo, DateTime eta)
        {
            var mision = _context.MisionesAmbulancia
                .FirstOrDefault(x => x.Codigo == codigo && x.Estado);

            if (mision == null)
                return NotFound();

            mision.ETA = eta;
            _context.SaveChanges();

            return Ok(new { mensaje = "ETA registrado" });
        }

        // 🔷 MIS: Ver misiones activas con ETA
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
                           m.FechaSalida,
                           m.ETA,
                           a.Medico
                       };

            return Ok(data);
        }

        // 🔷 MIS: Misiones sin ETA (crítico)
        [HttpGet("sin-eta")]
        public IActionResult SinETA()
        {
            var data = from m in _context.MisionesAmbulancia
                       where m.ETA == null && m.Estado
                       select new
                       {
                           m.Codigo,
                           m.NivelGravedad,
                           m.FechaSalida
                       };

            return Ok(data);
        }
    }
}