using FinancasFlow.Models;

namespace FinancasFlow.Services
{
    public class FinanceiroService
    {
        public double CalcularSaldo(
            List<Transacao> transacoes)
        {
            double entradas = transacoes
                .Where(t => t.Tipo == "Entrada")
                .Sum(t => t.Valor);

            double despesasPagas = transacoes
                .Where(t =>
                    t.Tipo == "Despesa" &&
                    t.FormaPagamento != "Cartão de Crédito")
                .Sum(t => t.Valor);


            return entradas - despesasPagas;
        }


        public double CalcularFaturaCartao(
            List<Transacao> transacoes)
        {
            return transacoes
                .Where(t =>
                    t.Tipo == "Despesa" &&
                    t.FormaPagamento == "Cartão de Crédito")
                .Sum(t => t.Valor);
        }

        private void ConsultarSaldo()
        {
            decimal saldo =
                financeiro.CalcularSaldo(transacoes);


            AdicionarMensagemBot(

                $"💰 Seu saldo disponível é:\n\n" +

                $"R$ {saldo:F2}"
            );
        }

        private void ConsultarFatura()
        {
            decimal fatura =
                financeiro.CalcularFaturaCartao(transacoes);


            AdicionarMensagemBot(

                $"💳 Sua próxima fatura do cartão está em:\n\n" +

                $"R$ {fatura:F2}"
            );
        }
    }
}