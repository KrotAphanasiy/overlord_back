using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Foundation.Options;
using Microsoft.Extensions.Options;

namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of IPictureService.
    /// </summary>
    public class DiskPictureService : IPictureService
    {
        private readonly IOptions<ImageUploadOptions> _uploadConfiguration;

        /// <summary>
        /// Constructs path to get image from storage
        /// </summary>
        /// <param name="fileName">File's name</param>
        /// <returns>Image's path as String</returns>
        private string ConstructImagePath(string fileName)
        {
            return $"{_uploadConfiguration.Value.ImageUploadPath}{fileName}.jpg";
        }

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="uploadConfiguration"></param>
        public DiskPictureService(IOptions<ImageUploadOptions> uploadConfiguration)
        {
            _uploadConfiguration = uploadConfiguration;
        }

        /// <summary>
        /// Saves image
        /// </summary>
        /// <param name="image">Image as byte array</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>The image's file's name as String</returns>
        public async Task<string> Save(byte[] image, CancellationToken ct = default)
        {
            var fileName = Guid.NewGuid().ToString();

            var path = ConstructImagePath(fileName);

            using (var sourceStream = File.Open(path, FileMode.OpenOrCreate))
            {
                sourceStream.Seek(0, SeekOrigin.End);
                await sourceStream.WriteAsync(image, 0, image.Length, ct);
            }
            var result = $"{fileName}.jpg";
            return result;
        }

        /// <summary>
        /// Gets image's upload path
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Upload image's path as String</returns>
        public Task<string> GetImagesUploadPath(CancellationToken ct = default)
        {
            return Task.FromResult(_uploadConfiguration.Value.ImageUploadPath);
        }

        /// <summary>
        /// Deletes an image by the name of its file
        /// </summary>
        /// <param name="fileName">Image's filename</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
        public Task<bool> Delete(string fileName, CancellationToken ct = default)
        {
            var path = ConstructImagePath(fileName);

            if (!File.Exists(path)) return Task.FromResult(false);

            File.Delete(path);

            return Task.FromResult(true);
        }
    }
}
