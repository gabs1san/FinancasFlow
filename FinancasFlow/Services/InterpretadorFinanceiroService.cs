using System.Globalization;
using System.Text.RegularExpressions;

namespace FinancasFlow.Services
{
    public class InterpretadorFinanceiroService
    {
        public decimal ExtrairValor(
            string mensagem)
        {
            Match match =
                Regex.Match(
                    mensagem,
                    @"\d+([.,]\d+)?");


            if (!match.Success)
            {
                return 0;
            }


            string valor =
                match.Value
                    .Replace(",", ".");


            return decimal.Parse(
                valor,
                CultureInfo.InvariantCulture);
        }


        public string IdentificarCategoria(
            string mensagem)
        {
            mensagem = mensagem.ToLower();


            // ALIMENTAÇÃO

            if (
                mensagem.Contains("mercado") ||
                mensagem.Contains("supermercado") ||
                mensagem.Contains("restaurante") ||
                mensagem.Contains("lanche") ||
                mensagem.Contains("ifood"))
            {
                return "Alimentação";
            }


            // TRANSPORTE

            if (
                mensagem.Contains("uber") ||
                mensagem.Contains("99") ||
                mensagem.Contains("ônibus") ||
                mensagem.Contains("onibus") ||
                mensagem.Contains("metrô") ||
                mensagem.Contains("metro") ||
                mensagem.Contains("combustível"))
            {
                return "Transporte";
            }


            // ENTRETENIMENTO

            if (
                mensagem.Contains("netflix") ||
                mensagem.Contains("spotify") ||
                mensagem.Contains("cinema") ||
                mensagem.Contains("jogo"))
            {
                return "Entretenimento";
            }


            // SALÁRIO

            if (
                mensagem.Contains("salário") ||
                mensagem.Contains("salario"))
            {
                return "Salário";
            }


            // MORADIA

            if (
                mensagem.Contains("aluguel") ||
                mensagem.Contains("luz") ||
                mensagem.Contains("água") ||
                mensagem.Contains("agua"))
            {
                return "Moradia";
            }


            return "Outros";
        }


        public string IdentificarFormaPagamento(
            string mensagem)
        {
            mensagem = mensagem.ToLower();


            if (mensagem.Contains("pix"))
            {
                return "PIX";
            }


            if (mensagem.Contains("dinheiro"))
            {
                return "Dinheiro";
            }


            if (
                mensagem.Contains("débito") ||
                mensagem.Contains("debito"))
            {
                return "Débito";
            }


            if (
                mensagem.Contains("crédito") ||
                mensagem.Contains("credito"))
            {
                return "Crédito";
            }


            return "Não informado";
        }
    }
}