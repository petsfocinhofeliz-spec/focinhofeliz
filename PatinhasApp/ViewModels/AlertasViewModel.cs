using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PatinhasApp.Data;
using PatinhasApp.Models;

namespace PatinhasApp.ViewModels;

// Item mostrado na tela de alertas (junta o registro com o nome do cão).
public class AlertaSaude
{
    public RegistroSaude Registro { get; set; } = default!;
    public string NomeCao { get; set; } = string.Empty;

    public string Titulo => $"{Registro.TipoTexto}: {Registro.Descricao}";
    public string Detalhe => $"{NomeCao} • {Registro.StatusVencimento} ({Registro.ProximaDataTexto})";
    public bool Vencido => (Registro.DiasParaVencer ?? 1) < 0;
}

// Tela "Alertas": vacinas/vermífugos vencidos ou que vencem nos próximos 30 dias.
public partial class AlertasViewModel : BaseViewModel
{
    private readonly DatabaseService _database;

    public ObservableCollection<AlertaSaude> Alertas { get; } = new();

    [ObservableProperty] private bool semAlertas;

    public AlertasViewModel(DatabaseService database)
    {
        _database = database;
        Titulo = "Alertas";
    }

    [RelayCommand]
    public async Task CarregarAsync()
    {
        if (Ocupado) return;
        try
        {
            Ocupado = true;

            var caes = await _database.ObterCaesAsync();
            var mapaCaes = caes.ToDictionary(c => c.Id, c => c.Nome);

            var registros = await _database.ObterTodosRegistrosSaudeAsync();
            var limite = DateTime.Today.AddDays(30);

            var proximos = registros
                .Where(r => r.ProximaData.HasValue && r.ProximaData.Value.Date <= limite)
                .OrderBy(r => r.ProximaData)
                .Select(r => new AlertaSaude
                {
                    Registro = r,
                    NomeCao = mapaCaes.TryGetValue(r.CaoId, out var nome) ? nome : "(cão removido)"
                })
                .ToList();

            Alertas.Clear();
            foreach (var a in proximos)
                Alertas.Add(a);

            SemAlertas = Alertas.Count == 0;
        }
        finally
        {
            Ocupado = false;
        }
    }

    [RelayCommand]
    private async Task AbrirCaoAsync(AlertaSaude alerta)
    {
        if (alerta?.Registro == null) return;
        await Shell.Current.GoToAsync($"detalheCao?id={alerta.Registro.CaoId}");
    }
}
