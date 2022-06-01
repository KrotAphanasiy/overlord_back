using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Foundation.Pagination;
using Flash.Central.ViewModel.RecognitionEvent;

namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for recognition event services.
    /// Derived from IServiceBase.
    /// </summary>
    public interface IRecognitionEventService : IServiceBase<RecognitionEventModel, RecognitionEventVm, Guid>
    {
        /// <summary>
        /// Gets paged collection of recognition events
        /// </summary>
        /// <param name="pagination">The objec t of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection</returns>
        Task<Paged<RecognitionEventVm>> GetPagedAsync(PaginationModel pagination, CancellationToken ct);
        /// <summary>
        /// Gets paged collection of recognition events by camera's guid. Can be filtered by dates.
        /// </summary>
        /// <param name="pagination">The object of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="cameraUid">Camera's guid</param>
        /// <param name="startedAt">Optional. The date of event's start</param>
        /// <param name="endedAt">Optional. The date of event's end</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection</returns>
        Task<Paged<RecognitionEventVm>> GetPagedByCameraUidAsync(PaginationModel pagination, Guid cameraUid, DateTime? startedAt, DateTime? endedAt, CancellationToken ct);
        /// <summary>
        /// Gets recognition events between sertain dates. Can be filtered by id;
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="startedAt">Optional. The date of event's start</param>
        /// <param name="endedAt">Optional. The date of event's end</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection</returns>
        Task<List<RecognitionEventVm>> GetBetweenDates(Guid? id, DateTime startedAt, DateTime endedAt, CancellationToken ct);
        /// <summary>
        /// Checks if the image exisits in a storage.
        /// </summary>
        /// <param name="imageLink">Link to the image</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
        Task<bool> ImageExists(string imageLink, CancellationToken ct = default);

        /// <summary>
        /// Gets list of image links to delete and responding visit ids and a link to image to replace deleted ones
        /// </summary>
        /// <param name="picturesToKeep">Number of pictures</param>
        /// <param name="timestamp">The date to take before</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection</returns>
        Task<List<(string, long, string)>> GetImageLinksFromProcessedEvents(int picturesToKeep, DateTime timestamp, CancellationToken ct = default);
        /// <summary>
        /// Gets list of image links to delete and responding visit ids and a link to image to replace deleted ones
        /// </summary>
        /// <param name="picturesToKeep">Number of pictures</param>
        /// <param name="timestamp">The date to take before</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection</returns>
        Task<List<(string, long, string)>> GetProcessedImageLinksFromProcessedEvents(int picturesToKeep, DateTime timestamp, CancellationToken ct = default);
    }
}
