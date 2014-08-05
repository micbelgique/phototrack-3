using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trasys.PhotoTrack.BackEnd.Startup))]
namespace Trasys.PhotoTrack.BackEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
