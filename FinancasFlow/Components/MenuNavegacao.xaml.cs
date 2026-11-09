
namespace FinancasFlow.Components;

public partial class MenuNavegacao : ContentView
{
    public event EventHandler<string>? AbaSelecionada;

    private const uint DuracaoAnimacao = 300;

    private string _abaAtual = "Inicio";

    public MenuNavegacao()
    {
        InitializeComponent();
    }

    private void Inicio_Tapped(object sender, TappedEventArgs e)
        => SelecionarAba("Inicio");

    private void Resumo_Tapped(object sender, TappedEventArgs e)
        => SelecionarAba("Resumo");

    private void Adicionar_Tapped(object sender, TappedEventArgs e)
        => SelecionarAba("Adicionar");

    private void Perfil_Tapped(object sender, TappedEventArgs e)
        => SelecionarAba("Perfil");

    private void SelecionarAba(string aba)
    {
        if (aba == _abaAtual)
            return;

        // Solicita a navegação ao contêiner.
        AbaSelecionada?.Invoke(this, aba);
    }

    public async Task AtualizarSelecaoAsync(string aba)
    {
        if (aba == _abaAtual)
            return;

        View? destino = aba switch
        {
            "Inicio" => ItemInicio,
            "Resumo" => ItemResumo,
            "Adicionar" => ItemAdicionar,
            "Perfil" => ItemPerfil,
            _ => null
        };

        if (destino is null)
            return;

        _abaAtual = aba;

        // Coordenadas relativas ao GridMenu.
        double deslocamento = destino.X - ItemInicio.X;

        await IndicadorSelecionado.TranslateToAsync(
            deslocamento,
            0,
            DuracaoAnimacao,
            Easing.CubicInOut
        );

        AtualizarCores(aba);
    }

    private void AtualizarCores(string aba)
    {
        AtualizarTexto(TextoInicio, aba == "Inicio");
        AtualizarTexto(TextoResumo, aba == "Resumo");
        AtualizarTexto(TextoAdicionar, aba == "Adicionar");
        AtualizarTexto(TextoPerfil, aba == "Perfil");
    }

    private static void AtualizarTexto(Label label, bool selecionado)
    {
        label.TextColor = Color.FromArgb(
            selecionado ? "#D8B866" : "#A99A78");

        label.FontAttributes = selecionado
            ? FontAttributes.Bold
            : FontAttributes.None;
    }
}
