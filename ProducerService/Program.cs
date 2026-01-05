using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

// Declare queue
await channel.QueueDeclareAsync(queue: "my_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

// Produce some messages
for (int i = 1; i <= 5; i++)
{
    var message = new { Id = i, Content = $"Message {i}" };
    var json = JsonSerializer.Serialize(message);
    var body = Encoding.UTF8.GetBytes(json);
    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "my_queue", body: body);
    Console.WriteLine($"Sent: {json}");
}

Console.WriteLine("All messages sent.");
