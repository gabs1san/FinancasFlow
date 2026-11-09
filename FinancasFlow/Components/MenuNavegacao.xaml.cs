
namespace FinancasFlow.Components;

public partial class MenuNavegacao : ContentView
{
    public event EventHandler<string>? AbaSelecionada;

    private const string CorDourada = "#D8B866";
    private const string CorInativa = "#A99A78";
    private const string FundoSelecionado = "#332A1A";
    private const string BordaSelecionada = "#8D7133";

    public MenuNavegacao()
    {
        InitializeComponent();
        AtualizarSelecao("Inicio");
    }

    private void Inicio_Tapped(object sender, TappedEventArgs e)
    {
        SelecionarAba("Inicio");
    }

    private void Resumo_Tapped(object sender, TappedEventArgs e)
    {
        SelecionarAba("Resumo");
    }

    private void Adicionar_Tapped(object sender, TappedEventArgs e)
    {
        SelecionarAba("Adicionar");
    }

    private void Perfil_Tapped(object sender, TappedEventArgs e)
    {
        SelecionarAba("Perfil");
    }

    private void SelecionarAba(string aba)
    {
        AbaSelecionada?.Invoke(this, aba);
    }

    public void AtualizarSelecao(string aba)
    {
        AtualizarItem(
            BordaInicio, TextoInicio, aba == "Inicio");

        AtualizarItem(
            BordaResumo, TextoResumo, aba == "Resumo");

        AtualizarItem(
            BordaAdicionar, TextoAdicionar, aba == "Adicionar");

        AtualizarItem(
            BordaPerfil, TextoPerfil, aba == "Perfil");
    }

    private static void AtualizarItem(
        Border borda,
        Label texto,
        bool selecionado)
    {
        borda.BackgroundColor = Color.FromArgb(
            selecionado ? FundoSelecionado : "#151515");

        borda.Stroke = new SolidColorBrush(
            Color.FromArgb(
                selecionado ? BordaSelecionada : "#151515"));

        texto.TextColor = Color.FromArgb(
            selecionado ? CorDourada : CorInativa);

        texto.FontAttributes = selecionado
            ? FontAttributes.Bold
            : FontAttributes.None;
    }
}
