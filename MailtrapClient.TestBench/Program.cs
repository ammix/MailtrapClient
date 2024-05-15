using Mailtrap;

namespace MailtrapClientTestBench;

internal class Program
{
    static void Main(string[] args)
    {
        var sender = new MailAddress("mailtrap@demomailtrap.com", "Mailtrap Test");
        var receiver = new MailAddress("maksym.brendel@gmail.com", "Maksym Brendel");
        var content = new Content("Welcome to Mailtrap Sending!");

        var client = new MailtrapClient(token: "<token here>");
        var response = client.Send(
            from: sender,
            to: [receiver],
            subject: "Hello from Mailtrap!",
            content: content);

        Console.WriteLine(response);
    }
}
