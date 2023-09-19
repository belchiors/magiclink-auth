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
        _emailService = new EmailService();
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

            // TODO: Implement service to send e-mail to user
            await _emailService.SendEmail(entity);

            channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
        };

        channel.BasicConsume(
            queue: "task_queue",
            autoAck: false,
            consumer: consumer);

        Console.ReadLine();
    }
}
