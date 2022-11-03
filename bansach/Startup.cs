using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bansach.Startup))]
namespace bansach
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
