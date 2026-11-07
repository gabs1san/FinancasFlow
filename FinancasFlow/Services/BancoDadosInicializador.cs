
using FinancasFlow.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancasFlow.Services
{
    public class BancoDadosInicializador
    {
        private readonly IDbContextFactory<FinancasFlowDbContext> _contextFactory;

        public BancoDadosInicializador(
            IDbContextFactory<FinancasFlowDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task InicializarAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            await context.Database.MigrateAsync();
        }
    }
}
