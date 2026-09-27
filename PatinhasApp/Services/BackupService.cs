using PatinhasApp.Data;

namespace PatinhasApp.Services;

// Cuida do backup: exportar (salvar/compartilhar) e importar (restaurar).
// Como o banco inteiro é UM arquivo (.db3), basta copiar esse arquivo.
public class BackupService
{
    private readonly DatabaseService _database;

    public BackupService(DatabaseService database)
    {
        _database = database;
    }

    // Faz uma cópia do banco e abre a tela de compartilhamento do Android
    // (a pessoa pode enviar para o WhatsApp, Google Drive, e-mail, etc.).
    public async Task ExportarAsync()
    {
        await _database.InicializarAsync();

        var nomeBackup = $"backup_patinhas_{DateTime.Now:yyyy-MM-dd_HHmm}.db3";
        var destino = Path.Combine(FileSystem.CacheDirectory, nomeBackup);

        File.Copy(_database.CaminhoBanco, destino, overwrite: true);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Backup do Patinhas do Bem",
            File = new ShareFile(destino)
        });
    }

    // Deixa a pessoa escolher um arquivo .db3 e substitui o banco atual por ele.
    // Retorna true se um arquivo foi de fato importado.
    public async Task<bool> ImportarAsync()
    {
        var arquivo = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Escolha o arquivo de backup (.db3)"
        });

        if (arquivo == null)
            return false;

        await _database.FecharAsync();
        File.Copy(arquivo.FullPath, _database.CaminhoBanco, overwrite: true);
        await _database.InicializarAsync();

        return true;
    }
}
