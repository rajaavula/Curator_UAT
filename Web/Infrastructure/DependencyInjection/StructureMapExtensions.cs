using Microsoft.Extensions.Configuration;
using StructureMap;

namespace LeadingEdge.Curator.Web.Infrastructure.DependencyInjection
{
    public static class StructureMapExtensions
    {
        public static TSettings AddSettings<TSettings>(this Registry registry, IConfiguration configuration, bool bindNonPublicProperties = true)
            where TSettings : class, new()
        {
            var settings = new TSettings();

            configuration.Bind(settings, options => options.BindNonPublicProperties = bindNonPublicProperties);

            registry.For<TSettings>().Use(settings).Singleton();

            return settings;
        }
    }
}