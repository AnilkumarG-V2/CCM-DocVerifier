using V2.DocVerifier.Models;

namespace V2.DocVerifier.Services.Interfaces
{
    public interface IGeminiClient
    {
        Task<List<T>> GetGeminiResponseAsync<T>(string prompt, string fileName) where T : GeminiBaseModel;
    }
}
