using System;
using Confluent.Kafka;

class Producer
{
    public static void Run()  // ✅ No async, fully synchronous
    {
        Console.WriteLine("Starting Kafka Producer...");
        string bootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS");
        if (string.IsNullOrEmpty(bootstrapServers))
        {
            Console.WriteLine("KAFKA_BOOTSTRAP_SERVERS environment variable is not set.");
        }
        else
        {
            Console.WriteLine($"KAFKA_BOOTSTRAP_SERVERS: {bootstrapServers}");
        }


        string topic = "test-topic";  // Ensure this topic exists in Kafka

        var config = new ProducerConfig { BootstrapServers = bootstrapServers };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            for (int i = 1; i <= 5; i++)
            {
                string message = $"Hello Kafka11 {i}";
                var result = producer.ProduceAsync(topic, new Message<Null, string> { Value = message })
                                     .GetAwaiter().GetResult();  // ✅ Synchronously wait for the task to complete

                Console.WriteLine($"Produced to {result.TopicPartitionOffset}: {message}");
            }
        }
    }
}