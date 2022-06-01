using System;
using DigitalSkynet.DotnetCore.DataAccess.Repository;
using Flash.Domain.Entities;

namespace Flash.Central.Data.Repositories.Interfaces
{
    /// <summary>
    /// IInterface. Specifies the contract for camera repository classes.
    /// Derived from IGenericDeletableRepository.
    /// </summary>
	public interface ICameraRepository : IGenericDeletableRepository<Camera, Guid>
	{

	}
}
