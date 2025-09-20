namespace ip_a.Models;

public class IpModel
{
    public string City { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Country_code { get; set; } = string.Empty;
    public string Encoding { get; set; } = string.Empty;
    public string Forwarded { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Ifconfig_cmd_hostname { get; set; } = string.Empty;
    public int Ifconfig_copyrightYear { get; set; }
    public string Ifconfig_hostname { get; set; } = string.Empty;
    public string Ifconfig_icon_html { get; set; } = string.Empty;
    public string Ifconfig_plausible { get; set; } = string.Empty;
    public string Ifconfig_self_hosted_plausible { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public string Isp { get; set; } = string.Empty;
    public string Isp_organization { get; set; } = string.Empty;
    public string Lang { get; set; } = string.Empty;
    public bool MaxMindShow { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Mime { get; set; } = string.Empty;
    public int Port { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public string Referer { get; set; } = string.Empty;
    public string Ua { get; set; } = string.Empty;
}