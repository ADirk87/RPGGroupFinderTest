using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GroupFinderCapstone.Startup))]
namespace GroupFinderCapstone
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
