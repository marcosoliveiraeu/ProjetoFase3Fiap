using Jogos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Infrastructure.Data
{
    public class DbContextFCG : DbContext
    {
        public DbContextFCG(DbContextOptions<DbContextFCG> options) : base(options) { }

        // DbSets

        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<JogosUsuario> JogosUsuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Jogo>()
                .Property(e => e.Categoria)
                .HasConversion<string>();
    
        }
    }

}
