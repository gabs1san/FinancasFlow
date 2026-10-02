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
            _memoria = new MemoriaFinanceira();

            // Cria o agente financeiro
            _agente = new Orquestrador(_memoria);

            ConfigurarDadosIniciais();
        }


        // =========================================
        // DADOS INICIAIS
        // =========================================

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


        // =========================================
        // ENVIO DE MENSAGEM
        // =========================================

        private async void EnviarButton_Clicked(
            object sender,
            EventArgs e)
        {
            string mensagem = txtMensagem.Text;

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
                _agente.ProcessarMensagem(mensagem);

            // Mostra resposta do agente
            AdicionarMensagemBot(resposta);

            // Futuramente podemos colocar aqui
            // o ScrollTo para descer o chat automaticamente.
        }


        // =========================================
        // MENSAGEM DO USUÁRIO
        // =========================================

        private void AdicionarMensagemUsuario(
            string mensagem)
        {
            Frame mensagemFrame =
                new Frame
                {
                    // Azul do usuário
                    BackgroundColor =
                        Color.FromArgb("#285476"),

                    CornerRadius = 12,

                    Padding =
                        new Thickness(12, 9),

                    // Mensagem fica à direita
                    HorizontalOptions =
                        LayoutOptions.End,

                    MaximumWidthRequest = 290,

                    HasShadow = false
                };


            Label mensagemLabel =
                new Label
                {
                    Text = mensagem,

                    FontSize = 15,

                    TextColor =
                        Colors.White
                };


            mensagemFrame.Content =
                mensagemLabel;


            ChatContainer.Add(
                mensagemFrame);
        }


        // =========================================
        // MENSAGEM DO AGENTE
        // =========================================

        private void AdicionarMensagemBot(
            string mensagem)
        {
            Frame mensagemFrame =
                new Frame
                {
                    // Azul mais claro para o agente
                    BackgroundColor =
                        Color.FromArgb("#173750"),

                    CornerRadius = 12,

                    Padding =
                        new Thickness(12, 10),

                    // Mensagem fica à esquerda
                    HorizontalOptions =
                        LayoutOptions.Start,

                    MaximumWidthRequest = 320,

                    HasShadow = false
                };


            VerticalStackLayout conteudo =
                new VerticalStackLayout
                {
                    Spacing = 5
                };


            // Cabeçalho do agente

            HorizontalStackLayout cabecalho =
                new HorizontalStackLayout
                {
                    Spacing = 6
                };


            Label icone =
                new Label
                {
                    Text = "🤖",

                    FontSize = 16
                };


            Label nome =
                new Label
                {
                    Text = "FinFlow Assistant",

                    FontSize = 13,

                    FontAttributes =
                        FontAttributes.Bold,

                    TextColor =
                        Color.FromArgb("#72D6FF"),

                    VerticalOptions =
                        LayoutOptions.Center
                };


            cabecalho.Add(icone);
            cabecalho.Add(nome);


            // Texto da resposta

            Label mensagemLabel =
                new Label
                {
                    Text = mensagem,

                    FontSize = 15,

                    TextColor =
                        Color.FromArgb("#E8F2F8")
                };


            conteudo.Add(cabecalho);
            conteudo.Add(mensagemLabel);


            mensagemFrame.Content =
                conteudo;


            ChatContainer.Add(
                mensagemFrame);
        }


        // =========================================
        // ÁUDIO
        // =========================================

        private async void AudioButton_Clicked(
            object sender,
            EventArgs e)
        {
            await DisplayAlert(
                "🎤 Áudio",
                "O reconhecimento de voz será implementado em breve!",
                "OK");
        }


        // =========================================
        // NAVEGAÇÃO
        // =========================================

        private async void Inicio_Tapped(
            object sender,
            TappedEventArgs e)
        {
            await Shell.Current.GoToAsync(
                "//MainPage");
        }


        private async void Resumo_Tapped(
            object sender,
            TappedEventArgs e)
        {
            await Shell.Current.GoToAsync(
                nameof(ResumoPage));
        }


        private async void Adicionar_Tapped(
            object sender,
            TappedEventArgs e)
        {
            await Shell.Current.GoToAsync(
                nameof(AdicionarPage));
        }


        private async void Perfil_Tapped(
            object sender,
            TappedEventArgs e)
        {
            await Shell.Current.GoToAsync(
                nameof(PerfilPage));
        }
    }
}