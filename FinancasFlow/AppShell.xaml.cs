
using FinancasFlow.Views;

namespace FinancasFlow;

public partial class AppShell : Shell
{
    public AppShell(MainContainerPage mainContainerPage)
    {
        InitializeComponent();

        PaginaInicial.Content = mainContainerPage;
    }
}
