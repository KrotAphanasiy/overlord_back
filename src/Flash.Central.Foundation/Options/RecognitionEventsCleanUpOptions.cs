using System;

namespace Flash.Central.Foundation.Options
{
    /// <summary>
    /// Class. Represents options parameters for recognition events
    /// </summary>
    public class RecognitionEventsCleanUpOptions
    {
        public string CronExpression { get; set; }
        public TimeSpan EventLifetime { get; set; }
    }
}
