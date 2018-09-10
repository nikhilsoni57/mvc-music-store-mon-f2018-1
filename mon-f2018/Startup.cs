using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mon_f2018.Startup))]
namespace mon_f2018
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
