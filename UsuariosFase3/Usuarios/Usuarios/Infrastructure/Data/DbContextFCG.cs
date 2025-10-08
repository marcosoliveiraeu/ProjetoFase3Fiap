namespace Usuarios.Infrastructure.Data
{
    using Usuarios.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DbContextFCG : DbContext
    {
        public DbContextFCG(DbContextOptions<DbContextFCG> options) : base(options) { }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapear enums como string
            modelBuilder.Entity<Usuario>()
                .Property(e => e.Perfil)
                .HasConversion<string>();

        }
    }

}
