using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ip_a.Models;
using ip_a.Services;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace ip_a.ViewModel;

public partial class AppPageViewModel : ObservableObject
{
    private const string IsResolvingHeadlineText = "Resolving Public IP";
    private const string IsResolvingSubheadlineText = "• • • • •";

    private readonly RevealServiceClient revealClient;

    public AppPageViewModel(RevealServiceClient revealServiceClient)
    {
        revealClient = revealServiceClient;
    }

    [ObservableProperty]
    public partial string Headline
    {
        get; set;
    } = IsResolvingHeadlineText;

    [ObservableProperty]
    public partial string Subheadline
    {
        get; set;
    } = string.Empty;

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
    public partial bool SecondaryBtnEnabled
    {
        get; set;
    }

    [ObservableProperty]
    public partial bool PrimaryBtnEnabled
    {
        get; set;
    }

    [ObservableProperty]
    public partial ObservableCollection<IpModel> IpCollection
    {
        get; set;
    } = [];

    private string IpAddress
    {
        get; set;
    } = string.Empty;

    public async Task GetCollection()
    {
        var list = await PersistenceService.GetCollectionAsync();
        IpCollection = new ObservableCollection<IpModel>(list);
    }

    [RelayCommand]
    public async Task ResolvePublicIpAsync()
    {
        try
        {
            Headline = IsResolvingHeadlineText;
            Subheadline = IsResolvingSubheadlineText;
            ErrorEnabled = false;
            ProgressBarEnabled = true;
            SecondaryBtnEnabled = false;
            PrimaryBtnEnabled = false;

            // Add some delay to see the progress bar
            await Task.Delay(1000);

            // Resolve Public IpAddress
            var response = await revealClient.GetAsync();

            // Update IpAddress for clipboard copy
            IpAddress = response.Ip;

            // Save to recent activity
            IpCollection.Add(response);
            await PersistenceService.SetCollectionAsync([.. IpCollection]);

            Headline = IpAddress;
            Subheadline = response.InternetServiceProvider;
            SecondaryBtnEnabled = true;
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
        PrimaryBtnEnabled = true;
    }

    [RelayCommand]
    public async Task CopyValueAsync()
    {
        try
        {
            ProgressBarEnabled = true;
            SecondaryBtnEnabled = false;
            PrimaryBtnEnabled = false;

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
            SecondaryBtnEnabled = true;
            PrimaryBtnEnabled = true;
        }

        ProgressBarEnabled = false;
        SecondaryBtnEnabled = true;
        PrimaryBtnEnabled = true;
    }

    [RelayCommand]
    public async Task DeleteRecentActivity()
    {
        try
        {
            // Add some delay to see the progress bar
            await Task.Delay(500);

            // Clear recent activity
            IpCollection.Clear();
            await PersistenceService.SetCollectionAsync([.. IpCollection]);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"{ex.Message}";
            ErrorEnabled = true;
        }
    }
}