using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mozzhhevelnik.Startup))]
namespace mozzhhevelnik
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
