namespace ip_a;

public sealed partial class AppWindow : WinUIEx.WindowEx
{
    public AppWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
    }
}