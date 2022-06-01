namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for encoding services.
    /// </summary>
    public interface IEncodingService
    {
        string DecodeBase64(string base64String);
        string EncodeToBase64(string origin);
    }
}
