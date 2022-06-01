using System;
using AutoMapper;
using DigitalSkynet.DotnetCore.DataAccess.Repository;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Domain.Entities;

namespace Flash.Central.Data.Repositories
{
    /// <summary>
    /// Class. Implements contract for all members of ICameraRepository.
    /// Derived from GenericDeletableRepository.
    /// </summary>
    public class CameraRepository : GenericDeletableRepository<CentralDbContext, Camera, Guid>, ICameraRepository
    {
        /// <summary>
        /// Constructor. Initializes parameters.
        /// </summary>
        /// <param name="dbContext">The object of CentralDbContext
        /// <see cref="CentralDbContext"/>
        /// </param>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        public CameraRepository (CentralDbContext dbContext, IMapper mapper) : base (dbContext, mapper)
        { }
    }
}
