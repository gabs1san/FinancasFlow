
using FinancasFlow.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancasFlow.Services
{
    public class BancoDadosInicializador
    {
        private readonly IDbContextFactory<FinancasFlowDbContext>
            _contextFactory;

        private readonly CartaoService _cartaoService;

        public BancoDadosInicializador(
            IDbContextFactory<FinancasFlowDbContext> contextFactory,
            CartaoService cartaoService)
        {
            _contextFactory = contextFactory;
            _cartaoService = cartaoService;
        }

        public async Task InicializarAsync()
        {
            await using (var context =
                await _contextFactory.CreateDbContextAsync())
            {
                // Aplica as migrations pendentes
                await context.Database.MigrateAsync();
            }

            // Garante que o cartão de teste exista
            await _cartaoService.GarantirCartaoTesteAsync();
        }
    }
}
