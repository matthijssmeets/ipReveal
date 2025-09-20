using ip_a.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ip_a.Services;

public class RevealServiceClient(HttpClient httpClient)
{
    public async Task<IpModel?> GetAsync()
    {
        return await httpClient.GetFromJsonAsync<IpModel>("all.json");
    }
}