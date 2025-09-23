using System.Text.Json.Serialization;

namespace ip_a.Models;

public class IpModel
{
    public string Ip { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;

    [JsonPropertyName("isp_organization")]
    public string InternetServiceProvider { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;
}