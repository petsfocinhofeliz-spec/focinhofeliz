using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PatinhasApp.Data;
using PatinhasApp.Models;
using System.Runtime.ConstrainedExecution;

namespace PatinhasApp.ViewModels;

// Tela de cadastrar/editar um cão.
// [QueryProperty] recebe o "id" que vem pela rota (só existe ao editar).
[QueryProperty(nameof(CaoId), "id")]
public partial class CadastroCaoViewModel : BaseViewModel
{
    private readonly DatabaseService _database;
    private DateTime _dataCadastro = DateTime.Now;

    public CadastroCaoViewModel(DatabaseService database)
    {
        _database = database;
        Titulo = "Novo cão";
    }

    [ObservableProperty] private int caoId;

    [ObservableProperty] private string nome = string.Empty;
    [ObservableProperty] private string? caminhoFoto;
    [ObservableProperty] private bool temFoto;
    [ObservableProperty] private int sexoSelecionado;      // 0 = Macho, 1 = Fêmea
    [ObservableProperty] private int porteSelecionado;     // 0,1,2
    [ObservableProperty] private string? raca;
    [ObservableProperty] private string? cor;
    [ObservableProperty] private int castradoSelecionado;
    [ObservableProperty] private DateTime dataResgate = DateTime.Today;
    [ObservableProperty] private int situacaoSelecionada;  // 0,1,2
    [ObservableProperty] private string? observacoes;

    public List<string> OpcoesCastrado { get; } = new() { "Não", "Sim" };
    
    public List<string> OpcoesSexo { get; } = new() { "Macho", "Fêmea" };
    public List<string> OpcoesPorte { get; } = new() { "Pequeno", "Médio", "Grande" };
    public List<string> OpcoesSituacao { get; } = new() { "No projeto", "Apoiado", "Adotado" };

    // Quando o id chega pela rota (edição), carrega os dados do cão.
    partial void OnCaoIdChanged(int value)
    {
        if (value > 0)
            _ = CarregarAsync(value);
    }

    partial void OnCaminhoFotoChanged(string? value)
        => TemFoto = !string.IsNullOrWhiteSpace(value);

    private async Task CarregarAsync(int id)
    {
        var cao = await _database.ObterCaoAsync(id);
        if (cao == null) return;

        Titulo = "Editar cão";
        Nome = cao.Nome;
        CaminhoFoto = cao.CaminhoFoto;
        SexoSelecionado = (int)cao.Sexo;
        PorteSelecionado = (int)cao.Porte;     
        CastradoSelecionado = cao.Castrado ? 1 : 0;
        Raca = cao.Raca;
        Cor = cao.Cor;        
        DataResgate = cao.DataResgate;
        SituacaoSelecionada = (int)cao.Situacao;
        Observacoes = cao.Observacoes;
        _dataCadastro = cao.DataCadastro;
    }

    [RelayCommand]
    private async Task EscolherFotoAsync()
    {
        try
        {
            var foto = await MediaPicker.Default.PickPhotoAsync();
            if (foto != null)
                CaminhoFoto = await CopiarFotoAsync(foto);
        }
        catch (FeatureNotSupportedException)
        {
            await MostrarAsync("A galeria não é suportada neste aparelho.");
        }
    }

    [RelayCommand]
    private async Task TirarFotoAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await MostrarAsync("A câmera não está disponível neste aparelho.");
                return;
            }

            var foto = await MediaPicker.Default.CapturePhotoAsync();
            if (foto != null)
                CaminhoFoto = await CopiarFotoAsync(foto);
        }
        catch (Exception ex)
        {
            await MostrarAsync($"Não foi possível usar a câmera: {ex.Message}");
        }
    }

    // Copia a foto escolhida para a pasta do app, para não depender da galeria.
    private static async Task<string> CopiarFotoAsync(FileResult foto)
    {
        var nome = $"cao_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(foto.FileName)}";
        var destino = Path.Combine(FileSystem.AppDataDirectory, nome);

        using var origem = await foto.OpenReadAsync();
        using var saida = File.Create(destino);
        await origem.CopyToAsync(saida);

        return destino;
    }

    [RelayCommand]
    private async Task SalvarAsync()
    {
        if (string.IsNullOrWhiteSpace(Nome))
        {
            await MostrarAsync("Informe o nome do cão.");
            return;
        }

        var cao = new Cao
        {
            Id = CaoId,
            Nome = Nome.Trim(),
            CaminhoFoto = CaminhoFoto,
            Sexo = (Sexo)SexoSelecionado,
            Porte = (Porte)PorteSelecionado,
            Raca = Raca?.Trim(),
            Cor = Cor?.Trim(),
            Castrado = CastradoSelecionado == 1,
            DataResgate = DataResgate,
            Situacao = (SituacaoCao)SituacaoSelecionada,
            Observacoes = Observacoes,
            DataCadastro = _dataCadastro
        };

        await _database.SalvarCaoAsync(cao);
        await Shell.Current.GoToAsync(".."); // volta para a tela anterior
    }

    [RelayCommand]
    private async Task CancelarAsync()
        => await Shell.Current.GoToAsync("..");

    private static Task MostrarAsync(string mensagem)
        => Application.Current!.MainPage!.DisplayAlert("Atenção", mensagem, "OK");
}
