namespace Cits.Core.Configuration;

public class OpenTelemetryOptions
{
    public const string ConfigurationSection = "OpenTelemetry";

    public bool IsEnable { get; set; }= false;
    public string? OtlpExportEndpoint { get; set; }
}