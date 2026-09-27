using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using PatinhasApp.Data;
using PatinhasApp.Models;

namespace PatinhasApp.ViewModels;

// Tela "Adoções": lista todas as adoções registradas.
public partial class AdocoesViewModel : BaseViewModel
{
    private readonly DatabaseService _database;

    public ObservableCollection<Adocao> Adocoes { get; } = new();

    public AdocoesViewModel(DatabaseService database)
    {
        _database = database;
        Titulo = "Adoções";
    }

    [RelayCommand]
    public async Task CarregarAsync()
    {
        if (Ocupado) return;
        try
        {
            Ocupado = true;
            var lista = await _database.ObterAdocoesAsync();
            Adocoes.Clear();
            foreach (var a in lista)
                Adocoes.Add(a);
        }
        finally
        {
            Ocupado = false;
        }
    }

    [RelayCommand]
    private async Task NovaAdocaoAsync()
        => await Shell.Current.GoToAsync("cadastroAdocao");
}
