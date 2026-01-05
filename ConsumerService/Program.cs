using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using ClickHouse.Driver.ADO;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

// Declare queue
await channel.QueueDeclareAsync(queue: "my_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

// ClickHouse connection
string chConnectionString = "Host=localhost;Port=8123;Username=default;Password=;Database=default;Compression=true";
await using var chConnection = new ClickHouseConnection(chConnectionString);
await chConnection.OpenAsync();

// Create table if not exists
await using var createCommand = chConnection.CreateCommand();
createCommand.CommandText = "CREATE TABLE IF NOT EXISTS default.messages (id Int64, content String) ENGINE = MergeTree ORDER BY id";
await createCommand.ExecuteNonQueryAsync();

Console.WriteLine("Waiting for messages...");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);
    var message = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
    long id = Convert.ToInt64(message["Id"]);
    string content = message["Content"].ToString();

    // Insert into ClickHouse
    await using var insertCommand = chConnection.CreateCommand();
    insertCommand.CommandText = $"INSERT INTO default.messages (id, content) VALUES ({id}, '{content.Replace("'", "''")}')";
    await insertCommand.ExecuteNonQueryAsync();

    Console.WriteLine($"Inserted: {json}");
};

await channel.BasicConsumeAsync("my_queue", autoAck: true, consumer: consumer);

Console.ReadLine();
