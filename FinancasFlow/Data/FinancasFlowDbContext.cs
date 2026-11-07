
using FinancasFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancasFlow.Data
{
    public class FinancasFlowDbContext : DbContext
    {
        public FinancasFlowDbContext(
            DbContextOptions<FinancasFlowDbContext> options)
            : base(options)
        {
        }

        // Tabelas do banco de dados

        public DbSet<Transacao> Transacoes { get; set; }

        public DbSet<CartaoDeCredito> Cartoes { get; set; }

        public DbSet<UsuarioFinanceiro> Usuarios { get; set; }

        public DbSet<MensagemChat> MensagensChat { get; set; }


        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento entre cartão e transações

            modelBuilder.Entity<Transacao>()
                .HasOne<CartaoDeCredito>()
                .WithMany()
                .HasForeignKey(t => t.CartaoCreditoId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
