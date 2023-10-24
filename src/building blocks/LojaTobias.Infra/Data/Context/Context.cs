using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Infra.Data
{
    public class Context : IdentityDbContext
    {
        protected Context()
        {
                
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(1000)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);

            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            base.OnModelCreating(modelBuilder);
        }
    }
}
