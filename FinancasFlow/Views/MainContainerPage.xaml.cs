
using FinancasFlow.Views.Conteudos;

namespace FinancasFlow.Views;

public partial class MainContainerPage : ContentPage
{
    private readonly MainContent _mainContent;
    private readonly ResumoContent _resumoContent;
    private readonly AdicionarContent _adicionarContent;
    private readonly PerfilContent _perfilContent;

    private string _abaAtual = "Inicio";
    private bool _animando;

    public MainContainerPage()
    {
        InitializeComponent();

        // Criamos os quatro componentes uma única vez.
        _mainContent = new MainContent();
        _resumoContent = new ResumoContent();
        _adicionarContent = new AdicionarContent();
        _perfilContent = new PerfilContent();

        // Exibe o chat inicialmente.
        ContentArea.Content = _mainContent;

        // Recebe os eventos do menu inferior.
        MenuInferior.AbaSelecionada += MenuInferior_AbaSelecionada;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _mainContent.InicializarAsync();
    }

    private async void MenuInferior_AbaSelecionada(
        object? sender,
        string aba)
    {
        if (_animando || aba == _abaAtual)
            return;

        View? novoConteudo = aba switch
        {
            "Inicio" => _mainContent,
            "Resumo" => _resumoContent,
            "Adicionar" => _adicionarContent,
            "Perfil" => _perfilContent,
            _ => null
        };

        if (novoConteudo is null)
            return;

        await TrocarConteudoAsync(novoConteudo);

        _abaAtual = aba;
    }

    private async Task TrocarConteudoAsync(View novoConteudo)
    {
        _animando = true;

        try
        {
            // Desaparece suavemente.
            await ContentArea.FadeToAsync(0, 120);

            // Troca apenas o conteúdo central.
            ContentArea.Content = novoConteudo;
            ContentArea.TranslationX = 25;

            // Aparece deslizando suavemente.
            await Task.WhenAll(
                ContentArea.FadeToAsync(1, 180),
                ContentArea.TranslateToAsync(0, 0, 180)
            );
        }
        finally
        {
            _animando = false;
        }
    }
}
