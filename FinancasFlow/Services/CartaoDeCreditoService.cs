using FinancasFlow.Models;

namespace FinancasFlow.Services
{
    public class CartaoCreditoService
    {
        public DateTime CalcularVencimentoFatura(
            DateTime dataCompra,
            CartaoCredito cartao)
        {
            int mes = dataCompra.Month;
            int ano = dataCompra.Year;


            // Compra antes do fechamento

            if (dataCompra.Day <= cartao.DiaFechamento)
            {
                mes++;
            }
            else
            {
                mes += 2;
            }


            if (mes > 12)
            {
                mes -= 12;

                ano++;
            }


            return new DateTime(
                ano,
                mes,
                cartao.DiaVencimento);
        }
    }
}