using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ip_a.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace ip_a.ViewModel;

public partial class AppPageViewModel : ObservableObject
{
    private const string IsResolvingText = "Resolving public IPv4";
    private readonly RevealServiceClient revealClient;

    public AppPageViewModel()
    {
        revealClient = Ioc.Default.GetRequiredService<RevealServiceClient>();
    }

    [ObservableProperty]
    public partial string TextValue
    {
        get; set;
    } = IsResolvingText;

    [ObservableProperty]
    public partial bool ProgressBarEnabled
    {
        get; set;
    }

    [ObservableProperty]
    public partial bool CopyBtnEnabled
    {
        get; set;
    }

    [ObservableProperty]
    public partial bool RefreshBtnEnabled
    {
        get; set;
    }

    private string Ipv4
    {
        get; set;
    } = string.Empty;

    [RelayCommand]
    public async Task ResolvePublicIpAsync()
    {
        try
        {
            // Update UI
            TextValue = IsResolvingText;
            ControlsEnabled(false);

            // Add some delay to see the progress bar
            await Task.Delay(1000);

            // Reveal

            var response = await revealClient.GetAsync();
            Ipv4 = response?.Ip ?? "Error";

            // Update UI
            var builder = new StringBuilder();
            builder.AppendLine($"Your IP is {Ipv4} [{response?.City} @ {response?.Country}]");
            builder.AppendLine($"Internet service provider: {response?.Isp_organization}");

            TextValue = builder.ToString();
        }
        catch (HttpRequestException)
        {
            TextValue = $"⁉️ - Please check your connection...";
        }
        catch (Exception ex)
        {
            TextValue = $"⁉️ - {ex.Message}";
        }

        // Update UI
        ControlsEnabled(true);
    }

    [RelayCommand]
    public async Task CopyValueAsync()
    {
        try
        {
            // Update UI
            ControlsEnabled(false);

            // Add some delay to see the progress bar
            await Task.Delay(500);

            // Copy to clipboard
            DataPackage dataPackage = new()
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetText(Ipv4);
            Clipboard.SetContent(dataPackage);
        }
        catch (Exception ex)
        {
            TextValue = $"⁉️ - {ex.Message}";
        }

        // Update UI
        ControlsEnabled(true);
    }

    private void ControlsEnabled(bool enabled)
    {
        ProgressBarEnabled = !enabled;
        CopyBtnEnabled = enabled;
        RefreshBtnEnabled = enabled;
    }
}