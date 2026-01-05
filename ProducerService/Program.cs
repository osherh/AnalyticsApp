using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

// Declare queue
await channel.QueueDeclareAsync(queue: "my_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

// Produce various complex messages
var messages = new object[]
{
    new { 
        EventType = "user_login", 
        UserId = 123, 
        Timestamp = DateTime.UtcNow.ToString("o"), 
        IpAddress = "192.168.1.1",
        Device = new { Type = "mobile", OS = "iOS", Version = "14.5" },
    },
    new { 
        EventType = "purchase", 
        UserId = 456, 
        OrderId = Guid.NewGuid().ToString(),
        Timestamp = DateTime.UtcNow.ToString("o"),
        Items = new[] {
            new { ProductId = 789, Name = "Book", Quantity = 1, Price = 29.99 },
            new { ProductId = 790, Name = "Pen", Quantity = 2, Price = 5.99 }
        },
        TotalAmount = 41.97,
        Currency = "USD",
        PaymentMethod = new { Type = "credit_card", LastFour = "1234", Provider = "Visa" },
        ShippingAddress = new { Street = "123 Main St", City = "city", State = "state", Zip = "12345" }
    },
    new { 
        EventType = "page_view", 
        UserId = 123, 
        SessionId = Guid.NewGuid().ToString(),
        Page = "/home", 
        Duration = 5.2, 
        Timestamp = DateTime.UtcNow.ToString("o"), 
        Referrer = "google.com",
        UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_5 like Mac OS X) AppleWebKit/605.1.15",
        Tags = new[] { "homepage", "landing" },
        Metadata = new { CampaignId = "summer2023", Source = "email", Medium = "newsletter" }
    },
    new { 
        EventType = "error_log", 
        Service = "ProducerService", 
        ErrorMessage = "Connection timeout", 
        Severity = "high", 
        Timestamp = DateTime.UtcNow.ToString("o"),
        StackTrace = new[] {
            "at ProducerService.Program.Main() in Program.cs:line 10",
            "at System.Net.Http.HttpClient.SendAsync()",
            "at RabbitMQ.Client.ConnectionFactory.CreateConnectionAsync()"
        },
        Context = new { QueueName = "my_queue", RetryCount = 3, Environment = "production" },
        AffectedUsers = new[] { 123, 456, 789 }
    },
    new { 
        EventType = "user_registration", 
        UserId = 789, 
        Email = "user@example.com", 
        Country = "country", 
        Timestamp = DateTime.UtcNow.ToString("o"),
        Profile = new { 
            FirstName = "first_name", 
            LastName = "last_name", 
            Age = 30, 
            Interests = new[] { "reading", "swimming", "tennis" },
            Preferences = new { Newsletter = true, Notifications = new { Email = true, SMS = false, Push = true } }
        },
        RegistrationSource = "website",
        ReferralCode = "code123"
    },
    new { 
        EventType = "api_call", 
        Endpoint = "/api/v1/users", 
        Method = "POST", 
        StatusCode = 201, 
        ResponseTime = 0.245, 
        Timestamp = DateTime.UtcNow.ToString("o"),
        RequestHeaders = new { 
            Authorization = "Bearer token123", 
            ContentType = "application/json", 
            UserAgent = "PostmanRuntime/7.28.4" 
        },
        RequestBody = new { Name = "Full Name", Email = "name@example.com" },
        ResponseBody = new { Id = 1001, Created = true },
        IpAddress = "10.0.0.1",
        UserId = 789
    }
};

foreach (var message in messages)
{
    var json = JsonSerializer.Serialize(message);
    var body = Encoding.UTF8.GetBytes(json);
    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "my_queue", body: body);
    Console.WriteLine($"Sent: {json}");
}

Console.WriteLine("All messages sent.");
