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

    // Evita recarregar (e sobrescrever o que a pessoa já digitou) se a tela reaparecer.
    private bool _carregado;

    // Cão da adoção antes da edição (para liberar o cão antigo se for trocado).
    private int _caoIdOriginal;

    [RelayCommand]
    public async Task CarregarCaesAsync()
    {
        if (_carregado) return;
        _carregado = true;

        var lista = await _database.ObterCaesAsync();

        Caes.Clear();
        foreach (var c in lista)
            Caes.Add(c);

        // Edição de uma adoção existente (o id da adoção, não do cão).
        if (AdocaoId > 0)
        {
            var adocao = await _database.ObterAdocaoAsync(AdocaoId);
            if (adocao == null) return;

            Titulo = "Editar adoção";
            _caoIdOriginal = adocao.CaoId;
            CaoSelecionado = Caes.FirstOrDefault(c => c.Id == adocao.CaoId);
            DataAdocao = adocao.DataAdocao;
            NomeAdotante = adocao.NomeAdotante;
            Contato = adocao.Contato;
            Observacoes = adocao.Observacoes;
            return;
        }

        // Novo cadastro vindo da tela de detalhe do cão.
        if (CaoId > 0)
            CaoSelecionado = Caes.FirstOrDefault(c => c.Id == CaoId);
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
            Contato = Contato?.Trim() ?? string.Empty,
            Observacoes = Observacoes
        };

        await _database.SalvarAdocaoAsync(adocao);

        // Se a adoção foi movida para outro cão, o cão antigo volta para "No projeto".
        if (_caoIdOriginal > 0 && _caoIdOriginal != CaoSelecionado.Id)
        {
            var anterior = await _database.ObterCaoAsync(_caoIdOriginal);
            if (anterior != null)
            {
                anterior.Situacao = SituacaoCao.NoProjeto;
                await _database.SalvarCaoAsync(anterior);
            }
        }

        // O cão fica como adotado.
        CaoSelecionado.Situacao = SituacaoCao.Adotado;

        await _database.SalvarCaoAsync(CaoSelecionado);

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelarAsync()
        => await Shell.Current.GoToAsync("..");
}
