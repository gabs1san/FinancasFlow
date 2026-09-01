namespace FinancasFlow.Models
{
    public class Transacao
    {
        public int Id { get; set; }

        // Entrada ou Despesa
        public string Tipo { get; set; } = string.Empty;

        // Ex: Salário, Alimentação, Transporte
        public string Categoria { get; set; } = string.Empty;

        // Ex: Mercado, Uber, Restaurante
        public string Descricao { get; set; } = string.Empty;

        // Valor financeiro
        public decimal Valor { get; set; }

        // PIX, Dinheiro, Débito, Crédito
        public string FormaPagamento { get; set; } = string.Empty;

        // Data da transação
        public DateTime Data { get; set; }

        // Caso seja uma compra no cartão
        public int? CartaoCreditoId { get; set; }

        // Caso seja uma compra no crédito
        public DateTime? DataFatura { get; set; }
    }
}