using System.ComponentModel.DataAnnotations;
using DigitalSkynet.DotnetCore.DataStructures.Interfaces;

namespace Flash.Central.Foundation.Base.Entities
{
    /// <summary>
    /// Class. Represents generic base entitie's class.
    /// Derived from BaseEntity.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BaseKeyEntity<TKey> : BaseEntity, IHasKey<TKey> where TKey : struct
    {
        [Key]
        public TKey Id { get; set; }
    }
}
