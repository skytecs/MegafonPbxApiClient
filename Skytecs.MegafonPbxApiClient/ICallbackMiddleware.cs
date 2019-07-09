using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Skytecs.MegafonPbxApiClient
{
    interface ICallbackMiddleware
    {
        Task InvokeAsync(HttpContext context, string callbackToken);
    }
}