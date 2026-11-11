
using FinancasFlow.Models;
using FinancasFlow.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FinancasFlow.ViewModels;

public class ResumoViewModel
{
    private readonly TransacaoService _transacaoService;

    private readonly CultureInfo _cultura =
        CultureInfo.GetCultureInfo("pt-BR");

    // Valores exibidos no dashboard
    public string Saldo { get; private set; } = "R$ 0,00";
    public string Receitas { get; private set; } = "R$ 0,00";
    public string Despesas { get; private set; } = "R$ 0,00";

    public string MesReferencia { get; private set; } = "";

    // Lista de despesas agrupadas por categoria
    public ObservableCollection<CategoriaResumo>
        Categorias
    { get; } = new();

    // Lista detalhada de receitas
    public ObservableCollection<ReceitaExtratoItem>
        ExtratoReceitas
    { get; } = new();

    public ResumoViewModel(TransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    public async Task CarregarAsync()
    {
        var hoje = DateTime.Today;

        // Busca o resumo financeiro do mês
        var resumo = await _transacaoService
            .ObterResumoMensalAsync(hoje.Year, hoje.Month);

        // Busca as receitas individuais do mês
        var receitas = await _transacaoService
            .ListarReceitasPorMesAsync(hoje.Year, hoje.Month);

        // Atualiza os indicadores financeiros
        Saldo = resumo.Saldo.ToString("C", _cultura);
        Receitas = resumo.TotalReceitas.ToString("C", _cultura);
        Despesas = resumo.TotalDespesas.ToString("C", _cultura);

        MesReferencia = hoje.ToString("MMMM yyyy", _cultura);

        // Atualiza as despesas por categoria
        Categorias.Clear();

        foreach (var categoria in resumo.DespesasPorCategoria
                     .OrderByDescending(c => c.Value))
        {
            Categorias.Add(new CategoriaResumo
            {
                Nome = categoria.Key,
                Valor = categoria.Value.ToString("C", _cultura)
            });
        }

        // Atualiza o extrato de receitas
        ExtratoReceitas.Clear();

        foreach (var receita in receitas)
        {
            ExtratoReceitas.Add(receita);
        }
    }
}

// Modelo para exibir as despesas agrupadas
public class CategoriaResumo
{
    public string Nome { get; set; } = string.Empty;

    public string Valor { get; set; } = string.Empty;
}
