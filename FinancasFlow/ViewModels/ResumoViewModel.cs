
using FinancasFlow.Models;
using FinancasFlow.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace FinancasFlow.ViewModels;

public class ResumoViewModel : INotifyPropertyChanged
{
    private readonly TransacaoService _transacaoService;

    private readonly CultureInfo _cultura =
        CultureInfo.GetCultureInfo("pt-BR");

    public event PropertyChangedEventHandler? PropertyChanged;

    private string _saldo = "R$ 0,00";
    private string _receitas = "R$ 0,00";
    private string _despesas = "R$ 0,00";
    private string _mesReferencia = "";

    public string Saldo
    {
        get => _saldo;
        private set => DefinirPropriedade(ref _saldo, value);
    }

    public string Receitas
    {
        get => _receitas;
        private set => DefinirPropriedade(ref _receitas, value);
    }

    public string Despesas
    {
        get => _despesas;
        private set => DefinirPropriedade(ref _despesas, value);
    }

    public string MesReferencia
    {
        get => _mesReferencia;
        private set => DefinirPropriedade(ref _mesReferencia, value);
    }

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

        var receitas = await _transacaoService
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

        foreach (var receita in receitas)
        {
            ExtratoReceitas.Add(receita);
        }
    }

    private bool DefinirPropriedade<T>(
        ref T campo,
        T valor,
        [CallerMemberName] string? nomePropriedade = null)
    {
        if (EqualityComparer<T>.Default.Equals(campo, valor))
            return false;

        campo = valor;

        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nomePropriedade));

        return true;
    }
}

public class CategoriaResumo
{
    public string Nome { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public double Percentual { get; set; }
}
