namespace Flash.Central.Foundation.Base.ViewModels
{
    /// <summary>
    /// Class. Represents base view model's class.
    /// Derived from BaseVm.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class BaseKeyVm<TKey> : BaseVm where TKey : struct
    {
        public TKey Id { get; set; }
    }
}
