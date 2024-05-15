using System.Text.Json.Serialization;

namespace Mailtrap;

internal record MailMessage
{
    public MailMessage(MailAddress from, MailAddress[] to, string subject, string? text, string? html, Attachment[]? attachments)
    {
        From = from;
        To = to;
        Subject = subject;
        Text = text;
        Html = html;
        Attachments = (attachments != null && attachments.Length == 0) ? null : attachments;
    }

    public MailAddress From { get; }
    public MailAddress[] To { get; }
    public string Subject { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Html { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Attachment[]? Attachments { get; }
}
