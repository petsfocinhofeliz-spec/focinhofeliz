using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PatinhasApp.Data;
using PatinhasApp.Services;

namespace PatinhasApp.ViewModels;

// Tela "Ajustes": mostra totais e cuida do backup (exportar/restaurar).
public partial class AjustesViewModel : BaseViewModel
{
    private readonly BackupService _backup;
    private readonly DatabaseService _database;

    [ObservableProperty] private int totalCaes;
    [ObservableProperty] private int totalAdocoes;

    public AjustesViewModel(BackupService backup, DatabaseService database)
    {
        _backup = backup;
        _database = database;
        Titulo = "Ajustes";
    }

    [RelayCommand]
    public async Task CarregarAsync()
    {
        var caes = await _database.ObterCaesAsync();
        var adocoes = await _database.ObterAdocoesAsync();
        TotalCaes = caes.Count;
        TotalAdocoes = adocoes.Count;
    }

    [RelayCommand]
    private async Task ExportarAsync()
    {
        try
        {
            await _backup.ExportarAsync();
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Erro", $"Não foi possível exportar: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task ImportarAsync()
    {
        var confirma = await Application.Current!.MainPage!.DisplayAlert(
            "Restaurar backup",
            "Isso vai SUBSTITUIR todos os dados atuais pelos dados do arquivo escolhido. Deseja continuar?",
            "Sim, restaurar", "Cancelar");
        if (!confirma) return;

        try
        {
            var ok = await _backup.ImportarAsync();
            if (ok)
            {
                await CarregarAsync();
                await Application.Current!.MainPage!.DisplayAlert(
                    "Pronto", "Backup restaurado com sucesso.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Erro", $"Não foi possível restaurar: {ex.Message}", "OK");
        }
    }
}
