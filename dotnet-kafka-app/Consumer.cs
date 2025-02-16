using System;
using System.Threading;
using Confluent.Kafka;

class Consumer
{
    public static void Run()
    {
        string bootstrapServers = "host.containers.internal:9092";
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