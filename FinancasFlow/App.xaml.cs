
using FinancasFlow.Services;

namespace FinancasFlow;

public partial class App : Application
{
    private readonly BancoDadosInicializador _bancoInicializador;
    private readonly AppShell _appShell;

    public App(
        BancoDadosInicializador bancoInicializador,
        AppShell appShell)
    {
        InitializeComponent();

        _bancoInicializador = bancoInicializador;
        _appShell = appShell;
    }

    protected override Window CreateWindow(
        IActivationState? activationState)
    {
        // Inicializa o banco antes de abrir a interface
        _bancoInicializador.InicializarAsync()
            .GetAwaiter()
            .GetResult();

        return new Window(_appShell);
    }
}
