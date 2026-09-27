using PatinhasApp.Models;
using SQLite;

namespace PatinhasApp.Data;

// Centraliza TUDO que fala com o banco SQLite.
// As telas nunca acessam o banco direto: elas chamam estes métodos.
public class DatabaseService
{
    public const string NomeArquivo = "patinhas.db3";

    // Pasta privada do app dentro do celular (ninguém mais acessa).
    public string CaminhoBanco => Path.Combine(FileSystem.AppDataDirectory, NomeArquivo);

    private SQLiteAsyncConnection? _conexao;

    // Abre a conexão e cria as tabelas na primeira vez que o app roda.
    public async Task InicializarAsync()
    {
        if (_conexao != null) return;

        _conexao = new SQLiteAsyncConnection(
            CaminhoBanco,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

        await _conexao.CreateTableAsync<Cao>();
        await _conexao.CreateTableAsync<RegistroSaude>();
        await _conexao.CreateTableAsync<Adocao>();
    }

    // Fecha a conexão (necessário antes de substituir o arquivo num restore).
    public async Task FecharAsync()
    {
        if (_conexao == null) return;
        await _conexao.CloseAsync();
        _conexao = null;
    }

    private async Task<SQLiteAsyncConnection> ObterConexaoAsync()
    {
        await InicializarAsync();
        return _conexao!;
    }

    // ============ CÃES ============

    public async Task<List<Cao>> ObterCaesAsync()
    {
        var db = await ObterConexaoAsync();
        return await db.Table<Cao>().OrderBy(c => c.Nome).ToListAsync();
    }

    public async Task<Cao?> ObterCaoAsync(int id)
    {
        var db = await ObterConexaoAsync();
        return await db.Table<Cao>().Where(c => c.Id == id).FirstOrDefaultAsync();
    }

    // Salva serve tanto para criar (Id == 0) quanto para editar.
    public async Task<int> SalvarCaoAsync(Cao cao)
    {
        var db = await ObterConexaoAsync();
        if (cao.Id == 0)
            return await db.InsertAsync(cao);
        return await db.UpdateAsync(cao);
    }

    public async Task<int> ExcluirCaoAsync(Cao cao)
    {
        var db = await ObterConexaoAsync();

        // Remove também o histórico de saúde e a adoção ligados a este cão.
        await db.Table<RegistroSaude>().Where(r => r.CaoId == cao.Id).DeleteAsync();
        await db.Table<Adocao>().Where(a => a.CaoId == cao.Id).DeleteAsync();

        return await db.DeleteAsync(cao);
    }

    // ============ REGISTROS DE SAÚDE ============

    public async Task<List<RegistroSaude>> ObterRegistrosSaudeAsync(int caoId)
    {
        var db = await ObterConexaoAsync();
        return await db.Table<RegistroSaude>()
            .Where(r => r.CaoId == caoId)
            .OrderByDescending(r => r.Data)
            .ToListAsync();
    }

    // Todos os registros (usado pela tela de Alertas).
    public async Task<List<RegistroSaude>> ObterTodosRegistrosSaudeAsync()
    {
        var db = await ObterConexaoAsync();
        return await db.Table<RegistroSaude>().ToListAsync();
    }

    public async Task<int> SalvarRegistroSaudeAsync(RegistroSaude registro)
    {
        var db = await ObterConexaoAsync();
        if (registro.Id == 0)
            return await db.InsertAsync(registro);
        return await db.UpdateAsync(registro);
    }

    public async Task<int> ExcluirRegistroSaudeAsync(RegistroSaude registro)
    {
        var db = await ObterConexaoAsync();
        return await db.DeleteAsync(registro);
    }

    // ============ ADOÇÕES ============

    public async Task<List<Adocao>> ObterAdocoesAsync()
    {
        var db = await ObterConexaoAsync();
        return await db.Table<Adocao>().OrderByDescending(a => a.DataAdocao).ToListAsync();
    }

    public async Task<Adocao?> ObterAdocaoPorCaoAsync(int caoId)
    {
        var db = await ObterConexaoAsync();
        return await db.Table<Adocao>().Where(a => a.CaoId == caoId).FirstOrDefaultAsync();
    }

    public async Task<int> SalvarAdocaoAsync(Adocao adocao)
    {
        var db = await ObterConexaoAsync();
        if (adocao.Id == 0)
            return await db.InsertAsync(adocao);
        return await db.UpdateAsync(adocao);
    }

    public async Task<int> ExcluirAdocaoAsync(Adocao adocao)
    {
        var db = await ObterConexaoAsync();
        return await db.DeleteAsync(adocao);
    }
}
