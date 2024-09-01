using TelegramDataStorage.Interfaces;

namespace TelegramDataStorage.IntegrationTests.Models;

public class BytesData
    : IStoredData
{
    public static string Key => nameof(BytesData);

    public byte[]? NullableData { get; init; }

    public byte[] Data { get; init; } = [ ];
}