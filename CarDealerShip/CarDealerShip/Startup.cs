using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarDealerShip.Startup))]
namespace CarDealerShip
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
