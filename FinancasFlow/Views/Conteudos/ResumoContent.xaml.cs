
using FinancasFlow.Services;
using FinancasFlow.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FinancasFlow.Views.Conteudos;

public partial class ResumoContent : ContentView
{
    private readonly ResumoViewModel _viewModel;
    private bool _extratoVisivel;

    public ResumoContent()
    {
        InitializeComponent();

        var transacaoService = IPlatformApplication.Current!
            .Services
            .GetRequiredService<TransacaoService>();

        _viewModel = new ResumoViewModel(transacaoService);

        // Disponibiliza os dados para o XAML
        BindingContext = _viewModel;
    }

    public async Task AtualizarAsync()
    {
        await _viewModel.CarregarAsync();

        TextoMes.Text = _viewModel.MesReferencia;
        TextoSaldo.Text = _viewModel.Saldo;
        TextoReceitas.Text = _viewModel.Receitas;
        TextoDespesas.Text = _viewModel.Despesas;

        // Total no rodapé da lista de categorias
        TextoTotalDespesas.Text = _viewModel.Despesas;

        ListaCategorias.Children.Clear();

        if (_viewModel.Categorias.Count == 0)
        {
            ListaCategorias.Children.Add(new Label
            {
                Text = "Nenhuma despesa registrada neste mês.",
                TextColor = Color.FromArgb("#A99A78"),
                FontSize = 13
            });

            return;
        }

        foreach (var categoria in _viewModel.Categorias)
        {
            var linha = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };

            var nome = new Label
            {
                Text = categoria.Nome,
                TextColor = Color.FromArgb("#D8B866"),
                FontSize = 14
            };

            var valor = new Label
            {
                Text = categoria.Valor,
                TextColor = Colors.White,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold
            };

            Grid.SetColumn(nome, 0);
            Grid.SetColumn(valor, 1);

            linha.Children.Add(nome);
            linha.Children.Add(valor);

            ListaCategorias.Children.Add(linha);
        }
    }

    private async void Receitas_Tapped(
        object sender,
        TappedEventArgs e)
    {
        _extratoVisivel = !_extratoVisivel;

        if (_extratoVisivel)
        {
            PainelExtrato.IsVisible = true;
            PainelExtrato.Opacity = 0;

            await PainelExtrato.FadeToAsync(1, 200);

            TextoAcaoExtrato.Text = "Ocultar extrato";
        }
        else
        {
            await PainelExtrato.FadeToAsync(0, 150);

            PainelExtrato.IsVisible = false;

            TextoAcaoExtrato.Text = "Ver extrato";
        }
    }
}
