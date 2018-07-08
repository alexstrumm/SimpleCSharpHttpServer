using Autofac;

namespace AspFromScratch {
    public interface IConfigurator {
        void ConfigureServices(ContainerBuilder services);
        void ConfigureMiddleware(MiddlewareBuilder builder);
    }
}
