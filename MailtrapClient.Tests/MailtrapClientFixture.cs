using System.Reflection;

using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using RestSharp;

using Mailtrap;

namespace MailtrapClientTests;

[TestFixture]
public class MailtrapClientFixture
{
    IRestClient restClient;
    IRestRequest restRequest;
    Mailtrap.MailtrapClient sut;

    [SetUp]
    public void Setup()
    {
        restClient = Substitute.For<IRestClient>();
        restRequest = Substitute.For<IRestRequest>();
        sut = new MailtrapClient("token", restClient, restRequest);
    }

    [Test]
    public void Should_successfully_send_email_without_subject()
    {
        restClient.Execute(restRequest)
            .Returns(x => new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = "{success: true}"
            });

        var mailResponse = sut.Send(
            new MailAddress("sales@example.com"),
            [new MailAddress("john_doe@example.com")],
            new Content("Congratulations on your order no. 1234"));

        AssertRequest("MailMessageSmallest.json");
        mailResponse.ResponseCode.Should().Be(MailResponseCode.Success);
        mailResponse.ResponseMessage.Should().Be("{success: true}");
    }

    [Test]
    public void Should_successfully_send_email()
    {
        restClient.Execute(restRequest)
            .Returns(x => new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = "{success: true}"
            });

        var mailResponse = sut.Send(
            new MailAddress(
                "sales@example.com",
                "Example Sales Team"),
            [new MailAddress(
                "john_doe@example.com",
                "John Doe")],
            "Your Example Order Confirmation",
            new Content(
                "Congratulations on your order no. 1234"));

        AssertRequest("MailMessage.json");
        mailResponse.ResponseCode.Should().Be(MailResponseCode.Success);
        mailResponse.ResponseMessage.Should().Be("{success: true}");
    }

    [Test]
    public void Should_successfully_send_email_with_html_and_attachments()
    {
        restClient.Execute(restRequest)
            .Returns(x => new RestResponse { 
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = "{success: true}" });

        var mailResponse = sut.Send(
            new MailAddress(
                "sales@example.com",
                "Example Sales Team"),
            [new MailAddress(
                "john_doe@example.com",
                "John Doe")],
            "Your Example Order Confirmation",
            new Content(
                "Congratulations on your order no. 1234",
                "<!DOCTYPE html><html><body><p>Congratulations on your order no. 1234</p></body></html>"),
            [new Attachment("ABCDEF", "Document.pdf")]);

        AssertRequest("MailMessageFull.json");
        mailResponse.ResponseCode.Should().Be(MailResponseCode.Success);
        mailResponse.ResponseMessage.Should().Be("{success: true}");
    }

    [Test]
    public void Should_fail_send_email_when_text_is_empty()
    {
        var mailResponse = sut.Send(
            new MailAddress("sales@example.com"),
            [new MailAddress("john_doe@example.com")],
            new Content(""));

        mailResponse.ResponseCode.Should().Be(MailResponseCode.BadRequest);
        mailResponse.ResponseMessage.Should().Be("Text or Html must be specified or both;Text or Html must be specified or both");
    }

    [Test]
    public void Should_fail_send_email_when_token_is_wrong()
    {
        restClient.Execute(restRequest)
            .Returns(x => new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized
            });

        var mailResponse = sut.Send(
            new MailAddress("sales@example.com"),
            [new MailAddress("john_doe@example.com")],
            new Content("Congratulations on your order no. 1234"));

        mailResponse.ResponseCode.Should().Be(MailResponseCode.Unauthorized);
    }

    void AssertRequest(string jsonResource)
    {
        restRequest.Resource.Should().Be("https://send.api.mailtrap.io/api/send");
        restRequest.Method.Should().Be(Method.POST);

        restRequest.Received().AddHeader("Content-Type", "application/json");
        restRequest.Received().AddHeader("Accept", "application/json");
        restRequest.Received().AddHeader("Authorization", $"Bearer token");
        restRequest.Received().AddParameter("application/json", ReadJsonResource(jsonResource), ParameterType.RequestBody);
    }

    static string ReadJsonResource(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(name));

        using Stream stream = assembly.GetManifestResourceStream(resourcePath);
        using StreamReader reader = new StreamReader(stream);
        var jsonString = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(reader.ReadToEnd()));
        return jsonString;
    }
}
