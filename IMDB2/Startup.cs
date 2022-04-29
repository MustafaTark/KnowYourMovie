using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;

namespace IMDB2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
         
            ConfigureAuth(app);
        }
    }
}
