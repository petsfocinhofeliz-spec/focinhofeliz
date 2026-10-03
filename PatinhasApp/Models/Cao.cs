using SQLite;

namespace PatinhasApp.Models;

// Representa a tabela "caes" no banco. Cada propriedade vira uma coluna.
[Table("caes")]
public class Cao
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    // Caminho do arquivo da foto salvo dentro do próprio celular.
    public string? CaminhoFoto { get; set; }

    public Sexo Sexo { get; set; }
    public Porte Porte { get; set; }
    public string? Raca { get; set; }
    public string? Cor { get; set; }
    public bool Castrado { get; set; }
    public DateTime DataResgate { get; set; } = DateTime.Today;
    public SituacaoCao Situacao { get; set; } = SituacaoCao.NoProjeto;
    public string? Observacoes { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    // ---- Propriedades só para exibição (o [Ignore] impede que virem coluna) ----

    [Ignore]
    public string SexoTexto => Sexo == Sexo.Macho ? "Macho" : "Fêmea";

    [Ignore]
    public string PorteTexto => Porte switch
    {
        Porte.Pequeno => "Pequeno",
        Porte.Medio => "Médio",
        _ => "Grande"
    };

    [Ignore]
    public string SituacaoTexto => Situacao switch
    {
        SituacaoCao.NoProjeto => "No projeto",
        SituacaoCao.Apoiado => "Apoiado",
        _ => "Adotado"
    };

    [Ignore]
    public string DataResgateTexto => DataResgate.ToString("dd/MM/yyyy");

    [Ignore]
    public string ResumoTexto => $"{SexoTexto} • {PorteTexto} • Resgate: {DataResgateTexto}";

    [Ignore]
    public bool TemFoto => !string.IsNullOrWhiteSpace(CaminhoFoto);
}
