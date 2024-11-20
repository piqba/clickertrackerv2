namespace Share.Otel;

public class OtelOptions
{
    public string? OtelEndpointUrl { get; init; } = string.Empty;
    public string? OtelApplicationName { get; init; } = Constants.DefaultOtelApplicationName;
}

