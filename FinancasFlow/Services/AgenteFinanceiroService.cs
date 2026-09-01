namespace FinancasFlow.Services
{
    public class AgenteFinanceiroService
    {
        public string IdentificarIntencao(string mensagem)
        {
            mensagem = mensagem.ToLower();


            // CONSULTAR SALDO

            if (
                mensagem.Contains("quanto sobrou") ||
                mensagem.Contains("meu saldo") ||
                mensagem.Contains("quanto tenho"))
            {
                return "CONSULTAR_SALDO";
            }


            // CONSULTAR FATURA

            if (
                mensagem.Contains("fatura") ||
                mensagem.Contains("quanto devo no cartão") ||
                mensagem.Contains("quanto devo no cartao"))
            {
                return "CONSULTAR_FATURA";
            }


            // REGISTRAR DESPESA

            if (
                mensagem.Contains("gastei") ||
                mensagem.Contains("paguei") ||
                mensagem.Contains("comprei"))
            {
                return "REGISTRAR_DESPESA";
            }


            // REGISTRAR ENTRADA

            if (
                mensagem.Contains("recebi") ||
                mensagem.Contains("ganhei"))
            {
                return "REGISTRAR_ENTRADA";
            }


            return "DESCONHECIDO";
        }
    }
}