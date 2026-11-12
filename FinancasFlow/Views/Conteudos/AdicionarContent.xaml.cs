
using FinancasFlow.ViewModels;

namespace FinancasFlow.Views.Conteudos;

public partial class AdicionarContent : ContentView
{
    private readonly AdicionarViewModel _viewModel;

    public AdicionarContent(AdicionarViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = _viewModel;

        _viewModel.PropertyChanged += ViewModel_PropertyChanged;

        _viewModel.MensagemSolicitada += ExibirMensagemAsync;
    }

    private void Despesa_Tapped(
        object sender,
        TappedEventArgs e)
    {
        _viewModel.TipoSelecionado = "Despesa";
    }

    private void Entrada_Tapped(
        object sender,
        TappedEventArgs e)
    {
        _viewModel.TipoSelecionado = "Receita";
    }

    private void ViewModel_PropertyChanged(
        object? sender,
        System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AdicionarViewModel.TipoSelecionado))
        {
            AtualizarSelecaoVisual();
        }
    }

    private void AtualizarSelecaoVisual()
    {
        bool despesaSelecionada =
            _viewModel.TipoSelecionado == "Despesa";

        AtualizarBorda(BordaDespesa, despesaSelecionada);
        AtualizarBorda(BordaEntrada, !despesaSelecionada);
    }

    private static void AtualizarBorda(
        Border borda,
        bool selecionada)
    {
        borda.BackgroundColor = Color.FromArgb(
            selecionada ? "#C9A34A" : "#151515");

        borda.Stroke = new SolidColorBrush(
            Color.FromArgb(
                selecionada ? "#C9A34A" : "#49402C"));
    }

    private static async Task ExibirMensagemAsync(
        string titulo,
        string mensagem)
    {
        if (Shell.Current is not null)
        {
            await Shell.Current.DisplayAlertAsync(
                titulo,
                mensagem,
                "OK");
        }
    }
}
