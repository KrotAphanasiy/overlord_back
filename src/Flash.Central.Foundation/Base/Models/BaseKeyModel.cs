using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Flash.Central.Foundation.Base.Models
{
    /// <summary>
    /// Class. Represents base model class.
    /// Derived from BaseModel.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BaseKeyModel<TKey> : BaseModel where TKey : struct
    {
        [JsonIgnore]
        public TKey Id { get; set; }
    }
}
