using FinancasFlow.Data;
using FinancasFlow.Models;
using FinancasFlow.Services;


namespace FinancasFlow.Services
{
    public class Orquestrador
    {
        private readonly MemoriaFinanceira _memoria;

        private readonly FinanceiroService _financeiroService;

        private readonly AgenteFinanceiroService _agenteService;


        public Orquestrador(
            MemoriaFinanceira memoria)
        {
            _memoria = memoria;

            _financeiroService =
                new FinanceiroService();

            _agenteService =
                new AgenteFinanceiroService();
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

                    return
                        "Vamos registrar sua despesa.";


                case "REGISTRAR_ENTRADA":

                    return
                        "Vamos registrar sua entrada.";


                default:

                    return
                        "Não consegui entender sua mensagem 😕";
            }
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
                    "Você ainda não cadastrou um cartão de crédito.";
            }


            CartaoCredito cartao =
                _memoria.Cartoes.First();


            decimal fatura =
                _financeiroService
                    .CalcularFatura(
                        _memoria.Transacoes,
                        cartao);


            return
                $"💳 Próxima fatura:\n\n" +
                $"R$ {fatura:N2}";
        }
    }
}