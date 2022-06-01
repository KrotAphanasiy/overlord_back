using System;
using DigitalSkynet.DotnetCore.DataStructures.Interfaces;

namespace Flash.Central.Foundation.Base.Entities
{
    /// <summary>
    /// Class. Represents base entitie's class.
    /// Derived from ISoftDeletable
    /// </summary>
    public class BaseEntity: ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
