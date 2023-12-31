using System.Text.Json;

namespace MagicLink.Shared.Models;

public record Message
{
    public string? Subject { get; init; }
    public string? From { get; init; }
    public string? To { get; init; }
    public string? Body { get; init; }

    public string ToJson()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return json;
    }

    public static Message? FromJson(string json)
    {
        return JsonSerializer.Deserialize<Message>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
};