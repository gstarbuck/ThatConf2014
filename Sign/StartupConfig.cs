using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System.Web.Http;

namespace Sign
{
    public class StartupConfig
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable HTTP static file server
            FileServerOptions fsOptions = new FileServerOptions();
            fsOptions.RequestPath = PathString.Empty;
            fsOptions.FileSystem = new PhysicalFileSystem(@"C:\ThatConf\2014\Sign\Sign");
            app.UseFileServer(fsOptions);

            // Enable WebAPI
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            app.UseWebApi(config);
        }
    }
}