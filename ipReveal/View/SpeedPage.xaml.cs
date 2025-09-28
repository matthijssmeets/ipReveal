using CommunityToolkit.Mvvm.DependencyInjection;
using ip_a.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace ip_a.View;

public sealed partial class SpeedPage : Page
{
    public SpeedPageViewModel ViewModel
    {
        get;
    } = Ioc.Default.GetRequiredService<SpeedPageViewModel>();

    public SpeedPage()
    {
        InitializeComponent();
    }
}