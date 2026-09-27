using SQLite;

namespace PatinhasApp.Models;

// Tabela "adocoes": guarda os dados de cada adoção no mesmo lugar.
[Table("adocoes")]
public class Adocao
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Liga a adoção ao cão (Cao.Id).
    [Indexed]
    public int CaoId { get; set; }

    // Cópia do nome do animal no momento da adoção (facilita a listagem).
    public string NomeAnimal { get; set; } = string.Empty;

    public DateTime DataAdocao { get; set; } = DateTime.Today;
    public string NomeAdotante { get; set; } = string.Empty;
    public string Contato { get; set; } = string.Empty;
    public string? Observacoes { get; set; }

    [Ignore]
    public string DataTexto => DataAdocao.ToString("dd/MM/yyyy");
}
