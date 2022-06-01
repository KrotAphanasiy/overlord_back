using System;
using System.Text;
using Flash.Central.Core.Services.Interfaces;

namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of IEncodingService.
    /// </summary>
    public class EncodingService : IEncodingService
    {
        /// <summary>
        /// Decodes string from Base64
        /// </summary>
        /// <param name="base64String">String to decode</param>
        /// <returns></returns>
        public string DecodeBase64(string base64String)
        {
            byte[] data = Convert.FromBase64String(base64String);
            var origin = Encoding.ASCII.GetString(data);
            return origin;
        }

        /// <summary>
        /// Encodes string to Base64
        /// </summary>
        /// <param name="origin">String to encode</param>
        /// <returns></returns>
        public string EncodeToBase64(string origin)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(origin);
            return Convert.ToBase64String(data);
        }
    }
}
