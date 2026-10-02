namespace FinancasFlow;

public partial class PerfilPage : ContentPage
{

    private async void Perfil_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PerfilPage());
    }
    public PerfilPage()
    {
        InitializeComponent();
    }
}