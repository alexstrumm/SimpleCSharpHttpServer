using Autofac;
using Unity;

namespace AspFromScratch {
    public interface IConfigurator {
        void ConfigureServices(ContainerBuilder services);
        void ConfigureMiddleware(PipelineBuilder builder);
    }
}
