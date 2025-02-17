using System;
using System.Threading;
using Confluent.Kafka;

class Consumer
{
    public static void Run()
    {
        Console.WriteLine("Starting Kafka Consumer...");
        //string bootstrapServers = "localhost:9092";
        //string bootstrapServers = "host.containers.internal:9092";
        string bootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS");
        if (string.IsNullOrEmpty(bootstrapServers))
        {
            Console.WriteLine("KAFKA_BOOTSTRAP_SERVERS environment variable is not set.");
        }
        else
        {
            Console.WriteLine($"KAFKA_BOOTSTRAP_SERVERS: {bootstrapServers}");
        }




        string topic = "test-topic";
        string groupId = "test-group";

        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            consumer.Subscribe(topic);
            Console.WriteLine($"Subscribed to {topic}, waiting for messages...");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var result = consumer.Consume(cts.Token);
                        Console.WriteLine($"Received: {result.Message.Value}");
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}