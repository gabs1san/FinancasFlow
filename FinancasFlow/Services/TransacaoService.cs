
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

    // Consultar transações de um mês
    public async Task<List<Transacao>> ListarPorMesAsync(
        int ano,
        int mes)
    {
        var inicio = new DateTime(ano, mes, 1);
        var fim = inicio.AddMonths(1);

        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Transacoes
            .AsNoTracking()
            .Where(t =>
                t.Data >= inicio &&
                t.Data < fim)
            .OrderByDescending(t => t.Data)
            .ToListAsync();
    }

    // Calcular resumo mensal
    public async Task<ResumoFinanceiro> ObterResumoMensalAsync(
        int ano,
        int mes)
    {
        var transacoes = await ListarPorMesAsync(ano, mes);


        foreach (var compra in comprasCredito)
        {
            System.Diagnostics.Debug.WriteLine(
                $"ID: {compra.Id} | " +
                $"Valor: {compra.Valor:C} | " +
                $"Cartão ID: {compra.CartaoCreditoId?.ToString() ?? "SEM CARTÃO"} | " +
                $"Descrição: {compra.Descricao}"
            );
        }


        // Todas as receitas
        var receitas = transacoes
            .Where(t => t.Tipo == "Receita")
            .Sum(t => t.Valor);

        // Despesas que realmente reduziram o dinheiro
        var despesasPagas = transacoes
            .Where(t =>
                t.Tipo == "Despesa" &&
                t.FormaPagamento != "Crédito")
            .ToList();

        var despesas = despesasPagas
            .Sum(t => t.Valor);

        // Despesas pagas por categoria
        var categorias = despesasPagas
            .GroupBy(t => t.Categoria)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Sum(t => t.Valor)
            );

        // Todas as compras, incluindo crédito
        var despesasPorPagamento = transacoes
            .Where(t => t.Tipo == "Despesa")
            .GroupBy(t => t.FormaPagamento)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Sum(t => t.Valor)
            );

        return new ResumoFinanceiro
        {
            TotalReceitas = receitas,
            TotalDespesas = despesas,
            DespesasPorCategoria = categorias,
            DespesasPorPagamento = despesasPorPagamento
        };
    }

    // Extrato de receitas do mês
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
