using CommunityToolkit.Mvvm.DependencyInjection;
using ip_a.View;
using ip_a.ViewModel;
using Microsoft.UI.Xaml;
using WinUIEx;

namespace ip_a;

public sealed partial class AppWindow : WindowEx
{
    public AppWindowViewModel ViewModel
    {
        get;
    } = Ioc.Default.GetRequiredService<AppWindowViewModel>();

    public AppWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        MainFrame.Navigate(typeof(AppPage));
    }

    private void ToAppPage_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(typeof(AppPage));
    }

    private void ToSpeedPage_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(typeof(SpeedPage));
    }

    private void AppExit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Exit();
    }
}