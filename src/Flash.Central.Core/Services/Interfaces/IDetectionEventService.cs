using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Foundation.Pagination;
using Flash.Central.ViewModel.DetectionEvent;


namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for detection events services.
    /// Derived from IServiceBase.
    /// </summary>
    public interface IDetectionEventService : IServiceBase<DetectionEventModel, DetectionEventVm, Guid>
    {
        /// <summary>
        /// Gets paged collection of detection events. 
        /// </summary>
        /// <param name="pagination">The object of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of detection events</returns>
        Task<Paged<DetectionEventVm>> GetPagedAsync(PaginationModel pagination, CancellationToken ct);
        /// <summary>
        /// Gets collectio of detectionecents between dates
        /// </summary>
        /// <param name="startedAt">The date of event's start</param>
        /// <param name="endedAt">The date of event's end</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of detection events</returns>
        Task<List<DetectionEventVm>> GetBetweenDates(DateTime startedAt, DateTime endedAt, CancellationToken ct);
        /// <summary>
        /// Checks if the image exists
        /// </summary>
        /// <param name="imageLink">Link to the image</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
        Task<bool> ImageExists(string imageLink, CancellationToken ct = default);

        /// <summary>
        /// Gets list of image links to delete
        /// </summary>
        /// <param name="picturesToKeep">Number of pictures</param>
        /// <param name="timestamp">The date to take before</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of links</returns>
        Task<List<string>> GetOriginalImageLinksBeforeTimestamp(int picturesToKeep, DateTime timestamp, CancellationToken ct = default);
        /// <summary>
        /// Gets list of image links to delete
        /// </summary>
        /// <param name="picturesToKeep">Number of pictures</param>
        /// <param name="timestamp">The date to take before</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of links</returns>
        Task<List<string>> GetCroppedImageLinksBeforeTimestamp(int picturesToKeep, DateTime timestamp, CancellationToken ct = default);
    }
}
