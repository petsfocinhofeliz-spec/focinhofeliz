namespace PatinhasApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // A primeira tela é o "Shell", que organiza as abas do app.
        MainPage = new AppShell();
    }
}
