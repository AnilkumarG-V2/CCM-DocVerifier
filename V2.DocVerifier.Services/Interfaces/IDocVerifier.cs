using V2.DocVerifier.Models;

namespace V2.DocVerifier.Services
{
    public interface IDocVerifier
    {
        Task<List<GeminiResponse>> ProcessAsync(GeminiViewModel model);
    }
}
