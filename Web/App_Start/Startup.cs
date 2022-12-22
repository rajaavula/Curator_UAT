using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LeadingEdge.Curator.Web.Startup))]

namespace LeadingEdge.Curator.Web
{
    public sealed partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}