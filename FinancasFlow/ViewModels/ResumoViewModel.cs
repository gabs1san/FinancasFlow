
using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using FinancasFlow.Models;
using FinancasFlow.Services;
using Microsoft.Maui.Graphics;

namespace FinancasFlow.ViewModels;

public partial class ResumoViewModel : ObservableObject
{
    private readonly TransacaoService _transacaoService;

    private readonly CultureInfo _cultura =
        CultureInfo.GetCultureInfo("pt-BR");

    private readonly CartaoService _cartaoService;
    private readonly FinanceiroService _financeiroService = new();

    [ObservableProperty]
    private string saldo = "R$ 0,00";

    [ObservableProperty]
    private string receitas = "R$ 0,00";

    [ObservableProperty]
    private string despesas = "R$ 0,00";

    [ObservableProperty]
    private string mesReferencia = string.Empty;


    [ObservableProperty]
    private string nomeCartao = "Meu Cartão";

    [ObservableProperty]
    private string faturaCartao = "R$ 0,00";

    [ObservableProperty]
    private string limiteCartao = "R$ 0,00";

    [ObservableProperty]
    private string limiteTotalCartao = "R$ 0,00";

    [ObservableProperty]
    private double progressoCartao;

    [ObservableProperty]
    private string percentualCartao = "0% utilizado";

    [ObservableProperty]
    private bool possuiCartao;


    // Despesas por categoria
    public ObservableCollection<CategoriaResumo> Categorias
    { get; } = new();

    // Extrato clicável de receitas
    public ObservableCollection<ReceitaExtratoItem> ExtratoReceitas
    { get; } = new();

    // Despesas por forma de pagamento
    public ObservableCollection<PagamentoResumo> Pagamentos
    { get; } = new();

    public ResumoViewModel(
        TransacaoService transacaoService,
        CartaoService cartaoService)
    {
        _transacaoService = transacaoService;
        _cartaoService = cartaoService;
    }

    public async Task CarregarAsync()
    {
        var hoje = DateTime.Today;

        var resumo = await _transacaoService
            .ObterResumoMensalAsync(
                hoje.Year,
                hoje.Month);

        var receitasDoMes = await _transacaoService
            .ListarReceitasPorMesAsync(
                hoje.Year,
                hoje.Month);

        // Atualizar valores principais
        Saldo = resumo.Saldo.ToString("C", _cultura);

        Receitas = resumo.TotalReceitas.ToString("C", _cultura);

        Despesas = resumo.TotalDespesas.ToString("C", _cultura);

        MesReferencia = hoje.ToString(
            "MMMM yyyy",
            _cultura);

        // DESPESAS POR CATEGORIA
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

        // EXTRATO DE RECEITAS
        ExtratoReceitas.Clear();

        foreach (var receita in receitasDoMes)
        {
            ExtratoReceitas.Add(receita);
        }

        // DESPESAS POR PAGAMENTO
        Pagamentos.Clear();

        var maiorDespesa = resumo.DespesasPorPagamento.Count > 0
            ? resumo.DespesasPorPagamento.Values.Max()
            : 0m;

        foreach (var pagamento in resumo.DespesasPorPagamento
                     .OrderByDescending(p => p.Value))
        {
            double progresso = maiorDespesa > 0
                ? (double)(pagamento.Value / maiorDespesa)
                : 0;

            string nome = string.IsNullOrWhiteSpace(pagamento.Key)
                ? "Não informado"
                : pagamento.Key;

            Pagamentos.Add(new PagamentoResumo
            {
                Nome = nome,
                Valor = pagamento.Value.ToString("C", _cultura),
                Progresso = progresso,

                Cor = nome switch
                {
                    "PIX" => Color.FromArgb("#D8B866"),
                    "Débito" => Color.FromArgb("#7C9ED4"),
                    "Crédito" => Color.FromArgb("#B7A28C"),
                    "Dinheiro" => Color.FromArgb("#4ADE80"),
                    _ => Color.FromArgb("#A99A78")
                }
            });


            // CARTÃO DE CRÉDITO
            var cartoes = await _cartaoService.ListarAsync();

            PossuiCartao = cartoes.Count > 0;

            if (PossuiCartao)
            {
                var cartao = cartoes.First();

                // Usa todas as transações para manter a mesma
                // regra que o chat utiliza atualmente.
                var transacoes = await _transacaoService.ListarAsync();

                decimal totalCompras =
                    _financeiroService.CalcularFatura(transacoes, cartao);

                decimal limiteDisponivel =
                    _financeiroService.CalcularLimiteDisponivel(
                        transacoes, cartao);

                NomeCartao = cartao.Nome;

                FaturaCartao = totalCompras.ToString("C", _cultura);

                LimiteCartao = limiteDisponivel.ToString("C", _cultura);

                LimiteTotalCartao = cartao.Limite.ToString("C", _cultura);

                ProgressoCartao = cartao.Limite > 0
                    ? Math.Clamp(
                        (double)(totalCompras / cartao.Limite),
                        0,
                        1)
                    : 0;

                PercentualCartao = cartao.Limite > 0
                    ? $"{(totalCompras / cartao.Limite):P1} utilizado"
                    : "Limite não configurado";
            }
            else
            {
                NomeCartao = "Nenhum cartão cadastrado";
                FaturaCartao = "R$ 0,00";
                LimiteCartao = "R$ 0,00";
                LimiteTotalCartao = "R$ 0,00";
                ProgressoCartao = 0;
                PercentualCartao = "Sem cartão";
            }

        }
    }
}

// Modelo visual das categorias
public class CategoriaResumo
{
    public string Nome { get; set; } = string.Empty;

    public string Valor { get; set; } = string.Empty;

    public double Percentual { get; set; }
}

// Modelo visual das formas de pagamento
public class PagamentoResumo
{
    public string Nome { get; set; } = string.Empty;

    public string Valor { get; set; } = string.Empty;

    // ProgressBar utiliza valores entre 0 e 1
    public double Progresso { get; set; }

    public Color Cor { get; set; } =
        Color.FromArgb("#D8B866");
}
