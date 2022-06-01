using System;

namespace Flash.Central.Foundation.Options
{
    /// <summary>
    /// Class. Represents parameters for superuser options;
    /// </summary>
    public class SuperuserOptions
    {
        /// <summary>
        /// Superuser guid
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Camera's api key.
        /// </summary>
        public string ApiKey { get; set; }
    }
}
