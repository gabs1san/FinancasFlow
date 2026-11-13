
using FinancasFlow.Data;
using FinancasFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancasFlow.Services;

public class CartaoService
{
    private readonly IDbContextFactory<FinancasFlowDbContext>
        _contextFactory;

    public CartaoService(
        IDbContextFactory<FinancasFlowDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    
    public async Task<List<CartaoDeCredito>> ListarAsync()
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Cartoes
            .AsNoTracking()
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }


    public async Task<CartaoDeCredito> GarantirCartaoTesteAsync()
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        // Verifica se o cartão de teste já existe
        var cartaoExistente = await context.Cartoes
            .FirstOrDefaultAsync(c => c.Nome == "Meu Cartão");

        if (cartaoExistente != null)
            return cartaoExistente;

        // Cria um cartão fictício
        var cartao = new CartaoDeCredito
        {
            Nome = "Meu Cartão",
            Limite = 3000m,
            DiaFechamento = 25,
            DiaVencimento = 5
        };

        context.Cartoes.Add(cartao);

        await context.SaveChangesAsync();

        return cartao;
    }


    public async Task<CartaoDeCredito> AdicionarAsync(
        CartaoDeCredito cartao)
    {
        if (string.IsNullOrWhiteSpace(cartao.Nome))
            throw new ArgumentException(
                "O nome do cartão é obrigatório.");

        if (cartao.Limite <= 0)
            throw new ArgumentException(
                "O limite deve ser maior que zero.");

        if (cartao.DiaFechamento < 1 ||
            cartao.DiaFechamento > 31)
            throw new ArgumentException(
                "Dia de fechamento inválido.");

        if (cartao.DiaVencimento < 1 ||
            cartao.DiaVencimento > 31)
            throw new ArgumentException(
                "Dia de vencimento inválido.");

        await using var context =
            await _contextFactory.CreateDbContextAsync();

        context.Cartoes.Add(cartao);

        await context.SaveChangesAsync();

        return cartao;
    }
}
