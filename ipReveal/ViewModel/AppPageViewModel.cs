using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ip_a.Models;
using ip_a.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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

    private readonly ResolveServiceClient _resolveServiceClient;

    public AppPageViewModel(ResolveServiceClient resolveServiceClient)
    {
        _resolveServiceClient = resolveServiceClient;
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

    private IpModel ResolvedIpAddr
    {
        get; set;
    }

    public async Task GetCollection()
    {
        var list = await PersistenceService.GetCollectionAsync();
        IpCollection = new ObservableCollection<IpModel>(list);
    }

    [RelayCommand]
    public async Task ResolvePublicIpAsync()
    {
        await ExecuteWithHandlingAsync<Task>(async () =>
        {
            Headline = IsResolvingHeadlineText;
            Subheadline = IsResolvingSubheadlineText;
            ErrorEnabled = false;
            ProgressBarEnabled = true;
            SecondaryBtnEnabled = false;
            PrimaryBtnEnabled = false;

            // Resolve Public IpAddress
            var response = await _resolveServiceClient.GetAsync();

            // Save response
            ResolvedIpAddr = response;

            Headline = ResolvedIpAddr.query;
            Subheadline = response.Isp;
        });
    }

    [RelayCommand]
    public async Task SaveToCollectionAsync()
    {
        await ExecuteWithHandlingAsync<Task>(async () =>
        {
            ProgressBarEnabled = true;
            SecondaryBtnEnabled = false;
            PrimaryBtnEnabled = false;

            // Save to recent activity
            IpCollection.Add(ResolvedIpAddr);
            await PersistenceService.SetCollectionAsync([.. IpCollection]);
        });
    }

    [RelayCommand]
    public async Task CopyValueAsync()
    {
        await ExecuteWithHandlingAsync<Task>(async () =>
        {
            ProgressBarEnabled = true;
            SecondaryBtnEnabled = false;
            PrimaryBtnEnabled = false;

            // Copy to clipboard
            DataPackage dataPackage = new()
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetText(ResolvedIpAddr.query);
            Clipboard.SetContent(dataPackage);
        });
    }

    [RelayCommand]
    public async Task DeleteRecentActivity()
    {
        await ExecuteWithHandlingAsync<Task>(async () =>
        {
            // Show confirmation dialog
            var dialog = new ContentDialog
            {
                XamlRoot = App.MainRoot!.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Delete Recent Activity?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };

            // Show dialog and check if user confirmed
            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            // Clear recent activity
            IpCollection.Clear();
            await PersistenceService.SetCollectionAsync([.. IpCollection]);
        });
    }

    public async Task SaveCollectionAsync()
    {
        await ExecuteWithHandlingAsync<Task>(async () =>
        {
            await PersistenceService.SetCollectionAsync([.. IpCollection]);
        });
    }

    public async Task ExecuteWithHandlingAsync<T>(Func<Task> func)
    {
        try
        {
            ProgressBarEnabled = true;
            SecondaryBtnEnabled = false;
            PrimaryBtnEnabled = false;

            // Add some delay to see the progress bar
            await Task.Delay(500);

            // Execute the function
            await func();
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
            ProgressBarEnabled = false;
            SecondaryBtnEnabled = true;
            PrimaryBtnEnabled = true;
        }

        ProgressBarEnabled = false;
        SecondaryBtnEnabled = true;
        PrimaryBtnEnabled = true;
    }
}