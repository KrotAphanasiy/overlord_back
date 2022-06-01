using System;

namespace Flash.Central.Foundation.Options
{
    /// <summary>
    /// Class. Represents parameters for detection events' cleanup options.
    /// </summary>
    public class DetectionEventsCleanUpOptions
    {
        /// <summary>
        /// CronExpression
        /// </summary>
        public string CronExpression { get; set; }
        /// <summary>
        /// The date time of event's lifetime
        /// </summary>
        public TimeSpan EventLifetime { get; set; }
    }
}
