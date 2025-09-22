using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ip_a.Services;
using Microsoft.UI.Xaml;
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

    public AppPageViewModel(RevealServiceClient revealServiceClient)
    {
        revealClient = revealServiceClient;
    }

    [ObservableProperty]
    public partial string TextValue
    {
        get; set;
    } = IsResolvingText;

    [ObservableProperty]
    public partial bool ErrorEnabled
    {
        get; set;
    }

    [ObservableProperty]
    public partial string ErrorMessage
    {
        get; set;
    } = string.Empty;

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

    private string IpAddress
    {
        get; set;
    } = string.Empty;

    [RelayCommand]
    public async Task ResolvePublicIpAsync()
    {
        try
        {
            TextValue = IsResolvingText;
            ErrorEnabled = false;
            ProgressBarEnabled = true;
            CopyBtnEnabled = false;
            RefreshBtnEnabled = false;

            // Add some delay to see the progress bar
            await Task.Delay(1000);

            // Resolve Public IpAddress
            var response = await revealClient.GetAsync();
            IpAddress = response?.Ip ?? "Error";

            var builder = new StringBuilder();
            builder.AppendLine($"Your IP is {IpAddress} [{response?.City} @ {response?.Country}]");
            builder.AppendLine($"Internet service provider: {response?.Isp_organization}");

            TextValue = builder.ToString();
            CopyBtnEnabled = true;
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Please check your connection...";
            ErrorEnabled = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"{ex.Message}";
            ErrorEnabled = true;
        }

        ProgressBarEnabled = false;
        RefreshBtnEnabled = true;
    }

    [RelayCommand]
    public async Task CopyValueAsync()
    {
        try
        {
            ProgressBarEnabled = true;
            CopyBtnEnabled = false;
            RefreshBtnEnabled = false;

            // Add some delay to see the progress bar
            await Task.Delay(500);

            // Copy to clipboard
            DataPackage dataPackage = new()
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetText(IpAddress);
            Clipboard.SetContent(dataPackage);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"{ex.Message}";
            ErrorEnabled = true;
            ProgressBarEnabled = false;
            CopyBtnEnabled = true;
            RefreshBtnEnabled = true;
        }

        ProgressBarEnabled = false;
        CopyBtnEnabled = true;
        RefreshBtnEnabled = true;
    }

    [RelayCommand]
    public static void AppExit()
    {
        Application.Current.Exit();
    }
}