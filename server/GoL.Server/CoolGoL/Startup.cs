using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Newtonsoft.Json;
using CoolGoL.App_Infrastructure;
using CoolGoL.Models;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(CoolGoL.Startup))]

namespace CoolGoL
{
    public partial class Startup
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
            ConfigureAuth(app);
            app.MapSignalR();
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => JsonSerializerFactory.Value);
        }
    }
}
