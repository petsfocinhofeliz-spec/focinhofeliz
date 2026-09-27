using PatinhasApp.ViewModels;

namespace PatinhasApp.Views;

public partial class AlertasPage : ContentPage
{
    private readonly AlertasViewModel _vm;

    public AlertasPage(AlertasViewModel vm)
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
