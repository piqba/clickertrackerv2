using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClickerR2d2.dto;

// TODO: pass to share project
public record WebPageEventDto(
    [property: JsonPropertyName("page_url")]
    string PageUrl,
    [property: JsonPropertyName("page_title")]
    string PageTitle,
    [property: JsonPropertyName("path_name")]
    string PathName,
    [property: JsonPropertyName("event_type")]
    string EventType,
    [property: JsonPropertyName("element_id")]
    string ElementId,
    [property: JsonPropertyName("element_type")]
    string ElementType,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("country")]
    string Country,
    [property: JsonPropertyName("locale")] string Locale,
    [property: JsonPropertyName("platform")]
    string Platform,
    [property: JsonPropertyName("user_agent")]
    string UserAgent,
    [property: JsonPropertyName("timestamp")]
    long UnixTimestamp
);