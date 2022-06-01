using System;

namespace Flash.Central.Foundation.Options
{
    /// <summary>
    /// Class. Represents parameters for picture's cleanup options.
    /// </summary>
    public class PicturesCleanupOptions
    {
        /// <summary>
        /// The lifetime of original picture
        /// </summary>
        public TimeSpan FullPicturesLifetime { get; set; }
        /// <summary>
        /// The lifetime of cropped picture
        /// </summary>
        public TimeSpan CroppedPicturesLifetime { get; set; }
        /// <summary>
        /// Cleanup expression
        /// </summary>
        public string CleanupCronExpression { get; set; }
        /// <summary>
        /// The number of pictures to keep
        /// </summary>
        public int PicturesToKeep { get; set; }
    }
}
