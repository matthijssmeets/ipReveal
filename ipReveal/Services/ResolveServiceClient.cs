using ip_a.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ip_a.Services;

public class ResolveServiceClient(HttpClient httpClient)
{
    public async Task<IpModel> GetAsync()
    {
        var response = await httpClient.GetFromJsonAsync("json", IpModelContext.Default.IpModel);
        return response ?? throw new Exception("We’re having trouble connecting to the server.");
    }
}