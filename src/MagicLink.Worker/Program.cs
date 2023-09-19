using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using EllipticCurve;
using MagicLink.Shared.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MagicLink.Worker;

class Program
{
    private static readonly IEmailService _emailService;

    static Program()
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

        if (apiKey == null)
        {
            throw new Exception("The SENDGRID_API_KEY environment variable is not found.");
        }

        _emailService = new EmailService(apiKey);
    }

    static void Main(string[] args)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };

        using var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "task_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        Console.WriteLine("Waiting for messages...");

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, args) =>
        {
            var body = args.Body.ToArray();
            var entity = Message.FromJson(Encoding.UTF8.GetString(body));

            var response = await _emailService.SendEmail(entity);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Message sent");
            }
            else
            {
                Console.WriteLine("Could not send e-mail message.");
            }

            channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
        };

        channel.BasicConsume(
            queue: "task_queue",
            autoAck: false,
            consumer: consumer);

        Console.ReadLine();
    }
}
