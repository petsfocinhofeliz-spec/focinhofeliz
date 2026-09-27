using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PatinhasApp.Data;
using PatinhasApp.Models;

namespace PatinhasApp.ViewModels;

// Tela de adicionar um registro de saúde a um cão.
[QueryProperty(nameof(CaoId), "caoId")]
public partial class CadastroSaudeViewModel : BaseViewModel
{
    private readonly DatabaseService _database;

    public CadastroSaudeViewModel(DatabaseService database)
    {
        _database = database;
        Titulo = "Novo registro de saúde";
    }

    [ObservableProperty] private int caoId;
    [ObservableProperty] private int tipoSelecionado;      // 0 vacina, 1 vermífugo, 2 problema
    [ObservableProperty] private string descricao = string.Empty;
    [ObservableProperty] private DateTime data = DateTime.Today;
    [ObservableProperty] private bool temProximaData = true;
    [ObservableProperty] private DateTime proximaData = DateTime.Today.AddMonths(12);
    [ObservableProperty] private string? observacoes;

    public List<string> OpcoesTipo { get; } =
        new() { "Vacina", "Vermífugo", "Problema de saúde" };

    [RelayCommand]
    private async Task SalvarAsync()
    {
        if (string.IsNullOrWhiteSpace(Descricao))
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Atenção", "Informe a descrição (ex.: V10, Cinomose).", "OK");
            return;
        }

        var registro = new RegistroSaude
        {
            CaoId = CaoId,
            Tipo = (TipoRegistroSaude)TipoSelecionado,
            Descricao = Descricao.Trim(),
            Data = Data,
            ProximaData = TemProximaData ? ProximaData : (DateTime?)null,
            Observacoes = Observacoes
        };

        await _database.SalvarRegistroSaudeAsync(registro);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelarAsync()
        => await Shell.Current.GoToAsync("..");
}
