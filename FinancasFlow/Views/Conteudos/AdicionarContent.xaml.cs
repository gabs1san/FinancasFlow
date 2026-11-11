
using System.Globalization;
using FinancasFlow.Models;
using FinancasFlow.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinancasFlow.Views.Conteudos;

public partial class AdicionarContent : ContentView
{
    private readonly TransacaoService _transacaoService;

    private string _tipoSelecionado = "Despesa";
    private bool _salvando;

    public AdicionarContent()
    {
        InitializeComponent();

        _transacaoService = IPlatformApplication.Current!
            .Services
            .GetRequiredService<TransacaoService>();

        CampoData.Date = DateTime.Today;
    }

    private void Despesa_Tapped(
        object sender,
        TappedEventArgs e)
    {
        SelecionarTipo("Despesa");
    }

    private void Entrada_Tapped(
        object sender,
        TappedEventArgs e)
    {
        SelecionarTipo("Receita");
    }

    private void SelecionarTipo(string tipo)
    {
        _tipoSelecionado = tipo;

        bool despesaSelecionada = tipo == "Despesa";

        AtualizarBorda(BordaDespesa, despesaSelecionada);
        AtualizarBorda(BordaEntrada, !despesaSelecionada);
    }

    private static void AtualizarBorda(
        Border borda,
        bool selecionada)
    {
        borda.BackgroundColor = Color.FromArgb(
            selecionada ? "#C9A34A" : "#151515");

        borda.Stroke = new SolidColorBrush(
            Color.FromArgb(
                selecionada ? "#C9A34A" : "#49402C"));
    }

    private async void SalvarLancamento_Clicked(
        object sender,
        EventArgs e)
    {
        if (_salvando)
            return;

        var cultura = CultureInfo.GetCultureInfo("pt-BR");

        if (!decimal.TryParse(
                CampoValor.Text,
                NumberStyles.Number,
                cultura,
                out decimal valor) || valor <= 0)
        {
            await ExibirMensagemAsync(
                "Valor inválido",
                "Informe um valor maior que zero.");
            return;
        }

        string descricao = CampoDescricao.Text?.Trim()
            ?? string.Empty;

        if (string.IsNullOrWhiteSpace(descricao))
        {
            await ExibirMensagemAsync(
                "Descrição obrigatória",
                "Informe a descrição do lançamento.");
            return;
        }

        if (CampoCategoria.SelectedItem is not string categoria)
        {
            await ExibirMensagemAsync(
                "Categoria obrigatória",
                "Selecione uma categoria.");
            return;
        }

        if (CampoPagamento.SelectedItem is not string pagamento)
        {
            await ExibirMensagemAsync(
                "Pagamento obrigatório",
                "Selecione a forma de pagamento.");
            return;
        }

        var transacao = new Transacao
        {
            Tipo = _tipoSelecionado,
            Categoria = categoria,
            Descricao = descricao,
            Valor = valor,
            FormaPagamento = pagamento,
            Data = CampoData.Date ?? DateTime.Today
        };

        try
        {
            _salvando = true;
            BotaoSalvar.IsEnabled = false;

            await _transacaoService.AdicionarAsync(transacao);

            await ExibirMensagemAsync(
                "Sucesso!",
                "Lançamento registrado com sucesso.");

            LimparFormulario();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await ExibirMensagemAsync(
                "Erro ao salvar",
                "Não foi possível registrar o lançamento.");
        }
        finally
        {
            _salvando = false;
            BotaoSalvar.IsEnabled = true;
        }
    }

    private static async Task ExibirMensagemAsync(
        string titulo,
        string mensagem)
    {
        if (Shell.Current is not null)
        {
            await Shell.Current.DisplayAlertAsync(
                titulo, mensagem, "OK");
        }
    }

    private void LimparFormulario()
    {
        CampoValor.Text = string.Empty;
        CampoDescricao.Text = string.Empty;

        CampoCategoria.SelectedIndex = -1;
        CampoPagamento.SelectedIndex = -1;

        CampoData.Date = DateTime.Today;

        SelecionarTipo("Despesa");
    }
}
