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

        private readonly TransacaoService _transacaoService;


        public Orquestrador(MemoriaFinanceira memoria, TransacaoService transacaoService)
        {
            _memoria = memoria;
            _transacaoService = transacaoService;

            _financeiroService =
                new FinanceiroService();

            _agenteService =
                new AgenteFinanceiroService();

            _interpretador =
                new InterpretadorFinanceiroService();
        }


        public async Task<string> ProcessarMensagemAsync(string mensagem)
        {
            string intencao = _agenteService.IdentificarIntencao(mensagem);

            switch (intencao)
            {
                case "CONSULTAR_SALDO":
                    return await ConsultarSaldoAsync();

                case "CONSULTAR_FATURA":
                    return await ConsultarFaturaAsync();

                case "REGISTRAR_DESPESA":
                    return await RegistrarDespesaAsync(mensagem);

                case "REGISTRAR_ENTRADA":
                    return await RegistrarEntradaAsync(mensagem);

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


        private async Task<string> RegistrarDespesaAsync(string mensagem)
        {
            decimal valor = _interpretador.ExtrairValor(mensagem);

            if (valor <= 0)
            {
                return "⚠️ Não consegui identificar o valor da despesa.";
            }

            string categoria = _interpretador.IdentificarCategoria(mensagem);
            string pagamento = _interpretador.IdentificarFormaPagamento(mensagem);

            var transacao = new Transacao
            {
                Tipo = "Despesa",
                Valor = valor,
                Categoria = categoria,
                Descricao = mensagem,
                FormaPagamento = pagamento,
                Data = DateTime.Now
            };

            if (pagamento == "Crédito" && _memoria.Cartoes.Count > 0)
            {
                transacao.CartaoCreditoId = _memoria.Cartoes.First().Id;
            }

            await _transacaoService.AdicionarAsync(transacao);

            return
                $"✅ Despesa registrada!\n\n" +
                $"💰 Valor: R$ {valor:N2}\n" +
                $"📂 Categoria: {categoria}\n" +
                $"💳 Pagamento: {pagamento}";
        }


        private async Task<string> RegistrarEntradaAsync(string mensagem)
        {
            decimal valor = _interpretador.ExtrairValor(mensagem);

            if (valor <= 0)
            {
                return "⚠️ Não consegui identificar o valor da entrada.";
            }

            string categoria = _interpretador.IdentificarCategoria(mensagem);

            var transacao = new Transacao
            {
                Tipo = "Receita",
                Valor = valor,
                Categoria = categoria,
                Descricao = mensagem,
                FormaPagamento = "Não informado",
                Data = DateTime.Now
            };

            await _transacaoService.AdicionarAsync(transacao);

            return
                $"✅ Receita registrada!\n\n" +
                $"💰 Valor: R$ {valor:N2}\n" +
                $"📂 Categoria: {categoria}";
        }


        private async Task<string> ConsultarSaldoAsync()
        {
            var hoje = DateTime.Today;

            var resumo = await _transacaoService.ObterResumoMensalAsync(
                hoje.Year,
                hoje.Month);

            decimal saldo = resumo.TotalReceitas - resumo.TotalDespesas;

            return
                $"💰 Seu saldo disponível neste mês é:\n\n" +
                $"R$ {saldo:N2}";
        }


        private async Task<string> ConsultarFaturaAsync()
        {
            if (_memoria.Cartoes.Count == 0)
            {
                return "💳 Você ainda não cadastrou um cartão de crédito.";
            }

            var cartao = _memoria.Cartoes.First();

            var transacoes = await _transacaoService.ListarAsync();

            decimal fatura = _financeiroService.CalcularFatura(
                transacoes,
                cartao);

            return
                $"💳 Fatura do cartão\n\n" +
                $"Cartão: {cartao.Nome}\n\n" +
                $"Total: R$ {fatura:N2}";
        }
    }

}
