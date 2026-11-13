
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

        public async Task<int> RemoverMensagensProcessamentoAsync()
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

            var mensagens = await context.MensagensChat
                .Where(m => m.Texto == "⏳ Processando sua mensagem...")
                .ToListAsync();

            if (mensagens.Count == 0)
                return 0;

            context.MensagensChat.RemoveRange(mensagens);

            await context.SaveChangesAsync();

            return mensagens.Count;
        }
    }
}
