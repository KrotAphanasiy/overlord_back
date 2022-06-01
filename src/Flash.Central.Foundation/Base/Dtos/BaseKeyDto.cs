using System.Threading.Tasks;

namespace Flash.Central.Foundation.Base.Dtos
{
    /// <summary>
    /// Class. Represents generic base Data Transfer Object.
    /// Derived from BaseDto.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BaseKeyDto<TKey> : BaseDto where TKey : struct
    {
        public TKey Id { get; set; }
    }
}
