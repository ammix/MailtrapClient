using Mailtrap;

namespace Mailtrap;

public record MailResponse
{
    internal MailResponse(MailResponseCode responseCode, string? responseMessage)
    {
        ResponseCode = responseCode;
        ResponseMessage = responseMessage;
    }

    public MailResponseCode ResponseCode { get; }
    public string? ResponseMessage { get; }
}
