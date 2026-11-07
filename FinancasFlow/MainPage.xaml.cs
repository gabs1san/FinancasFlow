
using FinancasFlow.Data;
using FinancasFlow.Models;
using FinancasFlow.Services;
using FinancasFlow.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FinancasFlow
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            // Memória financeira temporária
            var memoria = new MemoriaFinanceira();

            // Cartão de teste
            memoria.Cartoes.Add(new CartaoDeCredito
            {
                Id = 1,
                Nome = "Meu Cartão",
                Limite = 3000,
                DiaFechamento = 25,
                DiaVencimento = 5
            });

            // Agente financeiro
            var agente = new Orquestrador(memoria);

            // Recupera o serviço de histórico do SQLite
            var mensagemService = IPlatformApplication.Current!
                .Services
                .GetRequiredService<MensagemChatService>();

            // Cria o ViewModel
            _viewModel = new MainPageViewModel(
                agente,
                mensagemService
            );

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.CarregarMensagensAsync();
        }
    }
}
