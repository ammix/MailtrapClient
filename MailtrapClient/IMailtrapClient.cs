
namespace Mailtrap;

public interface IMailtrapClient
{
    public MailResponse Send(MailAddress from, MailAddress[] to, Content content);
    public MailResponse Send(MailAddress from, MailAddress[] to, string subject, Content content);
    public MailResponse Send(MailAddress from, MailAddress[] to, string subject, Content content, Attachment[] attachments);
}
