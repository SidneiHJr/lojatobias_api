using LojaTobias.Infra.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Infra.Data
{
    public class Context : IdentityDbContext<AspnetUserExtension>
    {
        protected Context()
        {
                
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Usado para configurar as tabelas do identity do tipo string para varchar(1000) em vez da padrao
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(1000)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);

            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            base.OnModelCreating(modelBuilder);
        }
    }
}
