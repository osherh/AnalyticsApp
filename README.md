# Analytics App

A microservices application consisting of a producer and consumer service, using RabbitMQ for messaging and ClickHouse for data storage.

## Architecture

- **Producer Service**: Sends JSON messages to RabbitMQ queue.
- **Consumer Service**: Consumes messages from RabbitMQ and inserts them into ClickHouse database.
- **RabbitMQ**: Message broker with one queue.
- **ClickHouse**: OLAP database for storing messages.

## Prerequisites

- .NET 8 SDK
- Docker

## Setup

1. Start the services:
   ```bash
   docker-compose up -d
   ```

2. Run the consumer service:
   ```bash
   cd ConsumerService
   dotnet run
   ```

3. In another terminal, run the producer service:
   ```bash
   cd ProducerService
   dotnet run
   ```

## Verification

- Check RabbitMQ management UI at http://localhost:15672 (guest/guest)
- Query ClickHouse: `SELECT * FROM default.messages`

## Technologies

- .NET 8
- RabbitMQ.Client 7.2.0
- ClickHouse.Driver 0.9.0
- Docker