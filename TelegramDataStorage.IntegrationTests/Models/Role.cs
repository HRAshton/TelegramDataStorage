namespace TelegramDataStorage.IntegrationTests.Models;

#pragma warning disable CS8618
#pragma warning disable SA1600

public class Role
{
    public string RoleName { get; set; }

    public List<string> Permissions { get; set; }
}