using FinancasFlow;

namespace FinancasFlow.Components
{
    public partial class MenuNavegacao : ContentView
    {
        public MenuNavegacao()
        {
            InitializeComponent();
        }

        private async void Inicio_Tapped(
            object sender,
            TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void Resumo_Tapped(
            object sender,
            TappedEventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ResumoPage));
        }

        private async void Adicionar_Tapped(
            object sender,
            TappedEventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AdicionarPage));
        }

        private async void Perfil_Tapped(
            object sender,
            TappedEventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PerfilPage));
        }
    }
}