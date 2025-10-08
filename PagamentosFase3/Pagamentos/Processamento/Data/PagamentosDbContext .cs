using Microsoft.EntityFrameworkCore;
using Pagamentos.Domain.Entities;

namespace Pagamentos.Infrastructure.Data
{
    public class PagamentosDbContext : DbContext
    {
        public PagamentosDbContext(DbContextOptions<PagamentosDbContext> options)
            : base(options) { }

        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração de Pagamento
            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Valor).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Status).HasConversion<int>();
                entity.Property(p => p.MetodoPagamento).HasConversion<int>();

                entity.HasMany(p => p.Transacoes)
                      .WithOne(t => t.Pagamento)
                      .HasForeignKey(t => t.PagamentoId);
            });

            // Configuração de Transacao
            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.HasKey(t => t.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
