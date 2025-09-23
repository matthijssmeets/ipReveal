using ip_a.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace ip_a.Services;

public class PersistenceService
{
    public static readonly string Path = System.IO.Path.Combine(
        ApplicationData.Current.LocalFolder.Path,
        "ip_data.json"
    );

    public static async Task<List<IpModel>> GetCollectionAsync()
    {
        if (!File.Exists(Path))
        {
            await SetCollection([]);
        }
        using FileStream openStream = File.OpenRead(Path);
        var list = await JsonSerializer.DeserializeAsync(openStream, IpModelContext.Default.ListIpModel);
        return list ?? [];
    }

    public static async Task SetCollectionAsync(List<IpModel> items)
    {
        await SetCollection(items);
    }

    private static async Task SetCollection(List<IpModel> items)
    {
        await using FileStream createStream = File.Create(Path);
        await JsonSerializer.SerializeAsync(createStream, items, IpModelContext.Default.ListIpModel);
    }
}