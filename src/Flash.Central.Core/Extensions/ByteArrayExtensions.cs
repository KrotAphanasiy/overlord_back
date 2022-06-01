using System.Drawing;
using System.IO;

namespace Flash.Central.Core.Extensions
{
    /// <summary>
    /// Class. Implements image extension
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Extension. Creates image from byte array.
        /// </summary>
        /// <param name="imageBytes">byte array to convert into image</param>
        /// <returns></returns>
        public static Image ToImage(this byte[] imageBytes)
        {
            Image image;
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                image = Image.FromStream(ms, true);
            }
            return image;
        }
    }
}
