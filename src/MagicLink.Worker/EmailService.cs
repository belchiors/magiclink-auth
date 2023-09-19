using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using MagicLink.Shared.Models;

namespace MagicLink.Worker;

public class EmailService : IEmailService
{
    private readonly SendGridClient _client;

    public EmailService()
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

        if (apiKey == null)
        {
            throw new Exception("The SENDGRID_API_KEY environment variable is not found.");
        }

        _client = new SendGridClient(apiKey);
    }

    public async Task<Response> SendEmail(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException("message");
        }

        var from = new EmailAddress("test@example.com", "Example User");
        var subject = "Sending with SendGrid is Fun";
        var to = new EmailAddress(message.From);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, "", message.Body);
        return await _client.SendEmailAsync(msg);
    }
}
