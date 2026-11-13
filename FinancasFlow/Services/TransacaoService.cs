
using FinancasFlow.Data;
using FinancasFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancasFlow.Services;

public class TransacaoService
{
    private readonly IDbContextFactory<FinancasFlowDbContext> _contextFactory;

    public TransacaoService(
        IDbContextFactory<FinancasFlowDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // Registrar uma receita ou despesa
    public async Task AdicionarAsync(Transacao transacao)
    {
        if (transacao is null)
            throw new ArgumentNullException(nameof(transacao));

        if (transacao.Valor <= 0)
            throw new ArgumentException(
                "O valor deve ser maior que zero.");

        if (transacao.Tipo != "Receita" &&
            transacao.Tipo != "Despesa")
            throw new ArgumentException(
                "O tipo deve ser Receita ou Despesa.");

        if (string.IsNullOrWhiteSpace(transacao.Categoria))
            throw new ArgumentException(
                "Informe uma categoria.");

        if (transacao.Data == default)
            transacao.Data = DateTime.Now;

        await using var context =
            await _contextFactory.CreateDbContextAsync();

        context.Transacoes.Add(transacao);

        await context.SaveChangesAsync();
    }

    // Consultar todas as transações
    public async Task<List<Transacao>> ListarAsync()
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Transacoes
            .AsNoTracking()
            .OrderByDescending(t => t.Data)
            .ToListAsync();
    }

    // Consultar as transações de um mês
    public async Task<List<Transacao>> ListarPorMesAsync(
        int ano, int mes)
    {
        var inicio = new DateTime(ano, mes, 1);
        var fim = inicio.AddMonths(1);

        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Transacoes
            .AsNoTracking()
            .Where(t => t.Data >= inicio && t.Data < fim)
            .OrderByDescending(t => t.Data)
            .ToListAsync();
    }



    public async Task<ResumoFinanceiro> ObterResumoMensalAsync(
        int ano, int mes)
    {
        var transacoes = await ListarPorMesAsync(ano, mes);

        // Todas as receitas do mês
        var receitas = transacoes
            .Where(t => t.Tipo == "Receita")
            .Sum(t => t.Valor);

        // Apenas despesas que saíram do dinheiro disponível
        var despesasPagas = transacoes
            .Where(t =>
                t.Tipo == "Despesa" &&
                t.FormaPagamento != "Crédito")
            .ToList();

        var despesas = despesasPagas
            .Sum(t => t.Valor);

        // Categorias das despesas efetivamente pagas
        var categorias = despesasPagas
            .GroupBy(t => t.Categoria)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Sum(t => t.Valor)
            );

        return new ResumoFinanceiro
        {
            TotalReceitas = receitas,
            TotalDespesas = despesas,
            DespesasPorCategoria = categorias
        };
    }


    public async Task<List<ReceitaExtratoItem>>
    ListarReceitasPorMesAsync(int ano, int mes)
    {
        var transacoes = await ListarPorMesAsync(ano, mes);

        return transacoes
            .Where(t => t.Tipo == "Receita")
            .OrderByDescending(t => t.Data)
            .ThenByDescending(t => t.Id)
            .Select(t => new ReceitaExtratoItem
            {
                Id = t.Id,
                Descricao = t.Descricao,
                Categoria = t.Categoria,
                Valor = t.Valor,
                Data = t.Data
            })
            .ToList();
    }

}
