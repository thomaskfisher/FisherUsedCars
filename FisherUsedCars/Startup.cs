using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FisherUsedCars.Startup))]
namespace FisherUsedCars
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
