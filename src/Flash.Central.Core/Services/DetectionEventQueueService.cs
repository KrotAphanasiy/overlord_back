using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Foundation.Options;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Flash.Central.Dtos.MqDtos.Messages;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of IQueueService.
    /// </summary>
    public class DetectionEventQueueService : IQueueService<DetectionEventMessage>
    {
        private readonly IOptions<KafkaDetectionEventOptions> _options;

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="options">Parameters of KafkaDetectionEventOptions</param>
        public DetectionEventQueueService(IOptions<KafkaDetectionEventOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Enqueues event's messages
        /// </summary>
        /// <param name="message">Detetection event's message</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Dictionary with message as value</returns>
        public async Task<DeliveryResult<string, string>> Enqueue(DetectionEventMessage message, CancellationToken ct)
        {

            using var producer = new ProducerBuilder<string, string>(
                new ProducerConfig
                {
                    BootstrapServers = string.Join(" ", _options.Value.BootstrapServers.ToArray()),
                }
            ).Build();
            var messageJson = JsonSerializer.Serialize(message);

            return await producer.ProduceAsync(_options.Value.Topic,
                new Message<string, string> {Value = messageJson}, cancellationToken: ct);
        }
    }
}
