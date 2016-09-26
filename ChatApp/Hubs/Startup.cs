using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChatApp.Hubs.Startup))]

namespace ChatApp.Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
