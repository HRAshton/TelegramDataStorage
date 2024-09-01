using Newtonsoft.Json;
using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.IntegrationTests.Models;

#pragma warning disable CS8618
#pragma warning disable SA1600

public record NestedData : IStoredData
{
    public static string Key => nameof(NestedData);

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public Address Address { get; set; } = new();

    public List<Role> Roles { get; set; } = [ ];

    public Dictionary<string, string> Preferences { get; set; } = new();

    public Status Status { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}