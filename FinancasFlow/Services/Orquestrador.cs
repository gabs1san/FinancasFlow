using FinancasFlow.Data;
using FinancasFlow.Models;



namespace FinancasFlow.Services
{
    public class Orquestrador
    {
        private readonly MemoriaFinanceira _memoria;


    private readonly FinanceiroService _financeiroService;

        private readonly AgenteFinanceiroService _agenteService;

        private readonly InterpretadorFinanceiroService _interpretador;


        public Orquestrador(
            MemoriaFinanceira memoria)
        {
            _memoria = memoria;

            _financeiroService =
                new FinanceiroService();

            _agenteService =
                new AgenteFinanceiroService();

            _interpretador =
                new InterpretadorFinanceiroService();
        }


        public string ProcessarMensagem(
            string mensagem)
        {
            string intencao =
                _agenteService
                    .IdentificarIntencao(mensagem);


            switch (intencao)
            {
                case "CONSULTAR_SALDO":

                    return ConsultarSaldo();


                case "CONSULTAR_FATURA":

                    return ConsultarFatura();


                case "REGISTRAR_DESPESA":

                    return RegistrarDespesa(mensagem);


                case "REGISTRAR_ENTRADA":

                    return RegistrarEntrada(mensagem);


                default:

                    return
                        "Não consegui entender sua mensagem 😕\n\n" +
                        "Tente algo como:\n\n" +
                        "• Gastei 50 reais no mercado usando PIX\n" +
                        "• Recebi 2000 reais de salário\n" +
                        "• Quanto sobrou?\n" +
                        "• Quanto devo no cartão?";
            }
        }


        private string RegistrarDespesa(
            string mensagem)
        {
            decimal valor =
                _interpretador.ExtrairValor(mensagem);


            if (valor <= 0)
            {
                return
                    "⚠️ Não consegui identificar o valor da despesa.\n\n" +
                    "Exemplo:\n" +
                    "'Gastei 50 reais no mercado usando PIX'";
            }


            string categoria =
                _interpretador
                    .IdentificarCategoria(mensagem);


            string pagamento =
                _interpretador
                    .IdentificarFormaPagamento(mensagem);


            Transacao transacao =
                new Transacao
                {
                    Id =
                        _memoria.Transacoes.Count + 1,

                    Tipo = "Despesa",

                    Valor = valor,

                    Categoria = categoria,

                    Descricao = mensagem,

                    FormaPagamento = pagamento,

                    Data = DateTime.Now
                };


            // Se for compra no cartão de crédito

            if (pagamento == "Crédito")
            {
                if (_memoria.Cartoes.Count > 0)
                {
                    CartaoDeCredito cartao =
                        _memoria.Cartoes.First();


                    transacao.CartaoCreditoId =
                        cartao.Id;
                }
            }


            // Adiciona a transação

            _memoria.Transacoes.Add(
                transacao);


            return
                $"✅ Despesa registrada!\n\n" +

                $"💰 Valor: R$ {valor:N2}\n" +

                $"📂 Categoria: {categoria}\n" +

                $"💳 Pagamento: {pagamento}";
        }


        private string RegistrarEntrada(
            string mensagem)
        {
            decimal valor =
                _interpretador.ExtrairValor(mensagem);


            if (valor <= 0)
            {
                return
                    "⚠️ Não consegui identificar o valor da entrada.";
            }


            string categoria =
                _interpretador
                    .IdentificarCategoria(mensagem);


            Transacao transacao =
                new Transacao
                {
                    Id =
                        _memoria.Transacoes.Count + 1,

                    Tipo = "Entrada",

                    Valor = valor,

                    Categoria = categoria,

                    Descricao = mensagem,

                    FormaPagamento = "Não informado",

                    Data = DateTime.Now
                };


            _memoria.Transacoes.Add(
                transacao);


            return
                $"✅ Entrada registrada!\n\n" +

                $"💰 Valor: R$ {valor:N2}\n" +

                $"📂 Categoria: {categoria}";
        }


        private string ConsultarSaldo()
        {
            decimal saldo =
                _financeiroService
                    .CalcularSaldoDisponivel(
                        _memoria.Transacoes);


            return
                $"💰 Seu saldo disponível é:\n\n" +
                $"R$ {saldo:N2}";
        }


        private string ConsultarFatura()
        {
            if (_memoria.Cartoes.Count == 0)
            {
                return
                    "💳 Você ainda não cadastrou um cartão de crédito.";
            }


            CartaoDeCredito cartao =
                _memoria.Cartoes.First();


            decimal fatura =
                _financeiroService
                    .CalcularFatura(
                        _memoria.Transacoes,
                        cartao);


            return
                $"💳 Próxima fatura\n\n" +
                $"Cartão: {cartao.Nome}\n\n" +
                $"Total: R$ {fatura:N2}";
        }
    }

}
