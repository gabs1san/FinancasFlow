
using FinancasFlow.Data;
using FinancasFlow.Models;
using FinancasFlow.Services;
using FinancasFlow.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Specialized;

namespace FinancasFlow.Views.Conteudos;

public partial class MainContent : ContentView
{
    private readonly MainPageViewModel _viewModel;
    private bool _mensagensCarregadas;

    public MainContent()
    {
        InitializeComponent();

        var memoria = new MemoriaFinanceira();

        memoria.Cartoes.Add(new CartaoDeCredito
        {
            Id = 1,
            Nome = "Meu Cartão",
            Limite = 3000,
            DiaFechamento = 25,
            DiaVencimento = 5
        });

        var agente = new Orquestrador(memoria);

        var mensagemService =
            IPlatformApplication.Current!.Services
                .GetRequiredService<MensagemChatService>();

        _viewModel = new MainPageViewModel(
            agente,
            mensagemService
        );

        BindingContext = _viewModel;

        // Observa alterações na coleção de mensagens
        _viewModel.Mensagens.CollectionChanged +=
            Mensagens_CollectionChanged;
    }

    public async Task InicializarAsync()
    {
        if (_mensagensCarregadas)
            return;

        await _viewModel.CarregarMensagensAsync();

        _mensagensCarregadas = true;

        // Ao abrir o chat, mostra a última mensagem
        RolarParaUltimaMensagem(false);
    }

    private void Mensagens_CollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            // Executa a rolagem na thread da interface
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
