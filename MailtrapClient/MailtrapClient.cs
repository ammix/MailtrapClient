using System.Text.Encodings.Web;
using System.Text.Json;
using RestSharp;

namespace Mailtrap;

public class MailtrapClient : IMailtrapClient
{
    const string mailtrapEndpoint = "https://send.api.mailtrap.io/api/send";

    readonly IRestClient restClient;
    readonly IRestRequest restRequest;
    readonly MailMessageValidator validator;
    readonly JsonSerializerOptions jsonSerializerOptions;

    public MailtrapClient(string token)
        : this(token, new RestClient(), new RestRequest())
    { }

    internal MailtrapClient(string token, IRestClient client, IRestRequest request)
    {
        restClient = client;
        restRequest = request;
        validator = new MailMessageValidator();
        jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new LowerCaseNamingPolicy(),
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        request.Resource = mailtrapEndpoint;
        request.Method = Method.POST;

        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", $"Bearer {token}");
    }

    public MailResponse Send(MailAddress from, MailAddress[] to, Content content)
    {
        return Send(from, to, "Without subject", content, []);
    }

    public MailResponse Send(MailAddress from, MailAddress[] to, string subject, Content content)
    {
        return Send(from, to, subject, content, []);
    }

    public MailResponse Send(MailAddress from, MailAddress[] to, string subject, Content content, Attachment[] attachments)
    {
        var mailMessage = new MailMessage(from, to, subject, content.Text, content.Html, attachments);

        var validationResult = validator.Validate(mailMessage);
        if (!validationResult.IsValid)
            return new MailResponse(MailResponseCode.BadRequest, string.Join(';', validationResult.Errors));

        var mailMessageJson = JsonSerializer.Serialize(mailMessage, jsonSerializerOptions);
        restRequest.AddParameter("application/json", mailMessageJson, ParameterType.RequestBody);
        var restResponse = restClient.Execute(restRequest);

        return new MailResponse((MailResponseCode)restResponse.StatusCode, restResponse.Content);
    }
}
