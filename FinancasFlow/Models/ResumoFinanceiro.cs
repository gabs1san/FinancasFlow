
namespace FinancasFlow.Models;

public class ResumoFinanceiro
{
    // Propriedades utilizadas pelo FinanceiroService
    public decimal TotalEntradas { get; set; }

    public decimal TotalDespesasPagas { get; set; }

    public decimal SaldoDisponivel { get; set; }

    public decimal TotalCartaoCredito { get; set; }

    public decimal ProximaFatura { get; set; }

    public decimal LimiteDisponivel { get; set; }

    // Propriedades utilizadas pelo novo dashboard
    public decimal TotalReceitas { get; set; }

    public decimal TotalDespesas { get; set; }

    public decimal Saldo =>
        TotalReceitas - TotalDespesas;

    public Dictionary<string, decimal> DespesasPorCategoria
    { get; set; } = new();
}
