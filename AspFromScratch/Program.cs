using System;
using AspFromScratch.Middleware;
using AspFromScratch.Services;
using Autofac;
using Unity;

namespace AspFromScratch {
    class Configurator : IConfigurator {
        public void ConfigureServices(ContainerBuilder services) {
            // регистрируем сервисы
            services.RegisterType<FakeDataService>().As<IDataService>();
        }

        public void ConfigureMiddleware(PipelineBuilder builder) {
            builder.Use<MvcMiddleware>();
        }
    }


    class Program {
        static void Main(string[] args) {
            new WebServer().On("http://localhost:40001/")
                           .Configure<Configurator>()
                           .Start();
        }
    }
}
