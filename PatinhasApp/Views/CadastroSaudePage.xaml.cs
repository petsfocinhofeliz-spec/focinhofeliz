using PatinhasApp.ViewModels;

namespace PatinhasApp.Views;

public partial class CadastroSaudePage : ContentPage
{
    public CadastroSaudePage(CadastroSaudeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
