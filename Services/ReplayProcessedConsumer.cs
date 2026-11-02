using System.Text;
using System.Text.Json;
using melee_tracker_capstone.Data;
using melee_tracker_capstone.Data.Entities;
using melee_tracker_capstone.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace melee_tracker_capstone.Services
{
    public class ReplayProcessedConsumer : BackgroundService
    {
        private const string QueueName = "replay_processed";
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection? _connection;
        private IChannel? _channel;

        public ReplayProcessedConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                ClientProvidedName = "app:melee_tracker:replay-processed-consumer"
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);

            await _channel.BasicQosAsync(0, 1, false, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += OnMessageReceived;

            await _channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);
        }

        private async Task OnMessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var message = JsonSerializer.Deserialize<ReplayProcessedMessage>(json, options);

                if (message == null)
                {
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var replay = await db.Replays.FindAsync(message.ReplayId);
                if (replay != null)
                {
                    if (message.Status == "complete")
                    {
                        replay.Status = ReplayStatus.Complete;
                        replay.ProcessedS3Key = message.ProcessedS3Key;
                        replay.StageId = message.StageId;
                        replay.PlayedAt = message.PlayedAt;
                        replay.LastFrame = message.LastFrame;
                    }
                    else
                    {
                        replay.Status = ReplayStatus.Failed;
                        replay.FailureReason = message.FailureReason;
                    }

                    await db.SaveChangesAsync();
                }

                await _channel!.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to handle replay_processed message: {ex}");
                await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
            }
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}