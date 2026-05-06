using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace H_Emergencias.Data
{
    public class EmergenciaDbContextFactory : IDesignTimeDbContextFactory<EmergenciaDbContext>
    {
        public EmergenciaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EmergenciaDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=H_Emergencias;Username=postgres;Password=1234");

            return new EmergenciaDbContext(optionsBuilder.Options);
        }
    }
}