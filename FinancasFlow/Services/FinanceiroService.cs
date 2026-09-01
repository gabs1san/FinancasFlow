using FinancasFlow.Models;

namespace FinancasFlow.Services
{
    public class FinanceiroService
    {
        public decimal CalcularTotalEntradas(
            List<Transacao> transacoes)
        {
            return transacoes
                .Where(t => t.Tipo == "Entrada")
                .Sum(t => t.Valor);
        }


        public decimal CalcularDespesasPagas(
            List<Transacao> transacoes)
        {
            return transacoes
                .Where(t =>
                    t.Tipo == "Despesa" &&
                    t.FormaPagamento != "Crédito")
                .Sum(t => t.Valor);
        }


        public decimal CalcularSaldoDisponivel(
            List<Transacao> transacoes)
        {
            decimal entradas =
                CalcularTotalEntradas(transacoes);

            decimal despesas =
                CalcularDespesasPagas(transacoes);


            return entradas - despesas;
        }


        public decimal CalcularFatura(
            List<Transacao> transacoes,
            CartaoCredito cartao)
        {
            return transacoes
                .Where(t =>
                    t.Tipo == "Despesa" &&
                    t.CartaoCreditoId == cartao.Id)
                .Sum(t => t.Valor);
        }


        public decimal CalcularLimiteDisponivel(
            List<Transacao> transacoes,
            CartaoCredito cartao)
        {
            decimal totalUtilizado =
                CalcularFatura(transacoes, cartao);


            return cartao.Limite - totalUtilizado;
        }


        public ResumoFinanceiro GerarResumo(
            List<Transacao> transacoes,
            CartaoCredito? cartao)
        {
            decimal entradas =
                CalcularTotalEntradas(transacoes);

            decimal despesasPagas =
                CalcularDespesasPagas(transacoes);

            decimal saldo =
                entradas - despesasPagas;


            ResumoFinanceiro resumo =
                new ResumoFinanceiro
                {
                    TotalEntradas = entradas,

                    TotalDespesasPagas = despesasPagas,

                    SaldoDisponivel = saldo
                };


            if (cartao != null)
            {
                resumo.TotalCartaoCredito =
                    CalcularFatura(
                        transacoes,
                        cartao);


                resumo.ProximaFatura =
                    resumo.TotalCartaoCredito;


                resumo.LimiteDisponivel =
                    CalcularLimiteDisponivel(
                        transacoes,
                        cartao);
            }


            return resumo;
        }
    }
}