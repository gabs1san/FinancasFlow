
using FinancasFlow.ViewModels;


namespace FinancasFlow.Views.Conteudos;

public partial class ResumoContent : ContentView
{
    private readonly ResumoViewModel _viewModel;

    private bool _extratoVisivel;
    private bool _animandoExtrato;

    public ResumoContent(ResumoViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = _viewModel;
    }

    public async Task AtualizarAsync()
    {
        await _viewModel.CarregarAsync();

        MensagemSemDespesas.IsVisible =
            _viewModel.Categorias.Count == 0;

        MensagemSemReceitas.IsVisible =
            _viewModel.Categorias.Count == 0;
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
