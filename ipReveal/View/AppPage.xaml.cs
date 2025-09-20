using ip_a.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ip_a.View;

public sealed partial class AppPage : Page
{
    public AppPageViewModel ViewModel
    {
        get;
    } = new();

    public AppPage()
    {
        InitializeComponent();
        Loaded += AppPage_Loaded;
    }

    private async void AppPage_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.ResolvePublicIpAsync();
    }
}