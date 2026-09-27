using PatinhasApp.ViewModels;

namespace PatinhasApp.Views;

public partial class CaesPage : ContentPage
{
    private readonly CaesViewModel _vm;

    // O ViewModel chega pronto por injeção de dependência (configurado no MauiProgram).
    public CaesPage(CaesViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    // Sempre que a tela aparece (inclusive ao voltar de outra), recarrega a lista.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.CarregarAsync();
    }
}
