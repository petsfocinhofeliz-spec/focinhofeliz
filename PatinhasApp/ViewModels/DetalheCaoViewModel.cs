using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PatinhasApp.Data;
using PatinhasApp.Models;

namespace PatinhasApp.ViewModels;

// Tela de detalhe de um cão: dados, histórico de saúde e adoção.
[QueryProperty(nameof(CaoId), "id")]
public partial class DetalheCaoViewModel : BaseViewModel
{
    private readonly DatabaseService _database;

    public DetalheCaoViewModel(DatabaseService database)
    {
        _database = database;
    }

    [ObservableProperty] private int caoId;
    [ObservableProperty] private Cao? cao;
    [ObservableProperty] private Adocao? adocao;
    [ObservableProperty] private bool possuiAdocao;

    public ObservableCollection<RegistroSaude> Registros { get; } = new();

    [RelayCommand]
    public async Task CarregarAsync()
    {
        if (CaoId <= 0) return;

        Cao = await _database.ObterCaoAsync(CaoId);
        if (Cao != null) Titulo = Cao.Nome;

        var registros = await _database.ObterRegistrosSaudeAsync(CaoId);
        Registros.Clear();
        foreach (var r in registros)
            Registros.Add(r);

        Adocao = await _database.ObterAdocaoPorCaoAsync(CaoId);
        PossuiAdocao = Adocao != null;
    }

    [RelayCommand]
    private async Task EditarCaoAsync()
        => await Shell.Current.GoToAsync($"cadastroCao?id={CaoId}");

    [RelayCommand]
    private async Task NovoRegistroSaudeAsync()
        => await Shell.Current.GoToAsync($"cadastroSaude?caoId={CaoId}");

    [RelayCommand]
    private async Task ExcluirRegistroAsync(RegistroSaude registro)
    {
        if (registro == null) return;

        var ok = await Application.Current!.MainPage!.DisplayAlert(
            "Excluir", $"Remover o registro \"{registro.Descricao}\"?", "Sim", "Não");
        if (!ok) return;

        await _database.ExcluirRegistroSaudeAsync(registro);
        Registros.Remove(registro);
    }

    [RelayCommand]
    private async Task RegistrarAdocaoAsync()
        => await Shell.Current.GoToAsync($"cadastroAdocao?caoId={CaoId}");

    [RelayCommand]
    private async Task ExcluirCaoAsync()
    {
        if (Cao == null) return;

        var ok = await Application.Current!.MainPage!.DisplayAlert(
            "Excluir cão",
            $"Excluir \"{Cao.Nome}\"? Isso também remove o histórico de saúde e a adoção ligados a ele.",
            "Sim", "Não");
        if (!ok) return;

        await _database.ExcluirCaoAsync(Cao);
        await Shell.Current.GoToAsync("..");
    }
}
