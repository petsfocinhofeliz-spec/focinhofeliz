using Microsoft.Extensions.Logging;
using PatinhasApp.Data;
using PatinhasApp.Services;
using PatinhasApp.ViewModels;
using PatinhasApp.Views;

namespace PatinhasApp;

// Ponto de partida do app. Aqui a gente "registra" todas as peças
// (banco, serviços, telas) para o próprio .NET criar e conectar tudo sozinho.
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // ---- Serviços que existem uma vez só no app inteiro (Singleton) ----
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<BackupService>();

        // ---- Telas das abas + seus ViewModels (uma instância só) ----
        builder.Services.AddSingleton<CaesViewModel>();
        builder.Services.AddSingleton<CaesPage>();
        builder.Services.AddSingleton<AdocoesViewModel>();
        builder.Services.AddSingleton<AdocoesPage>();
        builder.Services.AddSingleton<AlertasViewModel>();
        builder.Services.AddSingleton<AlertasPage>();
        builder.Services.AddSingleton<AjustesViewModel>();
        builder.Services.AddSingleton<AjustesPage>();

        // ---- Telas de cadastro/detalhe (instância nova a cada abertura) ----
        builder.Services.AddTransient<CadastroCaoViewModel>();
        builder.Services.AddTransient<CadastroCaoPage>();
        builder.Services.AddTransient<DetalheCaoViewModel>();
        builder.Services.AddTransient<DetalheCaoPage>();
        builder.Services.AddTransient<CadastroSaudeViewModel>();
        builder.Services.AddTransient<CadastroSaudePage>();
        builder.Services.AddTransient<CadastroAdocaoViewModel>();
        builder.Services.AddTransient<CadastroAdocaoPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
