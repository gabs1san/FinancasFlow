
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinancasFlow.Models;
using FinancasFlow.Services;

namespace FinancasFlow.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly Orquestrador _orquestrador;

        public ObservableCollection<MensagemChat> Mensagens { get; }
            = new();

        [ObservableProperty]
        private string mensagemDigitada = string.Empty;

        public MainPageViewModel(Orquestrador orquestrador)
        {
            _orquestrador = orquestrador;

            Mensagens.Add(
                new MensagemChat(
                    "Olá! 👋\n\nSou seu agente financeiro pessoal. Pode me contar seus gastos, entradas ou perguntar sobre seu saldo.",
                    false));
        }

        [RelayCommand]
        private void EnviarMensagem()
        {
            string mensagem = MensagemDigitada.Trim();

            if (string.IsNullOrWhiteSpace(mensagem))
                return;

            Mensagens.Add(
                new MensagemChat(mensagem, true));

            MensagemDigitada = string.Empty;

            string resposta =
                _orquestrador.ProcessarMensagem(mensagem);

            Mensagens.Add(
                new MensagemChat(resposta, false));
        }
    }
}
