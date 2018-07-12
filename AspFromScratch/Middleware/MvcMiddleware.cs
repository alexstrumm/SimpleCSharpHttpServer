using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;

namespace AspFromScratch.Middleware {
    public class MvcMiddleware : IMiddleware {
        private readonly HttpDelegate next;

        public MvcMiddleware(HttpDelegate next) {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data) {
            var streamWriter = new StreamWriter(context.Response.OutputStream);
            var streamReader = new StreamReader(context.Request.InputStream);
            var url = context.Request.RawUrl;
            var method = context.Request.HttpMethod;

            try {
                var parts = url.Split('/');

                var controllerName = parts[1];
                var actionName = parts[2];

                var controllerType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => {
                    return t.Name.ToUpper() == $"{controllerName.ToUpper()}CONTROLLER";
                });
                var actionInfo = controllerType.GetMethods().FirstOrDefault(m => {
                    return m.Name.ToUpper() == actionName.ToUpper() && m.GetCustomAttribute<HttpMethodAttribute>()?.Method?.ToUpper() == method.ToUpper();
                });
                // создаем контроллер не через активатор а через автофак
                var controllerInstance = WebServer.Services.Resolve(controllerType);

                object[] args = null;
                if (actionInfo.GetParameters().Count() > 0) {
                    args = new object[actionInfo.GetParameters().Count()];
                    var i = 0;
                    var dict = new Dictionary<string, string>();

                    streamReader.ReadToEnd().Split('&').Select(s => s.Split('=')).ToList().ForEach(p => dict.Add(p[0].ToUpper(), p[1]));

                    foreach (var item in actionInfo.GetParameters()) {
                        if (dict.TryGetValue(item.Name.ToUpper(), out var val)) {
                            args[i] = val;
                        } else {
                            args[i] = null;
                        }
                        ++i;
                    }
                }
                var result = actionInfo.Invoke(controllerInstance, args);
                context.Response.ContentType = "text/html";
                context.Response.StatusCode = 200;
                streamWriter.Write(result);
            } catch (Exception ex) {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = "Failed to handle request.";
                streamWriter.Write("<p>Failed to handle request.</p>");
                Console.WriteLine("Failed to handle request");
            } finally {

            }
        }
    }
}
