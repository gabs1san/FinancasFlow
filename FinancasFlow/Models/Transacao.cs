namespace FinancasFlow.Models
{
    public class Transacao
    {
        public int Id { get; set; }

        public string Tipo { get; set; }
        // Entrada ou Despesa

        public double Valor { get; set; }

        public string Categoria { get; set; }

        public string Descricao { get; set; }

        public string FormaPagamento { get; set; }

        public DateTime Data { get; set; }

        public bool Pago { get; set; }
    }
}