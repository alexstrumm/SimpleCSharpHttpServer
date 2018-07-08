using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AspFromScratch {
    public interface IMiddleware {
        Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data);
    }
}
