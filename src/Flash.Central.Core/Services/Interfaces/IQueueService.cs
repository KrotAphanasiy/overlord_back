using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Flash.Central.Core.Services.Interfaces
{
    // ReSharper disable once TypeParameterCanBeVariant
    /// <summary>
    /// Interface. Specifies the contract for message's queue.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IQueueService<TMessage>
        where TMessage : class
    {
        /// <summary>
        /// Enqueue messages
        /// </summary>
        /// <param name="message">The object of message</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Dictionary</returns>
        Task<DeliveryResult<string, string>> Enqueue(TMessage message, CancellationToken ct);
    }
}
