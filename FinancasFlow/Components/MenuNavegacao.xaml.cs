
namespace FinancasFlow.Components
{
    public partial class MenuNavegacao : ContentView
    {
        public event EventHandler<string>? AbaSelecionada;

        public MenuNavegacao()
        {
            InitializeComponent();
        }

        private void Inicio_Tapped(object sender, TappedEventArgs e)
        {
            AbaSelecionada?.Invoke(this, "Inicio");
        }

        private void Resumo_Tapped(object sender, TappedEventArgs e)
        {
            AbaSelecionada?.Invoke(this, "Resumo");
        }

        private void Adicionar_Tapped(object sender, TappedEventArgs e)
        {
            AbaSelecionada?.Invoke(this, "Adicionar");
        }

        private void Perfil_Tapped(object sender, TappedEventArgs e)
        {
            AbaSelecionada?.Invoke(this, "Perfil");
        }
    }
}
