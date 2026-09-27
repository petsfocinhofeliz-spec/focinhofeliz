using PatinhasApp.ViewModels;

namespace PatinhasApp.Views;

public partial class DetalheCaoPage : ContentPage
{
    private readonly DetalheCaoViewModel _vm;

    public DetalheCaoPage(DetalheCaoViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    // Recarrega ao aparecer para refletir mudanças feitas nas telas
    // de edição, novo registro de saúde ou registro de adoção.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.CarregarAsync();
    }
}
