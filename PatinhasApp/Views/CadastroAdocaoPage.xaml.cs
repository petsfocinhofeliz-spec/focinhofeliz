using PatinhasApp.ViewModels;

namespace PatinhasApp.Views;

public partial class CadastroAdocaoPage : ContentPage
{
    private readonly CadastroAdocaoViewModel _vm;

    public CadastroAdocaoPage(CadastroAdocaoViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    // Carrega a lista de cães para o seletor (e pré-seleciona se veio do detalhe).
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.CarregarCaesAsync();
    }
}
