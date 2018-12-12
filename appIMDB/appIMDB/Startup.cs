using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(appIMDB.Startup))]
namespace appIMDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
