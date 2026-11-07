
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

            Mensagens.Add(new MensagemChat
            {
                Texto = "Olá! 👋\n\nSou seu agente financeiro pessoal. Pode me contar seus gastos, entradas ou perguntar sobre seu saldo.",
                EnviadaPeloUsuario = false,
                DataEnvio = DateTime.Now
            });
        }

        [RelayCommand]
        private void EnviarMensagem()
        {
            string mensagem = MensagemDigitada.Trim();

            if (string.IsNullOrWhiteSpace(mensagem))
                return;

            // Mensagem enviada pelo usuário
            Mensagens.Add(new MensagemChat
            {
                Texto = mensagem,
                EnviadaPeloUsuario = true,
                DataEnvio = DateTime.Now
            });

            MensagemDigitada = string.Empty;

            // Processa a mensagem pelo Orquestrador
            string resposta = _orquestrador.ProcessarMensagem(mensagem);

            // Resposta do agente financeiro
            Mensagens.Add(new MensagemChat
            {
                Texto = resposta,
                EnviadaPeloUsuario = false,
                DataEnvio = DateTime.Now
            });
        }
    }
}
