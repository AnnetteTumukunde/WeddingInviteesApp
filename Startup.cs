using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeddingInviteesApp.Startup))]
namespace WeddingInviteesApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
