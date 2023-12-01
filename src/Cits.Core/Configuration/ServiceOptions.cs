namespace Cits.Core.Configuration;

public class ServiceOptions
{
    public const string ConfigurationSection = "ServiceOptions";

    public string Name { get; set; } = string.Empty;
    
    public ushort WorkerId { get; set; } = 1;
}