using System.Text.Json.Serialization;

namespace ip_a.Models;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Default,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(IpModel))]
public partial class IpModelContext : JsonSerializerContext
{ }