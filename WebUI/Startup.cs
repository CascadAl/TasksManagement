using Microsoft.Owin;
using Owin;
using WebUI.App_Start;

[assembly: OwinStartupAttribute(typeof(WebUI.Startup))]
namespace WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // при старте приложения вызываем свой метод для конфигурации
            AutofacConfig.Configure(app);
        }
    }
}
