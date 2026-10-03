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

    [RelayCommand]
    private async Task EditarAsync(Adocao adocao)
    {
        if (adocao == null)
            return;

        await Shell.Current.GoToAsync(
            $"cadastroAdocao?adocaoId={adocao.Id}");
    }

    [RelayCommand]
    private async Task ExcluirAsync(Adocao adocao)
    {
        if (adocao == null)
            return;

        bool confirmar = await Application.Current!.MainPage!.DisplayAlert(
            "Excluir adoção",
            $"Deseja realmente excluir a adoção de {adocao.NomeAnimal}?\n\n" +
            "O cão voltará para a situação \"No projeto\".",
            "Excluir",
            "Cancelar");

        if (!confirmar)
            return;

        var cao = await _database.ObterCaoAsync(adocao.CaoId);

        await _database.ExcluirAdocaoAsync(adocao);

        if (cao != null)
        {
            cao.Situacao = SituacaoCao.NoProjeto;
            await _database.SalvarCaoAsync(cao);
        }

        Adocoes.Remove(adocao);
    }
}
