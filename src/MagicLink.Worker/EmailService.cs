using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using MagicLink.Shared.Models;

namespace MagicLink.Worker;

public class EmailService : IEmailService
{
    private readonly SendGridClient _client;

    public EmailService(string apiKey)
    {
        _client = new SendGridClient(apiKey);
    }

    public async Task<Response> SendEmail(Message? message)
    {
        if (message == null)
        {
            throw new ArgumentNullException("message");
        }

        var from = new EmailAddress(message.From);
        var to = new EmailAddress(message.To);
        var email = MailHelper.CreateSingleEmail(from, to, message.Subject, message.Body, message.Body);

        return await _client.SendEmailAsync(email);
    }
}
