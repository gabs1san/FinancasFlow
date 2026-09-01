using FinancasFlow.Models;
using FinancasFlow.Services;

namespace FinancasFlow
{
    public partial class MainPage : ContentPage
    {
        private AgenteFinanceiroService agente;


        public MainPage()
        {
            InitializeComponent();

            agente =
                new AgenteFinanceiroService();
        }


        private void EnviarButton_Clicked(
            object sender,
            EventArgs e)
        {
            string mensagem =
                txtMensagem.Text;


            if (string.IsNullOrWhiteSpace(mensagem))
            {
                return;
            }


            // Mostra mensagem do usuário

            AdicionarMensagemUsuario(mensagem);


            // Processa com o agente

            Transacao transacao =
                agente.ProcessarMensagem(mensagem);


            // Mostra resultado

            MostrarResultado(transacao);


            // Limpa campo

            txtMensagem.Text = "";
        }


        private void MostrarResultado(
            Transacao transacao)
        {
            string mensagem =
                $"Tipo: {transacao.Tipo}\n" +
                $"Valor: R$ {transacao.Valor:F2}\n" +
                $"Categoria: {transacao.Categoria}\n" +
                $"Pagamento: {transacao.FormaPagamento}";


            AdicionarMensagemBot(mensagem);
        }


        private void AdicionarMensagemUsuario(
            string mensagem)
        {
            Frame frame = new Frame
            {
                BackgroundColor = Colors.LightGreen,
                CornerRadius = 15,
                Padding = 10,
                HorizontalOptions =
                    LayoutOptions.End
            };


            frame.Content = new Label
            {
                Text = mensagem,
                FontSize = 16
            };


            ChatContainer.Add(frame);
        }


        private void AdicionarMensagemBot(
            string mensagem)
        {
            Frame frame = new Frame
            {
                BackgroundColor = Colors.LightGray,
                CornerRadius = 15,
                Padding = 10,
                HorizontalOptions =
                    LayoutOptions.Start
            };


            frame.Content = new Label
            {
                Text = mensagem,
                FontSize = 16
            };


            ChatContainer.Add(frame);
        }


        private void AudioButton_Clicked(
            object sender,
            EventArgs e)
        {
            DisplayAlert(
                "Áudio",
                "Funcionalidade em desenvolvimento.",
                "OK");
        }
    }
}