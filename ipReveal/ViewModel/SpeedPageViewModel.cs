using CommunityToolkit.Mvvm.ComponentModel;
using SpeedTestSharp.Client;
using SpeedTestSharp.Enums;
using System;
using System.Threading.Tasks;

namespace ip_a.ViewModel;

public partial class SpeedPageViewModel : ObservableObject
{
    private readonly SpeedTestClient _speedTestClient;

    public SpeedPageViewModel(SpeedTestClient speedTestClient)
    {
        _speedTestClient = speedTestClient;
    }

    [ObservableProperty]
    public partial string Headline
    {
        get; set;
    } = string.Empty;

    [ObservableProperty]
    public partial bool ProgressBarEnabled
    {
        get; set;
    }

    public async Task TestSpeedAsync()
    {
        ProgressBarEnabled = true;
        _speedTestClient.StageChanged += (sender, stage) =>
        {
            switch (stage)
            {
                case TestStage.Prepare:
                case TestStage.Latency:
                    Headline = "Selecting Server...";
                    break;

                case TestStage.Download:
                    Headline = $"Testing {stage} Speed...";
                    break;

                case TestStage.Upload:
                    Headline = $"Testing {stage} Speed...";
                    break;

                case TestStage.Stopped:
                default:
                    break;
            }
        };

        var result = await _speedTestClient.TestSpeedAsync(SpeedUnit.Mbps);
        Headline = $"Latency: {result.Latency} ms{Environment.NewLine}" +
            $"Download: {result.DownloadSpeed:0.#} Mbps{Environment.NewLine}" +
            $"Upload: {result.UploadSpeed:0.#} Mbps";
        ProgressBarEnabled = false;
    }
}