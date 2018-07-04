using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;


namespace AspFromScratch {
    class WebServer {
        private readonly HttpListener listener;
        private Task task;
        public WebServer() {
            this.listener = new HttpListener();
        }
        public WebServer On(string endpoint) {
            this.listener.Prefixes.Add(endpoint);
            return this;
        }
        public WebServer Start() {
            this.listener.Start();
            this.task = Task.Run(() => {
                while (true) {
                    var context = this.listener.GetContext();
                    Task.Run(() => HandleRequest(context));
                }
            });
            return this;
        }
        private void HandleRequest(HttpListenerContext context) {
            var streamWriter = new StreamWriter(context.Response.OutputStream);
            var url = context.Request.Url;
            var method = context.Request.HttpMethod;

            try {
                var segments = url.Segments.ToArray();

                var controllerName = segments[1].Split('/')[0];
                var actionName = segments[2].Split('/')[0];

                var controllerType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => {
                    return t.Name.ToUpper() == $"{controllerName.ToUpper()}CONTROLLER";
                });
                var actionInfo = controllerType.GetMethods().FirstOrDefault(m => {
                    return m.Name.ToUpper() == actionName.ToUpper() && m.GetCustomAttribute<HttpMethodAttribute>()?.Method?.ToUpper() == method.ToUpper();
                });
                var controllerInstance = Activator.CreateInstance(controllerType);
                var result = actionInfo.Invoke(controllerInstance, null);
                context.Response.ContentType = "text/html";
                context.Response.StatusCode = 200;
                streamWriter.Write(result);
            } catch {
                context.Response.StatusCode = 400;
                context.Response.StatusDescription = "Failed to handle request.";
                streamWriter.Write("<p>Failed to handle request.</p>");
                Console.WriteLine("Failed to handle request");
            } finally {
                streamWriter.Close();
            }
        }
    }
}
