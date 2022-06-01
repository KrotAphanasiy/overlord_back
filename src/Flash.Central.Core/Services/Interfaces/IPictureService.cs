using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for picture services.
    /// </summary>
    public interface IPictureService
    {
        /// <summary>
        /// Saves pictures
        /// </summary>
        /// <param name="image">Image's byte array</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Image's link</returns>
        Task<string> Save(byte[] image, CancellationToken ct = default);
        /// <summary>
        /// Gets images upload path
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Upload path</returns>
        Task<string> GetImagesUploadPath(CancellationToken ct = default);
        /// <summary>
        /// Deletes an image by image's filename
        /// </summary>
        /// <param name="fileName">Image's filename</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
        Task<bool> Delete(string fileName, CancellationToken ct = default);

    }
}
