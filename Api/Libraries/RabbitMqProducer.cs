using RabbitMQ.Client;
using System.Text;

namespace Api.Libraries;

public class RabbitMqProducer
{
    private IConnection? _connection;
    private IChannel? _channel;

    public async Task RabbitMqProducerInit()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "192.168.1.15"
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: "demo-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public async Task SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: "demo-queue",
            body: body
        );

    }

}
