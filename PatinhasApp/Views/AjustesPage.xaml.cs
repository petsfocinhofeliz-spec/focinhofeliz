using PatinhasApp.ViewModels;

namespace PatinhasApp.Views;

public partial class AjustesPage : ContentPage
{
    private readonly AjustesViewModel _vm;

    public AjustesPage(AjustesViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.CarregarAsync();
    }
}
