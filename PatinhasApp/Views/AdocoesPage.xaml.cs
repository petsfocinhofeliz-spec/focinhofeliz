using PatinhasApp.ViewModels;

namespace PatinhasApp.Views;

public partial class AdocoesPage : ContentPage
{
    private readonly AdocoesViewModel _vm;

    public AdocoesPage(AdocoesViewModel vm)
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
