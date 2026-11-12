
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

    public MainContainerPage(
        MainContent mainContent,
        ResumoContent resumoContent,
        AdicionarContent adicionarContent,
        PerfilContent perfilContent)
    {
        InitializeComponent();

        _mainContent = mainContent;
        _resumoContent = resumoContent;
        _adicionarContent = adicionarContent;
        _perfilContent = perfilContent;

        ContentArea.Content = _mainContent;

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
                "Não foi possível atualizar a tela.",
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
