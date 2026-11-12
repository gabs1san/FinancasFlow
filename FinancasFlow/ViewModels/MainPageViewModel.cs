
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
        private readonly MensagemChatService _mensagemService;

        private bool _historicoCarregado = false;

        public ObservableCollection<MensagemChat> Mensagens { get; }
            = new();

        [ObservableProperty]
        private string mensagemDigitada = string.Empty;

        public MainPageViewModel(
            Orquestrador orquestrador,
            MensagemChatService mensagemService)
        {
            _orquestrador = orquestrador;
            _mensagemService = mensagemService;
        }

        public async Task CarregarMensagensAsync()
        {
            if (_historicoCarregado)
                return;

            var historico = await _mensagemService.ObterMensagensAsync();

            Mensagens.Clear();

            if (historico.Count == 0)
            {
                Mensagens.Add(new MensagemChat
                {
                    Texto = "Olá! 👋\n\nSou seu agente financeiro pessoal. Pode me contar seus gastos, entradas ou perguntar sobre seu saldo.",
                    EnviadaPeloUsuario = false,
                    DataEnvio = DateTime.Now
                });
            }
            else
            {
                foreach (var mensagem in historico)
                {
                    Mensagens.Add(mensagem);
                }
            }

            _historicoCarregado = true;
        }

        [RelayCommand]
        private async Task EnviarMensagemAsync()
        {
            

            if (string.IsNullOrWhiteSpace(mensagemDigitada))
                return;

            string mensagem = MensagemDigitada.Trim();


            MensagemDigitada = string.Empty;

            // Mensagem do usuário
            var mensagemUsuario = new MensagemChat
            {
                Texto = mensagem,
                EnviadaPeloUsuario = true,
                DataEnvio = DateTime.Now
            };

            Mensagens.Add(mensagemUsuario);

            await _mensagemService.SalvarMensagemAsync(mensagemUsuario);

            // Processa a mensagem
            string resposta = _orquestrador.ProcessarMensagem(mensagem);

            // Mensagem do assistente
            var mensagemAssistente = new MensagemChat
            {
                Texto = resposta,
                EnviadaPeloUsuario = false,
                DataEnvio = DateTime.Now
            };

            Mensagens.Add(mensagemAssistente);

            await _mensagemService.SalvarMensagemAsync(mensagemAssistente);
        }
    }
}
