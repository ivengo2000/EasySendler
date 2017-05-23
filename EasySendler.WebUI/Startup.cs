using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EasySendler.Startup))]
namespace EasySendler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
