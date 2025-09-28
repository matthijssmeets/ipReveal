using CommunityToolkit.Mvvm.DependencyInjection;
using ip_a.Services;
using ip_a.View;
using ip_a.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace ip_a;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    public static FrameworkElement? MainRoot
    {
        get; private set;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var services = new ServiceCollection();

        services.AddTransient<AppWindow>();
        services.AddTransient<AppWindowViewModel>();
        services.AddTransient<AppPage>();
        services.AddTransient<AppPageViewModel>();
        services.AddTransient<SpeedPage>();
        services.AddTransient<SpeedPageViewModel>();
        services.AddHttpClient<ResolveServiceClient>(client =>
        {
            client.BaseAddress = new Uri("http://ip-api.com");
        });
        Ioc.Default.ConfigureServices(services.BuildServiceProvider());

        var window = Ioc.Default.GetRequiredService<AppWindow>();
        window.Activate();
        MainRoot = window.Content as FrameworkElement;
    }
}