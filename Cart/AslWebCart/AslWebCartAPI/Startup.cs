using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AslWebCartAPI.Startup))]
namespace AslWebCartAPI
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
