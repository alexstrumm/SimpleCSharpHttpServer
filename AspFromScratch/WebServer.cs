using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Autofac;
using Autofac.Core;
using Autofac.Features.ResolveAnything;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole;
using Unity;

namespace AspFromScratch {
    public class WebServer {
        private readonly HttpListener listener;
        // автофаковый контейнер зависимостей, хранит все зарегистрированные сервисы
        // синглтон для того, чтобы можно было резолвить сервисы из любого мидлвейра
        // по хорошему, конечно, стоило бы его в мидлвейры передавать при их создании
        // чтобы только мидлвейры имели доступ к нему, но так проще
        public static IContainer Services { get; private set; }
        // метод первого мидлвейра в пайплайне
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

            var configurer = new T();
            // создаем билдер контейнера зависимостей автофака
            var builder = new ContainerBuilder();
            // этот регистерсорс нужен для того, чтобы можно было резолвить через автофак
            // даже те типы, которые не были зарегистрированы
            // автофак сам найдет реализацию каждого интерфейса из параметров конструктора
            // если они, конечно, были зарегистрированы
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            // передаем билдер в метод конфигуратора, тот добавляет свои зависимости в билдер
            configurer.ConfigureServices(builder);
            // строим контейнер, который будет содержать все зависимости приложения
            Services = builder.Build();
            var middlewareBuilder = new PipelineBuilder();
            // аналогично, передаем конфигуратору билдер пайплайна мидлвейров
            configurer.ConfigureMiddleware(middlewareBuilder);
            // билдим, получаем готовый пайплайн и ссылку на метод первого мидлвейра
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
                // приходит запрос
                var context = this.listener.GetContext();
                // отдаем его первому мидлвейру
                Task.Run(() => HandleRequest(context));
            }
        }

        private void HandleRequest(HttpListenerContext context) {
            this.pipelineStarter.Invoke(context, new Dictionary<string, object>());
        }
    }
}
