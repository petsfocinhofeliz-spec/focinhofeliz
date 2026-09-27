using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PatinhasApp.Data;
using PatinhasApp.Models;

namespace PatinhasApp.ViewModels;

// Tela de registrar uma adoção. Pode receber "caoId" já preenchido
// (quando aberta pela tela de detalhe do cão) ou deixar escolher o cão.
[QueryProperty(nameof(CaoId), "caoId")]
public partial class CadastroAdocaoViewModel : BaseViewModel
{
    private readonly DatabaseService _database;

    public CadastroAdocaoViewModel(DatabaseService database)
    {
        _database = database;
        Titulo = "Registrar adoção";
    }

    [ObservableProperty] private int caoId;
    [ObservableProperty] private Cao? caoSelecionado;
    [ObservableProperty] private DateTime dataAdocao = DateTime.Today;
    [ObservableProperty] private string nomeAdotante = string.Empty;
    [ObservableProperty] private string contato = string.Empty;
    [ObservableProperty] private string? observacoes;

    public ObservableCollection<Cao> Caes { get; } = new();

    [RelayCommand]
    public async Task CarregarCaesAsync()
    {
        var lista = await _database.ObterCaesAsync();
        Caes.Clear();
        foreach (var c in lista)
            Caes.Add(c);

        // Se veio da tela de detalhe, já deixa o cão selecionado.
        if (CaoId > 0)
            CaoSelecionado = Caes.FirstOrDefault(c => c.Id == CaoId);
    }

    [RelayCommand]
    private async Task SalvarAsync()
    {
        if (CaoSelecionado == null)
        {
            await Application.Current!.MainPage!.DisplayAlert("Atenção", "Escolha o cão adotado.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(NomeAdotante))
        {
            await Application.Current!.MainPage!.DisplayAlert("Atenção", "Informe o nome do adotante.", "OK");
            return;
        }

        var adocao = new Adocao
        {
            CaoId = CaoSelecionado.Id,
            NomeAnimal = CaoSelecionado.Nome,
            DataAdocao = DataAdocao,
            NomeAdotante = NomeAdotante.Trim(),
            Contato = Contato.Trim(),
            Observacoes = Observacoes
        };

        await _database.SalvarAdocaoAsync(adocao);

        // Ao adotar, o cão passa automaticamente para a situação "Adotado".
        CaoSelecionado.Situacao = SituacaoCao.Adotado;
        await _database.SalvarCaoAsync(CaoSelecionado);

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelarAsync()
        => await Shell.Current.GoToAsync("..");
}
