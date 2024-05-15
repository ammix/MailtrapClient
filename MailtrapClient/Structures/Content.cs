using System.Text.Json.Serialization;

namespace Mailtrap;

public record Content
{
    public Content(string text)
    {
        Text = text;
    }

    public Content(string? text, string? html)
    {
        Text = text;
        Html = html;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Html { get; }
}
