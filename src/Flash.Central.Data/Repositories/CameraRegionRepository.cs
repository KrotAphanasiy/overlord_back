using AutoMapper;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Domain.Entities;
using DigitalSkynet.DotnetCore.DataAccess.Repository;


namespace Flash.Central.Data.Repositories
{
    /// <summary>
    /// Class. Implements contract for all members of ICameraRegionRepository.
    /// Derived from GenericDeletableRepository.
    /// </summary>
    public class CameraRegionRepository : GenericDeletableRepository<CentralDbContext, CameraRegion, long>, ICameraRegionRepository
    {
        /// <summary>
        /// Constructor. Initializes parameters.
        /// </summary>
        /// <param name="dbContext">The object of CentralDbContext
        /// <see cref="CentralDbContext"/>
        /// </param>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        public CameraRegionRepository(CentralDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
