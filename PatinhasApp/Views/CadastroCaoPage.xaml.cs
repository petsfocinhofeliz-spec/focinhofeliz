using PatinhasApp.ViewModels;

namespace PatinhasApp.Views;

public partial class CadastroCaoPage : ContentPage
{
    // A edição carrega os dados sozinha quando o "id" chega pela rota
    // (ver OnCaoIdChanged no ViewModel), então aqui não precisa OnAppearing.
    public CadastroCaoPage(CadastroCaoViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
