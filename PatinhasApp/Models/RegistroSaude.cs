using SQLite;

namespace PatinhasApp.Models;

// Tabela "registros_saude": vacinas, vermífugos e problemas de saúde de cada cão.
[Table("registros_saude")]
public class RegistroSaude
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Liga este registro a um cão específico (Cao.Id).
    [Indexed]
    public int CaoId { get; set; }

    public TipoRegistroSaude Tipo { get; set; }

    // Ex.: "V10", "Antirrábica", "Vermífugo X", "Cinomose"
    public string Descricao { get; set; } = string.Empty;

    // Data em que foi aplicado/registrado.
    public DateTime Data { get; set; } = DateTime.Today;

    // Próxima data prevista (usada nos alertas). Pode ficar vazia.
    public DateTime? ProximaData { get; set; }

    public string? Observacoes { get; set; }

    // ---- Só para exibição ----

    [Ignore]
    public string TipoTexto => Tipo switch
    {
        TipoRegistroSaude.Vacina => "Vacina",
        TipoRegistroSaude.Vermifugo => "Vermífugo",
        _ => "Problema de saúde"
    };

    [Ignore]
    public string DataTexto => Data.ToString("dd/MM/yyyy");

    [Ignore]
    public string ProximaDataTexto => ProximaData?.ToString("dd/MM/yyyy") ?? "—";

    [Ignore]
    public bool TemProximaData => ProximaData.HasValue;

    // Quantos dias faltam para a próxima data (negativo = já venceu).
    [Ignore]
    public int? DiasParaVencer =>
        ProximaData.HasValue
            ? (int)(ProximaData.Value.Date - DateTime.Today).TotalDays
            : (int?)null;

    [Ignore]
    public string StatusVencimento
    {
        get
        {
            if (!ProximaData.HasValue) return string.Empty;
            var dias = DiasParaVencer!.Value;
            if (dias < 0) return $"Vencido há {-dias} dia(s)";
            if (dias == 0) return "Vence hoje";
            return $"Vence em {dias} dia(s)";
        }
    }
}
