
using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using FinancasFlow.Models;
using FinancasFlow.Services;

namespace FinancasFlow.ViewModels;

public partial class ResumoViewModel : ObservableObject
{
    private readonly TransacaoService _transacaoService;

    private readonly CultureInfo _cultura =
        CultureInfo.GetCultureInfo("pt-BR");

    [ObservableProperty]
    private string saldo = "R$ 0,00";

    [ObservableProperty]
    private string receitas = "R$ 0,00";

    [ObservableProperty]
    private string despesas = "R$ 0,00";

    [ObservableProperty]
    private string mesReferencia = string.Empty;

    public ObservableCollection<CategoriaResumo> Categorias
    { get; } = new();

    public ObservableCollection<ReceitaExtratoItem> ExtratoReceitas
    { get; } = new();

    public ResumoViewModel(TransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    public async Task CarregarAsync()
    {
        var hoje = DateTime.Today;

        var resumo = await _transacaoService
            .ObterResumoMensalAsync(hoje.Year, hoje.Month);

        var receitasDoMes = await _transacaoService
            .ListarReceitasPorMesAsync(hoje.Year, hoje.Month);

        Saldo = resumo.Saldo.ToString("C", _cultura);
        Receitas = resumo.TotalReceitas.ToString("C", _cultura);
        Despesas = resumo.TotalDespesas.ToString("C", _cultura);
        MesReferencia = hoje.ToString("MMMM yyyy", _cultura);

        Categorias.Clear();

        foreach (var categoria in resumo.DespesasPorCategoria
                     .OrderByDescending(c => c.Value))
        {
            double percentual = resumo.TotalDespesas > 0
                ? (double)(categoria.Value / resumo.TotalDespesas) * 100
                : 0;

            Categorias.Add(new CategoriaResumo
            {
                Nome = categoria.Key,
                Valor = categoria.Value.ToString("C", _cultura),
                Percentual = percentual
            });
        }

        ExtratoReceitas.Clear();

        foreach (var receita in receitasDoMes)
        {
            ExtratoReceitas.Add(receita);
        }
    }
}

public class CategoriaResumo
{
    public string Nome { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public double Percentual { get; set; }
}
