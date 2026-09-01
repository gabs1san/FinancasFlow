namespace FinancasFlow.Models
{
    public class UsuarioFinanceiro
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        // Dia em que normalmente recebe salário
        public int DiaRecebimentoSalario { get; set; }

        // Salário esperado
        public decimal SalarioMensal { get; set; }
    }
}