namespace FinancasFlow.Models;

public class ReceitaExtratoItem
{
    public int Id { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public DateTime Data { get; set; }
}