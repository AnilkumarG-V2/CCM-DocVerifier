using V2.DocVerifier.Models;

namespace V2.DocVerifier.Services.Interfaces
{
    public interface IFileProcessor
    {
        Task SplitFileAsync(GeminiViewModel model);
    }
}
