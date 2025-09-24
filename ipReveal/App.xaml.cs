using CommunityToolkit.Mvvm.DependencyInjection;
using ip_a.Services;
using ip_a.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace ip_a;

public partial class App : Application
{
    private Window? window;

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

        services.AddTransient<AppPageViewModel>();
        services.AddHttpClient<ResolveServiceClient>(client =>
        {
            client.BaseAddress = new Uri("http://ip-api.com");
        });

        Ioc.Default.ConfigureServices(services.BuildServiceProvider());

        window = new AppWindow
        {
            ExtendsContentIntoTitleBar = true
        };
        window.Activate();
        MainRoot = window.Content as FrameworkElement
            ?? throw new InvalidOperationException("Window.Content is not a FrameworkElement.");
    }
}