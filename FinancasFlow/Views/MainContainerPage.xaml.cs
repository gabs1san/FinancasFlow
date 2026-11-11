
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

        _animando = true;

        try
        {
            if (aba == "Resumo")
                await _resumoContent.AtualizarAsync();

            await Task.WhenAll(
                TrocarConteudoAsync(novoConteudo),
                MenuInferior.AtualizarSelecaoAsync(aba)
            );

            _abaAtual = aba;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await DisplayAlertAsync(
                "Erro",
                "Não foi possível atualizar o resumo.",
                "OK");
        }
        finally
        {
            _animando = false;
        }
    }


    private async Task TrocarConteudoAsync(View novoConteudo)
    {
        await ContentArea.FadeToAsync(0, 120);

        ContentArea.Content = novoConteudo;
        ContentArea.TranslationX = 25;

        await Task.WhenAll(
            ContentArea.FadeToAsync(1, 120),
            ContentArea.TranslateToAsync(0, 0, 120)
        );
    }
}
