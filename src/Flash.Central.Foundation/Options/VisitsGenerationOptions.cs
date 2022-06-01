using System;

namespace Flash.Central.Foundation.Options
{
    /// <summary>
    /// Class. Represents parameters of visit's generation.
    /// </summary>
    public class VisitsGenerationOptions
    {
        /// <summary>
        /// The level of upper plate number similarity
        /// </summary>
        public double UpperSimilarity { get; set; }
        /// <summary>
        /// The level of lower plate number similarity
        /// </summary>
        public double LowerSimilarity { get; set; }
        /// <summary>
        /// Minimal duration
        /// </summary>
        public TimeSpan MinimalDuration { get; set; }
        /// <summary>
        /// The date and time of start
        /// </summary>
        public DateTime StartingDate { get; set; }
    }
}
