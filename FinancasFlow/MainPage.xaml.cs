
using FinancasFlow.Data;
using FinancasFlow.Models;
using FinancasFlow.Services;
using FinancasFlow.ViewModels;

namespace FinancasFlow
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Cria a memória temporária da aplicação.
            var memoria = new MemoriaFinanceira();

            // Cartão de teste.
            memoria.Cartoes.Add(
                new CartaoDeCredito
                {
                    Id = 1,
                    Nome = "Meu Cartão",
                    Limite = 3000,
                    DiaFechamento = 25,
                    DiaVencimento = 5
                });

            // Cria o agente financeiro com essa mesma memória.
            var agente = new Orquestrador(memoria);

            // Conecta a interface ao ViewModel.
            BindingContext = new MainPageViewModel(agente);
        }
    }
}
