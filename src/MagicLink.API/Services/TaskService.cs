using System;
using System.Text;
using RabbitMQ.Client;

using MagicLink.API.Interfaces;

namespace MagicLink.API.Services;

public class TaskService : ITaskService
{
    private readonly ConnectionFactory? _connectionFactory;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ILogger<TaskService> logger)
    {
        _logger = logger;

        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };
    }

    public void SendMessage(byte[] message)
    {
        try
        {
            // The connection abstracts the socket connection, and takes care of protocol version negotiation, authentication, and so on.
            using var connection = _connectionFactory?.CreateConnection();

            using var channel = connection?.CreateModel();

            channel?.QueueDeclare(
                queue: "task_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            // Mark message as persistent
            var properties = channel?.CreateBasicProperties();
            
            if (properties != null)
            {
                properties.Persistent = true;
            }
            else
            {
                throw new Exception("Could not create basic properties");
            }

            _logger.LogInformation("Sending message");

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "task_queue",
                basicProperties: properties,
                body: message);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return;
        }
    }
}