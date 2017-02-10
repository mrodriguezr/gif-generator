using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GiftMaker.Startup))]
namespace GiftMaker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
