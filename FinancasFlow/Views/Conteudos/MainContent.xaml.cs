
using FinancasFlow.ViewModels;
using System.Collections.Specialized;

namespace FinancasFlow.Views.Conteudos;

public partial class MainContent : ContentView
{
    private readonly MainPageViewModel _viewModel;

    private bool _mensagensCarregadas;

    public MainContent(MainPageViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = _viewModel;

        // Observa novas mensagens para rolagem automática
        _viewModel.Mensagens.CollectionChanged +=
            Mensagens_CollectionChanged;
    }

    public async Task InicializarAsync()
    {
        if (_mensagensCarregadas)
            return;

        await _viewModel.CarregarMensagensAsync();

        _mensagensCarregadas = true;

        RolarParaUltimaMensagem(false);
    }

    private void Mensagens_CollectionChanged(
     object? sender,
     NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            Dispatcher.Dispatch(() =>
            {
                RolarParaUltimaMensagem(true);
            });
        }
    }

    private void RolarParaUltimaMensagem(bool animar)
    {
        if (_viewModel.Mensagens.Count == 0)
            return;

        var ultimaMensagem = _viewModel.Mensagens.Last();

        ListaMensagens.ScrollTo(
            ultimaMensagem,
            position: ScrollToPosition.End,
            animate: animar
        );
    }
}
