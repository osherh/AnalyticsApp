## Plan: Implement Schemaless Message Production and Consumption to ClickHouse

Update the producer to generate diverse complex JSON messages (e.g., user events, purchases, logs) and the consumer to insert them directly into ClickHouse's JSON column without schema awareness, leveraging ClickHouse's JSON type for schemaless storage.

### Steps
1. Modify [ProducerService/Program.cs](ProducerService/Program.cs) to produce varied complex messages with different structures and fields.
2. Update [ConsumerService/Program.cs](ConsumerService/Program.cs) to create a schemaless table with JSON column and insert raw JSON strings.
3. Test the updated services to ensure messages are produced and consumed correctly into ClickHouse.