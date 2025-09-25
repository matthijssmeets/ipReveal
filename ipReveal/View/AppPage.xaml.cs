using CommunityToolkit.Mvvm.DependencyInjection;
using ip_a.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ip_a.View;

public sealed partial class AppPage : Page
{
    public AppPageViewModel ViewModel
    {
        get;
    } = Ioc.Default.GetRequiredService<AppPageViewModel>();

    public AppPage()
    {
        InitializeComponent();
        Loaded += AppPage_Loaded;
    }

    private async void AppPage_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.GetCollection();
        await ViewModel.ResolvePublicIpAsync();
    }

    private async void OnRowMenuItemClicked(object sender, RoutedEventArgs e)
    {
        var menuItem = sender as MenuFlyoutItem;
        if (menuItem == deleteRow)
        {
            tableView.CollectionView.Remove(deleteRow.DataContext);
        }
        await ViewModel.SaveCollectionAsync();
    }
}