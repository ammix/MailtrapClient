using System.Text.Json.Serialization;

namespace Mailtrap;

public record MailAddress
{
    public MailAddress(string email, string name)
    {
        Email = email;
        Name = name;
    }
    public MailAddress(string email)
    {
        Email = email;
    }

    public string Email { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; }
}
