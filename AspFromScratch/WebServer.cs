using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Autofac;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole;

namespace AspFromScratch {
    public class WebServer {
        private readonly HttpListener listener;
        private IContainer servicesContainer;
        private HttpDelegate pipelineStarter;
        private ILogger logger;

        private Task task;

        public WebServer() {
            this.listener = new HttpListener();
        }

        public WebServer On(string endpoint) {
            Guard.Against.NullOrWhiteSpace(endpoint, nameof(endpoint));
            this.listener.Prefixes.Add(endpoint);
            return this;
        }

        public WebServer Configure<T>() where T : IConfigurator, new() {
            this.logger = Log.Logger = new LoggerConfiguration().WriteTo.Console(LogEventLevel.Information).CreateLogger();

            var configurer = new T();
            var builder = new ContainerBuilder();

            configurer.ConfigureServices(builder);
            this.servicesContainer = builder.Build();

            var middlewareBuilder = new MiddlewareBuilder();
            configurer.ConfigureMiddleware(middlewareBuilder);
            this.pipelineStarter = middlewareBuilder.Build();

            return this;
        }

        public void Stop() {
            this.listener.Stop();
        }

        public WebServer Start() {
            this.listener.Start();
            Log.Logger.Information($"Listening on {this.listener.Prefixes.First()}.");
            this.task = Task.Run(() => {
                while (true) {
                    var context = this.listener.GetContext();
                    Task.Run(() => HandleRequest(context));
                }
            });
            return this;
        }

        private void HandleRequest(HttpListenerContext context) {
            this.pipelineStarter.Invoke(context, new Dictionary<string, object>());
        }
    }
}
