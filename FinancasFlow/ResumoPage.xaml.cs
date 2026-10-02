namespace FinancasFlow;

public partial class ResumoPage : ContentPage
{

    private async void Resumo_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ResumoPage());
    }

    public ResumoPage()
    {
        InitializeComponent();
    }

   
}