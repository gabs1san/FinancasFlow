
namespace FinancasFlow.Models
{
    public class MensagemChat
    {
        public string Texto { get; set; } = string.Empty;

        public bool EhUsuario { get; set; }

        public MensagemChat(string texto, bool ehUsuario)
        {
            Texto = texto;
            EhUsuario = ehUsuario;
        }
    }
}
