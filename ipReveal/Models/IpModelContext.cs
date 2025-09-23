using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ip_a.Models;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Default,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(IpModel))]
[JsonSerializable(typeof(List<IpModel>))]
public partial class IpModelContext : JsonSerializerContext
{ }