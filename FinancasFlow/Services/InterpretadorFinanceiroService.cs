using System.Globalization;
using System.Text.RegularExpressions;

namespace FinancasFlow.Services
{
    public class InterpretadorFinanceiroService
    {

        public string IdentificarCategoria(string mensagem)
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                return "Outros";

            mensagem = mensagem.ToLowerInvariant();

            // ALIMENTAÇÃO
            if (mensagem.Contains("mercado") ||
                mensagem.Contains("supermercado") ||
                mensagem.Contains("restaurante") ||
                mensagem.Contains("lanche") ||
                mensagem.Contains("ifood"))
            {
                return "Alimentação";
            }

            // TRANSPORTE
            if (mensagem.Contains("uber") ||
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
            if (mensagem.Contains("netflix") ||
                mensagem.Contains("spotify") ||
                mensagem.Contains("cinema") ||
                mensagem.Contains("jogo"))
            {
                return "Entretenimento";
            }

            // SALÁRIO
            if (mensagem.Contains("salário") ||
                mensagem.Contains("salario"))
            {
                return "Salário";
            }

            // MORADIA
            if (mensagem.Contains("aluguel") ||
                mensagem.Contains("luz") ||
                mensagem.Contains("água") ||
                mensagem.Contains("agua"))
            {
                return "Moradia";
            }

            return "Outros";
        }

        public decimal ExtrairValor(string mensagem)
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                return 0;

            var match = Regex.Match(
                mensagem,
                @"(?<!\d)\d{1,3}(?:\.\d{3})+(?:,\d{1,2})?(?!\d)|(?<!\d)\d+(?:[.,]\d{1,2})?(?!\d)"
            );

            if (!match.Success)
                return 0;

            string valor = match.Value;

            // Formato brasileiro com separador de milhar
            if (valor.Contains('.') && valor.Contains(','))
            {
                valor = valor.Replace(".", "").Replace(",", ".");
            }
            else if (valor.Contains(','))
            {
                valor = valor.Replace(",", ".");
            }
            else if (Regex.IsMatch(valor, @"^\d{1,3}(?:\.\d{3})+$"))
            {
                valor = valor.Replace(".", "");
            }

            return decimal.TryParse(
                valor,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out decimal resultado)
                ? resultado
                : 0;
        }



        public string IdentificarFormaPagamento(string mensagem)
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                return "Não informado";

            mensagem = mensagem.ToLowerInvariant();

            if (Regex.IsMatch(mensagem, @"\bpix\b"))
                return "PIX";

            if (Regex.IsMatch(mensagem, @"\b(crédito|credito)\b"))
                return "Crédito";

            if (Regex.IsMatch(mensagem, @"\b(débito|debito)\b"))
                return "Débito";

            if (Regex.IsMatch(mensagem, @"\bdinheiro\b"))
                return "Dinheiro";

            return "Não informado";
        }

    }
}