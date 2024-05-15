namespace Mailtrap;

public record Attachment
{
    public Attachment(string content, string filename)
    {
        Content = content;
        Filename = filename;
    }

    public string Content { get; }
    public string Filename { get; }
}
