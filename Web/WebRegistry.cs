using LeadingEdge.Curator.Web.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Owin.Security;
using StructureMap;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace LeadingEdge.Curator.Web
{
    public sealed class WebRegistry : Registry
    {
        public WebRegistry(IConfiguration configuration)
        {
            Configuration = configuration;

            ScanAssembly();

            RegisterAuthentication();

            RegisterSession();

            RegisterPrincipal();
        }

        public IConfiguration Configuration { get; }

        private void ScanAssembly()
        {
            // Apply project-wide conventions
            Scan(scanner =>
            {
                // Scan the following assemblies
                scanner.TheCallingAssembly();

                // Apply the following conventions
                scanner.With(new AspNetMvcConvention());
            });
        }

        private void RegisterAuthentication()
        {
            For<IAuthenticationManager>().Use(c => HttpContext.Current.GetOwinContext().Authentication).ContainerScoped();
        }

        private void RegisterSession()
        {
            For<HttpSessionState>().Use(c => HttpContext.Current.Session).ContainerScoped();
        }

        private void RegisterPrincipal()
        {
            For<IPrincipal>().Use(c => HttpContext.Current.User).ContainerScoped();
        }
    }
}