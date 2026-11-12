
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancasFlow.Models;
using FinancasFlow.Services;

namespace FinancasFlow.ViewModels;

public partial class AdicionarViewModel : ObservableObject
{
    private readonly TransacaoService _transacaoService;

    private readonly CultureInfo _cultura =
        CultureInfo.GetCultureInfo("pt-BR");

    // Evento utilizado pela View para exibir alertas
    public event Func<string, string, Task>? MensagemSolicitada;

    [ObservableProperty]
    private string tipoSelecionado = "Despesa";

    [ObservableProperty]
    private string valor = string.Empty;

    [ObservableProperty]
    private string descricao = string.Empty;

    [ObservableProperty]
    private string? categoria;

    [ObservableProperty]
    private string? formaPagamento;

    [ObservableProperty]
    private DateTime data = DateTime.Today;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PodeSalvar))]
    [NotifyCanExecuteChangedFor(nameof(SalvarTransacaoCommand))]
    private bool salvando;

    public bool PodeSalvar => !Salvando;

    public AdicionarViewModel(TransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    private bool PodeExecutarSalvamento()
    {
        return !Salvando;
    }

    [RelayCommand(CanExecute = nameof(PodeExecutarSalvamento))]
    private async Task SalvarTransacaoAsync()
    {
        if (Salvando)
            return;

        // Validação do valor
        if (!decimal.TryParse(
                Valor,
                NumberStyles.Number,
                _cultura,
                out decimal valorConvertido) || valorConvertido <= 0)
        {
            await ExibirMensagemAsync(
                "Valor inválido",
                "Informe um valor maior que zero.");
            return;
        }

        // Validação da descrição
        string descricaoTratada =
            Descricao?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(descricaoTratada))
        {
            await ExibirMensagemAsync(
                "Descrição obrigatória",
                "Informe a descrição do lançamento.");
            return;
        }

        // Validação da categoria
        if (string.IsNullOrWhiteSpace(Categoria))
        {
            await ExibirMensagemAsync(
                "Categoria obrigatória",
                "Selecione uma categoria.");
            return;
        }

        // Validação do pagamento
        if (string.IsNullOrWhiteSpace(FormaPagamento))
        {
            await ExibirMensagemAsync(
                "Pagamento obrigatório",
                "Selecione a forma de pagamento.");
            return;
        }

        var transacao = new Transacao
        {
            Tipo = TipoSelecionado,
            Categoria = Categoria,
            Descricao = descricaoTratada,
            Valor = valorConvertido,
            FormaPagamento = FormaPagamento,
            Data = Data
        };

        try
        {
            Salvando = true;

            await _transacaoService.AdicionarAsync(transacao);

            LimparFormulario();

            await ExibirMensagemAsync(
                "Sucesso!",
                "Lançamento registrado com sucesso.");
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
            Salvando = false;
        }
    }

    private async Task ExibirMensagemAsync(
        string titulo,
        string mensagem)
    {
        if (MensagemSolicitada is not null)
        {
            await MensagemSolicitada.Invoke(titulo, mensagem);
        }
    }

    private void LimparFormulario()
    {
        Valor = string.Empty;
        Descricao = string.Empty;
        Categoria = null;
        FormaPagamento = null;
        Data = DateTime.Today;
        TipoSelecionado = "Despesa";
    }
}
