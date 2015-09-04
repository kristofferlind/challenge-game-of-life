using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using GoL.Server.App_Infrastructure;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(GoL.Server.Startup))]

namespace GoL.Server
{
    public class Startup
    {
        private static readonly Lazy<JsonSerializer> JsonSerializerFactory = new Lazy<JsonSerializer>(GetJsonSerializer);

        private static JsonSerializer GetJsonSerializer()
        {
            var jsonSerializer = new JsonSerializer
            {
                ContractResolver = new FilteredCamelCasePropertyNamesContractResolver
                {
                    // 1) Register all types in specified assemblies:
                    AssembliesToInclude =
                    {
                        typeof (Startup).Assembly,
                        typeof (Cell).Assembly
                    }
                }
            };

            return jsonSerializer;
        }

        public void Configuration(IAppBuilder app)
        {
            //app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => JsonSerializerFactory.Value);
        }
    }
}
