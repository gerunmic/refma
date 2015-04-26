using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Refma.Startup))]
namespace Refma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
