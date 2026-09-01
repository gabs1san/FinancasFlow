using FinancasFlow.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FinancasFlow.Services
{
    public class AgenteFinanceiroService
    {
        public Transacao ProcessarMensagem(string mensagem)
        {
            mensagem = mensagem.ToLower();


            Transacao transacao = new Transacao();


            // TIPO

            if (mensagem.Contains("gastei"))
            {
                transacao.Tipo = "Despesa";
            }

            else if (
                mensagem.Contains("recebi") ||
                mensagem.Contains("ganhei"))
            {
                transacao.Tipo = "Entrada";
            }


            // VALOR

            transacao.Valor = ExtrairValor(mensagem);


            // CATEGORIA

            transacao.Categoria =
                IdentificarCategoria(mensagem);


            // PAGAMENTO

            transacao.FormaPagamento =
                IdentificarPagamento(mensagem);


            // DESCRIÇÃO

            transacao.Descricao = mensagem;


            // DATA

            transacao.Data = DateTime.Now;


            return transacao;
        }


        private double ExtrairValor(string mensagem)
        {
            Match match =
                Regex.Match(mensagem, @"\d+([.,]\d+)?");


            if (match.Success)
            {
                string valorTexto =
                    match.Value.Replace(",", ".");


                return double.Parse(
                    valorTexto,
                    CultureInfo.InvariantCulture);
            }


            return 0;
        }


        private string IdentificarCategoria(string mensagem)
        {
            if (
                mensagem.Contains("mercado") ||
                mensagem.Contains("supermercado") ||
                mensagem.Contains("restaurante") ||
                mensagem.Contains("lanche"))
            {
                return "Alimentação";
            }


            if (
                mensagem.Contains("uber") ||
                mensagem.Contains("ônibus") ||
                mensagem.Contains("metro") ||
                mensagem.Contains("metrô"))
            {
                return "Transporte";
            }


            if (
                mensagem.Contains("netflix") ||
                mensagem.Contains("spotify") ||
                mensagem.Contains("jogo"))
            {
                return "Entretenimento";
            }


            if (
                mensagem.Contains("salário") ||
                mensagem.Contains("salario"))
            {
                return "Salário";
            }


            return "Outros";
        }


        private string IdentificarPagamento(string mensagem)
        {
            if (mensagem.Contains("pix"))
            {
                return "PIX";
            }


            if (
                mensagem.Contains("cartão") ||
                mensagem.Contains("cartao"))
            {
                return "Cartão";
            }


            if (mensagem.Contains("dinheiro"))
            {
                return "Dinheiro";
            }


            if (mensagem.Contains("débito"))
            {
                return "Cartão de Débito";
            }


            if (mensagem.Contains("crédito"))
            {
                return "Cartão de Crédito";
            }


            return "Não informado";

        }
        private void ProcessarMensagem(string mensagem)
        {
            mensagem = mensagem.ToLower();


            if (
                mensagem.Contains("quanto sobrou") ||
                mensagem.Contains("quanto tenho") ||
                mensagem.Contains("meu saldo"))
            {
                ConsultarSaldo();
            }


            else if (
                mensagem.Contains("fatura") ||
                mensagem.Contains("cartão") ||
                mensagem.Contains("cartao"))
            {
                ConsultarFatura();
            }


            else if (mensagem.Contains("gastei"))
            {
                ProcessarDespesa(mensagem);
            }


            else if (
                mensagem.Contains("recebi") ||
                mensagem.Contains("ganhei"))
            {
                ProcessarEntrada(mensagem);
            }


            else
            {
                AdicionarMensagemBot(
                    "Não consegui entender sua mensagem 😕");
            }
        }
    }
}