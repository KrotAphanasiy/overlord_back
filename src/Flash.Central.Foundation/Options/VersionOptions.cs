namespace Flash.Central.Foundation.Options
{
    /// <summary>
    /// Class. Represents parameters for version options.
    /// </summary>
    public class VersionOptions
    {
        /// <summary>
        /// Hash.
        /// </summary>
        public string Hash { get; set; }
        /// <summary>
        /// Branch
        /// </summary>
        public string Branch { get; set; }
        /// <summary>
        /// The date of build
        /// </summary>
        public string BuildDate { get; set; }
        /// <summary>
        /// The date of deploy
        /// </summary>
        public string DeployDate { get; set; }
    }
}
