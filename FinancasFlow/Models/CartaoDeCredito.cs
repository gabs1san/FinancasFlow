namespace FinancasFlow.Models
{
    public class CartaoCredito
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public decimal Limite { get; set; }

        public int DiaFechamento { get; set; }

        public int DiaVencimento { get; set; }
    }
}