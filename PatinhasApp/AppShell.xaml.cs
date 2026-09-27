using PatinhasApp.Views;

namespace PatinhasApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Telas que abrem "por cima" das abas (recebem parâmetros pela rota).
        Routing.RegisterRoute("cadastroCao", typeof(CadastroCaoPage));
        Routing.RegisterRoute("detalheCao", typeof(DetalheCaoPage));
        Routing.RegisterRoute("cadastroSaude", typeof(CadastroSaudePage));
        Routing.RegisterRoute("cadastroAdocao", typeof(CadastroAdocaoPage));
    }
}
