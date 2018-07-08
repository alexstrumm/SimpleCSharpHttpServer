using System;
using AspFromScratch.Middleware;
using Autofac;

namespace AspFromScratch {
    class Configurator : IConfigurator {
        public void ConfigureMiddleware(MiddlewareBuilder builder) {
            builder.Use<MvcMiddleware>();
        }

        public void ConfigureServices(ContainerBuilder services) {

        }
    }


    class Program {
        static void Main(string[] args) {
            var server = new WebServer().On("http://localhost:40001/")
                                        .Configure<Configurator>()
                                        .Start();
            while (true) {
                var input = Console.ReadLine();
                if (input.ToUpper().Equals("EXIT")) {
                    server.Stop();
                    break;
                }
            }
        }
    }
}
