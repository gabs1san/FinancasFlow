
using FinancasFlow.Services;
using FinancasFlow.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FinancasFlow.Views.Conteudos;

public partial class ResumoContent : ContentView
{
    private readonly ResumoViewModel _viewModel;

    private bool _extratoVisivel;
    private bool _animandoExtrato;

    public ResumoContent()
    {
        InitializeComponent();

        var transacaoService = IPlatformApplication.Current!
            .Services
            .GetRequiredService<TransacaoService>();

        _viewModel = new ResumoViewModel(transacaoService);

        BindingContext = _viewModel;
    }

    public async Task AtualizarAsync()
    {
        await _viewModel.CarregarAsync();

        TextoMes.Text = _viewModel.MesReferencia;
        TextoSaldo.Text = _viewModel.Saldo;
        TextoReceitas.Text = _viewModel.Receitas;
        TextoDespesas.Text = _viewModel.Despesas;

        TextoTotalDespesas.Text = _viewModel.Despesas;

        MensagemSemDespesas.IsVisible =
            _viewModel.Categorias.Count == 0;

        MensagemSemReceitas.IsVisible =
            _viewModel.ExtratoReceitas.Count == 0;
    }

    private async void Receitas_Tapped(
        object sender,
        TappedEventArgs e)
    {
        if (_animandoExtrato)
            return;

        _animandoExtrato = true;

        try
        {
            if (!_extratoVisivel)
            {
                PainelExtrato.Opacity = 0;
                PainelExtrato.IsVisible = true;

                await PainelExtrato.FadeToAsync(1, 200);

                _extratoVisivel = true;
                TextoAcaoExtrato.Text = "Ocultar extrato";
            }
            else
            {
                await PainelExtrato.FadeToAsync(0, 150);

                PainelExtrato.IsVisible = false;

                _extratoVisivel = false;
                TextoAcaoExtrato.Text = "Ver extrato";
            }
        }
        finally
        {
            _animandoExtrato = false;
        }
    }
}
