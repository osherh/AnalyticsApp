## Plan: Design Microservices App with RabbitMQ and ClickHouse

Design two .NET 8 microservices: a producer that sends messages to RabbitMQ, and a consumer that reads messages and writes to ClickHouse. Use RabbitMQ.Client for messaging, ClickHouse.Driver for database operations, and official Docker images for RabbitMQ and ClickHouse. This enables decoupled, scalable data processing with message queuing and OLAP storage.

### Steps
1. Set up Docker containers for RabbitMQ (rabbitmq:management) and ClickHouse (clickhouse/clickhouse-server) with ports exposed.
2. Create .NET 8 console app for producer service, install RabbitMQ.Client NuGet package.
3. Implement message production in producer: connect to RabbitMQ, declare queue, use JSON for structured data for the message format, publish messages asynchronously.
4. Create .NET 8 console app for consumer service, install RabbitMQ.Client and ClickHouse.Driver NuGet packages.
5. Define ClickHouse table schema for storing messages with appropriate data types.
6. Implement message consumption in consumer: subscribe to queue, process messages, insert into ClickHouse table.
7. Configure RabbitMQ with default direct exchange and one durable queue for routing messages.
8. Deployment: Use Docker Compose for local orchestration.
9. Test end-to-end flow: run producer to send messages, verify consumer processes and stores them in ClickHouse.
