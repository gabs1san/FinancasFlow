using FinancasFlow.Models;


namespace FinancasFlow.Data
{
    public class MemoriaFinanceira
    {
        public List<Transacao> Transacoes { get; set; } = new();

        public List<CartaoCredito> Cartoes { get; set; } = new();

        public UsuarioFinanceiro Usuario { get; set; } =
            new UsuarioFinanceiro();
    }
}