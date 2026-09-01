using FinancasFlow.Data;
using FinancasFlow.Services;


namespace FinancasFlow  
{
    public partial class MainPage : ContentPage
    {
        private readonly MemoriaFinanceira _memoria;


    private readonly Orquestrador _agente;


        public MainPage()
        {
            InitializeComponent();


            // Cria a memória temporária

            _memoria =
                new MemoriaFinanceira();


            // Cria o agente financeiro

            _agente =
                new Orquestrador(
                    _memoria);


            ConfigurarDadosIniciais();
        }


        private void ConfigurarDadosIniciais()
        {
            /*
             * Aqui vamos colocar dados de teste.
             *
             * Posteriormente esses dados serão
             * cadastrados pelo próprio usuário.
             */


            _memoria.Cartoes.Add(
                new Models.CartaoDeCredito
                {
                    Id = 1,

                    Nome = "Meu Cartão",

                    Limite = 3000,

                    DiaFechamento = 25,

                    DiaVencimento = 5
                });
        }


        private async void EnviarButton_Clicked(
            object sender,
            EventArgs e)
        {
            string mensagem =
                txtMensagem.Text;


            // Verifica se está vazio

            if (string.IsNullOrWhiteSpace(mensagem))
            {
                return;
            }


            // Mostra mensagem do usuário

            AdicionarMensagemUsuario(mensagem);


            // Limpa o campo

            txtMensagem.Text = "";


            // Processa a mensagem

            string resposta =
                _agente.ProcessarMensagem(
                    mensagem);


            // Mostra resposta

            AdicionarMensagemBot(resposta);


            // Desce automaticamente o chat

           
        }


        private void AdicionarMensagemUsuario(
            string mensagem)
        {
            Frame mensagemFrame =
                new Frame
                {
                    BackgroundColor =
                        Colors.LightGreen,

                    CornerRadius = 15,

                    Padding = 12,

                    HorizontalOptions =
                        LayoutOptions.End,

                    MaximumWidthRequest = 300
                };


            Label mensagemLabel =
                new Label
                {
                    Text = mensagem,

                    FontSize = 16
                };


            mensagemFrame.Content =
                mensagemLabel;


            ChatContainer.Add(
                mensagemFrame);
        }


        private void AdicionarMensagemBot(
            string mensagem)
        {
            Frame mensagemFrame =
                new Frame
                {
                    BackgroundColor =
                        Colors.LightGray,

                    CornerRadius = 15,

                    Padding = 12,

                    HorizontalOptions =
                        LayoutOptions.Start,

                    MaximumWidthRequest = 300
                };


            Label mensagemLabel =
                new Label
                {
                    Text = mensagem,

                    FontSize = 16
                };


            mensagemFrame.Content =
                mensagemLabel;


            ChatContainer.Add(
                mensagemFrame);
        }


        private async void AudioButton_Clicked(
            object sender,
            EventArgs e)
        {
            await DisplayAlert(
                "🎤 Áudio",
                "O reconhecimento de voz será implementado em breve!",
                "OK");
        }
    }


}
