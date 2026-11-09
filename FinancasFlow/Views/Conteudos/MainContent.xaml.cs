
using FinancasFlow.Data;
using FinancasFlow.Models;
using FinancasFlow.Services;
using FinancasFlow.ViewModels;
using Microsoft.Extensions.DependencyInjection;

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
    }

    public async Task InicializarAsync()
    {
        if (_mensagensCarregadas)
            return;

        await _viewModel.CarregarMensagensAsync();

        _mensagensCarregadas = true;
    }
}
