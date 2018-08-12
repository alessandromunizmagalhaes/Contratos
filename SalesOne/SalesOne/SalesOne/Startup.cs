using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SalesOne.Startup))]
namespace SalesOne
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
