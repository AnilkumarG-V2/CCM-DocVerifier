using V2.DocVerifier.Models;

namespace V2.DocVerifier.Services
{
    public interface IDocValidator
    {

        Task<List<GeminiDestinationData>> ValidateAsync(GeminiViewModel model);

    }
}
