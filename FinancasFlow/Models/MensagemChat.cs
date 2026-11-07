
namespace FinancasFlow.Models
{
    public class MensagemChat
    {
        public int Id { get; set; }

        public string Texto { get; set; } = string.Empty;

        public bool EnviadaPeloUsuario { get; set; }

        public DateTime DataEnvio { get; set; } = DateTime.Now;

        // Construtor vazio para o Entity Framework
        public MensagemChat()
        {
        }

        // Construtor utilizado pelo MainPageViewModel
        public MensagemChat(string texto, bool enviadaPeloUsuario)
        {
            Texto = texto;
            EnviadaPeloUsuario = enviadaPeloUsuario;
            DataEnvio = DateTime.Now;
        }
    }
}
