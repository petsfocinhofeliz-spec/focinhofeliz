using Android.App;
using Android.Runtime;

namespace PatinhasApp;

// Classe de aplicação do Android. Conecta o Android ao nosso MauiProgram.
[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
