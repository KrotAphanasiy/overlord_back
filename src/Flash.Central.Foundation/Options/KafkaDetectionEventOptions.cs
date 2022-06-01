namespace Flash.Central.Foundation.Options
{
    /// <summary>
    /// Class. Represents parameters for Kafka detection events options
    /// </summary>
    public class KafkaDetectionEventOptions
    {
        /// <summary>
        /// The array of bootstrap servers
        /// </summary>
        public string[] BootstrapServers { get; set; }
        /// <summary>
        /// Topic
        /// </summary>
        public string Topic { get; set; }
    }
}
