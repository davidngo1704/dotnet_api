using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Api.Libraries;

public class RabbitMqConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "demo-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[x] Received: {message}");

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: "demo-queue",
            autoAck: true,
            consumer: consumer
        );
    }
}