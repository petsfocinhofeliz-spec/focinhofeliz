using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PatinhasApp.Data;
using PatinhasApp.Models;

namespace PatinhasApp.ViewModels;

// Tela de registrar/editar uma adoção.
[QueryProperty(nameof(CaoId), "caoId")]
[QueryProperty(nameof(AdocaoId), "adocaoId")]
public partial class CadastroAdocaoViewModel : BaseViewModel
{
    private readonly DatabaseService _database;

    public CadastroAdocaoViewModel(DatabaseService database)
    {
        _database = database;
        Titulo = "Registrar adoção";
    }

    [ObservableProperty] private int caoId;
    [ObservableProperty] private int adocaoId;
    [ObservableProperty] private Cao? caoSelecionado;
    [ObservableProperty] private DateTime dataAdocao = DateTime.Today;
    [ObservableProperty] private string nomeAdotante = string.Empty;
    [ObservableProperty] private string contato = string.Empty;
    [ObservableProperty] private string? observacoes;
    public ObservableCollection<Cao> Caes { get; } = new();
    partial void OnAdocaoIdChanged(int value)
    {
        if (value > 0)
            _ = CarregarAdocaoAsync(value);
    }

    [RelayCommand]
    public async Task CarregarCaesAsync()
    {
        var lista = await _database.ObterCaesAsync();

        Caes.Clear();

        foreach (var c in lista)
            Caes.Add(c);

        // Novo cadastro vindo da tela de detalhe do cão.
        if (AdocaoId == 0 && CaoId > 0)
        {
            CaoSelecionado = Caes.FirstOrDefault(c => c.Id == CaoId);
        }

        // Edição de uma adoção existente.
        if (AdocaoId > 0)
        {
            var adocao = await _database.ObterAdocaoPorCaoAsync(
                AdocaoId);

            if (adocao != null)
                CaoSelecionado = Caes.FirstOrDefault(
                    c => c.Id == adocao.CaoId);
        }
    }

    private async Task CarregarAdocaoAsync(int id)
    {
        var listaCaes = await _database.ObterCaesAsync();

        Caes.Clear();

        foreach (var c in listaCaes)
            Caes.Add(c);

        var adocao = await _database.ObterAdocaoAsync(id);

        if (adocao == null)
            return;

        Titulo = "Editar adoção";

        CaoSelecionado = Caes.FirstOrDefault(
            c => c.Id == adocao.CaoId);

        DataAdocao = adocao.DataAdocao;
        NomeAdotante = adocao.NomeAdotante;
        Contato = adocao.Contato;
        Observacoes = adocao.Observacoes;
    }

    [RelayCommand]
    private async Task SalvarAsync()
    {
        if (CaoSelecionado == null)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Atenção",
                "Escolha o cão adotado.",
                "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(NomeAdotante))
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Atenção",
                "Informe o nome do adotante.",
                "OK");

            return;
        }

        var adocao = new Adocao
        {
            Id = AdocaoId,
            CaoId = CaoSelecionado.Id,
            NomeAnimal = CaoSelecionado.Nome,
            DataAdocao = DataAdocao,
            NomeAdotante = NomeAdotante.Trim(),
            Contato = Contato.Trim(),
            Observacoes = Observacoes
        };

        await _database.SalvarAdocaoAsync(adocao);

        // O cão fica como adotado.
        CaoSelecionado.Situacao = SituacaoCao.Adotado;

        await _database.SalvarCaoAsync(CaoSelecionado);

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelarAsync()
        => await Shell.Current.GoToAsync("..");
}
