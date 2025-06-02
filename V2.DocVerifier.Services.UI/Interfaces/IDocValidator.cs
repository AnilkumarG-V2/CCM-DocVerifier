using V2.DocVerifier.Models;

namespace V2.DocVerifier.Services.UI.Interfaces
{
    public interface IDocValidator
    {
        Task<List<GeminiDestinationData>> ValidateAsync(GeminiRequest model);
    }
}
