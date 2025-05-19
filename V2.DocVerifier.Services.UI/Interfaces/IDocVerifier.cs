
using V2.DocVerifier.Models;

namespace V2.DocVerifier.Services.UI.Interfaces
{
    public interface IDocVerifier
    {
        Task<List<GeminiResponse>> Execute(GeminiRequest model);
    }
}
