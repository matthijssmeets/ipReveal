namespace ip_a.Models;

public class IpModel
{
    //public string Status { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    //public string Region { get; set; } = string.Empty;
    public string RegionName { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public float Lat { get; set; }
    public float Lon { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public string Isp { get; set; } = string.Empty;
    public string Org { get; set; } = string.Empty;
    public string query { get; set; } = string.Empty;
}