using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Autofac;
using Autofac.Features.ResolveAnything;
using Serilog;
using Serilog.Events;

namespace AspFromScratch {
    public class WebServer {
        private readonly HttpListener listener;
        public static IContainer Services { get; private set; }
        private HttpDelegate pipelineStarter;
        private ILogger logger;

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


            var configurer = new T(); // new Configurer
            var builder = new ContainerBuilder(); // Autofac
            builder.RegisterSource(
                new AnyConcreteTypeNotAlreadyRegisteredSource());        
            configurer.ConfigureServices(builder); 
            Services = builder.Build(); // Container
            var middlewareBuilder = new PipelineBuilder();
            configurer.ConfigureMiddleware(middlewareBuilder);
            this.pipelineStarter = middlewareBuilder.Build();

            return this;
        }

        public void Stop() {
            this.listener.Stop();
        }

        public void Start() {
            this.listener.Start();
            Log.Logger.Information($"Listening on {this.listener.Prefixes.First()}.");

            while (true) {
                var context = this.listener.GetContext();
                Task.Run(() => HandleRequest(context)).Wait();

            }
        }

        private void HandleRequest(HttpListenerContext context) {
            this.pipelineStarter.Invoke(context, new Dictionary<string, object>());
        }
    }
}
