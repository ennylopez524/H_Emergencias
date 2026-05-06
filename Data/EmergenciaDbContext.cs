using Microsoft.EntityFrameworkCore;
using H_Emergencias.Models;

namespace H_Emergencias.Data
{
    public class EmergenciaDbContext : DbContext
    {
        public EmergenciaDbContext(DbContextOptions<EmergenciaDbContext> options)
            : base(options) 
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Atencion> Atenciones { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
        public DbSet<MisionAmbulancia> MisionesAmbulancia { get; set; }
    }
}