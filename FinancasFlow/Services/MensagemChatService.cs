
using FinancasFlow.Data;
using FinancasFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancasFlow.Services
{
    public class MensagemChatService
    {
        private readonly IDbContextFactory<FinancasFlowDbContext> _contextFactory;

        public MensagemChatService(
            IDbContextFactory<FinancasFlowDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task SalvarMensagemAsync(MensagemChat mensagem)
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            context.MensagensChat.Add(mensagem);

            await context.SaveChangesAsync();
        }

        public async Task<List<MensagemChat>> ObterMensagensAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            return await context.MensagensChat
                .AsNoTracking()
                .OrderBy(m => m.DataEnvio)
                .ThenBy(m => m.Id)
                .ToListAsync();
        }
    }
}
