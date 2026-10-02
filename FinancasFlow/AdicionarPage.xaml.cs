namespace FinancasFlow;

public partial class AdicionarPage : ContentPage
{

    private async void Adicionar_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AdicionarPage());
    }
    public AdicionarPage()
    {
        InitializeComponent();
    }
}