namespace FinancasFlow.Models
{
    public class ResumoFinanceiro
    {
        public decimal TotalEntradas { get; set; }

        public decimal TotalDespesasPagas { get; set; }

        public decimal TotalCartaoCredito { get; set; }

        public decimal SaldoDisponivel { get; set; }

        public decimal ProximaFatura { get; set; }

        public decimal LimiteDisponivel { get; set; }
    }
}