
using FinancasFlow.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinancasFlow
{
    public partial class App : Application
    {
        private readonly BancoDadosInicializador _bancoInicializador;

        public App(BancoDadosInicializador bancoInicializador)
        {
            InitializeComponent();

            _bancoInicializador = bancoInicializador;
        }

        protected override Window CreateWindow(
            IActivationState? activationState)
        {
            // Inicializa o SQLite antes de abrir a interface.
            _bancoInicializador.InicializarAsync()
                .GetAwaiter()
                .GetResult();

            return new Window(new AppShell());
        }
    }
}
