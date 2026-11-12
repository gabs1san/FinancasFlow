
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FinancasFlow.Models;
using FinancasFlow.Services;

namespace FinancasFlow.ViewModels;

public class AdicionarViewModel : INotifyPropertyChanged
{
    private readonly TransacaoService _transacaoService;

    private readonly CultureInfo _cultura =
        CultureInfo.GetCultureInfo("pt-BR");

    public event PropertyChangedEventHandler? PropertyChanged;

    // Evento para solicitar mensagens à interface
    public event Func<string, string, Task>? MensagemSolicitada;

    private string _tipoSelecionado = "Despesa";
    private string _valor = string.Empty;
    private string _descricao = string.Empty;
    private string? _categoria;
    private string? _formaPagamento;
    private DateTime _data = DateTime.Today;
    private bool _salvando;

    public string TipoSelecionado
    {
        get => _tipoSelecionado;
        set => DefinirPropriedade(ref _tipoSelecionado, value);
    }

    public string Valor
    {
        get => _valor;
        set => DefinirPropriedade(ref _valor, value);
    }

    public string Descricao
    {
        get => _descricao;
        set => DefinirPropriedade(ref _descricao, value);
    }

    public string? Categoria
    {
        get => _categoria;
        set => DefinirPropriedade(ref _categoria, value);
    }

    public string? FormaPagamento
    {
        get => _formaPagamento;
        set => DefinirPropriedade(ref _formaPagamento, value);
    }

    public DateTime Data
    {
        get => _data;
        set => DefinirPropriedade(ref _data, value);
    }

    public bool Salvando
    {
        get => _salvando;
        private set
        {
            if (DefinirPropriedade(ref _salvando, value))
                OnPropertyChanged(nameof(PodeSalvar));
        }
    }

    public bool PodeSalvar => !Salvando;

    public ICommand SalvarTransacaoCommand { get; }

    public AdicionarViewModel(TransacaoService transacaoService)
    {
        _transacaoService = transacaoService;

        SalvarTransacaoCommand = new Command(
            async () => await SalvarAsync(),
            () => PodeSalvar);
    }

    public async Task SalvarAsync()
    {
        if (Salvando)
            return;

        if (!decimal.TryParse(
                Valor,
                NumberStyles.Number,
                _cultura,
                out decimal valor) || valor <= 0)
        {
            await ExibirMensagemAsync(
                "Valor inválido",
                "Informe um valor maior que zero.");
            return;
        }

        string descricao = Descricao?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(descricao))
        {
            await ExibirMensagemAsync(
                "Descrição obrigatória",
                "Informe a descrição do lançamento.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Categoria))
        {
            await ExibirMensagemAsync(
                "Categoria obrigatória",
                "Selecione uma categoria.");
            return;
        }

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
            Descricao = descricao,
            Valor = valor,
            FormaPagamento = FormaPagamento,
            Data = Data
        };

        try
        {
            Salvando = true;

            ((Command)SalvarTransacaoCommand)
                .ChangeCanExecute();

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
            Salvando = false;

            ((Command)SalvarTransacaoCommand)
                .ChangeCanExecute();
        }
    }

    private async Task ExibirMensagemAsync(
        string titulo,
        string mensagem)
    {
        if (MensagemSolicitada is not null)
            await MensagemSolicitada.Invoke(titulo, mensagem);
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

    private bool DefinirPropriedade<T>(
        ref T campo,
        T valor,
        [CallerMemberName] string? nome = null)
    {
        if (EqualityComparer<T>.Default.Equals(campo, valor))
            return false;

        campo = valor;
        OnPropertyChanged(nome);

        return true;
    }

    private void OnPropertyChanged(string? nome)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nome));
    }
}
