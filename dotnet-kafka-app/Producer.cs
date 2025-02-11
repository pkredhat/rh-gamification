using System;
using Confluent.Kafka;

class Producer
{
    public static void Run()  // ✅ No async, fully synchronous
    {
        string bootstrapServers = "localhost:9092";  // Change if using a remote broker
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