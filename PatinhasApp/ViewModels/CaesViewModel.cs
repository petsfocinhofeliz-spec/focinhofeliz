using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PatinhasApp.Data;
using PatinhasApp.Models;

namespace PatinhasApp.ViewModels;

// Tela "Cães": lista com filtro por situação.
public partial class CaesViewModel : BaseViewModel
{
    private readonly DatabaseService _database;

    // Lista que a tela mostra (atualiza a tela sozinha quando muda).
    public ObservableCollection<Cao> Caes { get; } = new();

    public List<string> OpcoesFiltro { get; } =
        new() { "Todos", "No projeto", "Apoiados", "Adotados" };

    [ObservableProperty]
    private string filtroSituacao = "Todos";

    private List<Cao> _todos = new();

    public CaesViewModel(DatabaseService database)
    {
        _database = database;
        Titulo = "Cães";
    }

    [RelayCommand]
    public async Task CarregarAsync()
    {
        if (Ocupado) return;
        try
        {
            Ocupado = true;
            _todos = await _database.ObterCaesAsync();
            AplicarFiltro();
        }
        finally
        {
            Ocupado = false;
        }
    }

    // Chamado automaticamente sempre que FiltroSituacao muda no Picker.
    partial void OnFiltroSituacaoChanged(string value) => AplicarFiltro();

    private void AplicarFiltro()
    {
        var lista = FiltroSituacao switch
        {
            "No projeto" => _todos.Where(c => c.Situacao == SituacaoCao.NoProjeto),
            "Apoiados" => _todos.Where(c => c.Situacao == SituacaoCao.Apoiado),
            "Adotados" => _todos.Where(c => c.Situacao == SituacaoCao.Adotado),
            _ => _todos
        };

        Caes.Clear();
        foreach (var c in lista)
            Caes.Add(c);
    }

    [RelayCommand]
    private async Task NovoCaoAsync()
        => await Shell.Current.GoToAsync("cadastroCao");

    [RelayCommand]
    private async Task AbrirCaoAsync(Cao cao)
    {
        if (cao == null) return;
        await Shell.Current.GoToAsync($"detalheCao?id={cao.Id}");
    }
}
