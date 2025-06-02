
using V2.DocVerifier.Models;

namespace V2.DocVerifier.Services.UI.Interfaces
{
    public interface IDocVerifier
    {
        Task<List<GeminiResponse>> ExecuteAsync(GeminiRequest model);

        Task<List<GeminiResponse>> LoadPayStubAsync(string fileName);

        Task<List<GeminiPayStubResult>> GetPayStubResultsAsync();
    }
}
