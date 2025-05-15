using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V2.DocVerifier.Models;

namespace V2.DocVerifier.Services
{
    public interface IDocVerifier
    {
        Task<List<GeminiResponse>> Process(GeminiViewModel model);
    }
}
