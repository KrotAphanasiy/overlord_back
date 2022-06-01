using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies base contract for services
    /// </summary>
    /// <typeparam name="TInputModel">Generic class of model</typeparam>
    /// <typeparam name="TViewModel">Generic class of view model</typeparam>
    /// <typeparam name="TKey">Key</typeparam>
    public interface IServiceBase<TInputModel, TViewModel, TKey> where TKey : struct
    {
        /// <summary>
        /// Generic method to get collection of objects
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        Task<List<TViewModel>> GetAll(CancellationToken ct);
        /// <summary>
        /// Generic method. Gets object by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TViewModel> Get(TKey id, CancellationToken ct);
        /// <summary>
        /// Generic method. Creates new object.
        /// </summary>
        /// <param name="model">Generic input model</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created object</returns>
        Task<TViewModel> Create(TInputModel model, CancellationToken ct);
        /// <summary>
        /// Generic method. Updates object by id
        /// </summary>
        /// <param name="id">Object;s id</param>
        /// <param name="model">Generic input model</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated object</returns>
        Task<TViewModel> Update(TKey id, TInputModel model, CancellationToken ct);
        /// <summary>
        /// Generic method. Deletes object by id.
        /// </summary>
        /// <param name="id">Object's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
        Task<bool> Delete(TKey id, CancellationToken ct);
    }
}
