namespace FinancasFlow
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
                nameof(ResumoPage),
                typeof(ResumoPage));

            Routing.RegisterRoute(
                nameof(AdicionarPage),
                typeof(AdicionarPage));

            Routing.RegisterRoute(
                nameof(PerfilPage),
                typeof(PerfilPage));
        }
    }
}