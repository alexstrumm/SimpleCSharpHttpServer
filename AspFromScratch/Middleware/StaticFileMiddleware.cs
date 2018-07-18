using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AspFromScratch.Middleware {
    class StaticFileMiddleware : IMiddleware {
        private readonly HttpDelegate next;

        public StaticFileMiddleware(HttpDelegate next) {
            this.next = next;
            
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data) {
            var url = context.Request.RawUrl;
            if (url.Contains('.')) {
                var filePath = "wwwroot" + url;
                if (File.Exists(filePath)) {
                    var bytes = File.ReadAllBytes(filePath);
                    context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                    context.Response.StatusCode = 200;
                } else {
                    context.Response.StatusCode = 404;
                    context.Response.StatusDescription = "File not found.";
                }
                context.Response.Close();
            } else {
                await next(context, data);
            }
        }
    }
}
