namespace FinancasFlow.Models
{
    public class Transacao
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public double Valor { get; set; }

        public string Categoria { get; set; }

        public string Descricao { get; set; }

        public string FormaPagamento { get; set; }

        public DateTime Data { get; set; }
    }
}