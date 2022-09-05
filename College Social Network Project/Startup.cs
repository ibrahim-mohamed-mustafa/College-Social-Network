using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(College_Social_Network_Project.Startup))]
namespace College_Social_Network_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration { EnableDetailedErrors = true };

            ConfigureAuth(app);
            app.MapSignalR(); //Any connection or hub wire up and configuration should go here
            
        }
    }
}
