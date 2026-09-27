using CommunityToolkit.Mvvm.ComponentModel;

namespace PatinhasApp.ViewModels;

// Base comum a todas as telas. "partial" + [ObservableProperty] fazem o
// CommunityToolkit gerar automaticamente as propriedades que a tela observa.
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool ocupado;   // true enquanto carrega dados (mostra "girando")

    [ObservableProperty]
    private string titulo = string.Empty;
}
