using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace melee_tracker_capstone.Services
{
    public class RabbitMQPublisher : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        public RabbitMQPublisher() 
        {
            // Cannot use await inside of constructor
            // have to use GetAwaiter().GetResult() to block the thread until the task is complete
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            factory.ClientProvidedName = "app:melee_tracker:replay-publisher";
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            _channel.QueueDeclareAsync(
                queue: "replay_upload",
                durable: true,
                exclusive: false,
                autoDelete: false
            ).GetAwaiter().GetResult();

        }

        public async Task PublishReplayJob(Guid replayId, string s3Key)
        {
            var message = new { replayId = replayId, s3Key = s3Key };
            var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: "replay_upload",
                body: body
                );
        }
            

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }

    }
}
