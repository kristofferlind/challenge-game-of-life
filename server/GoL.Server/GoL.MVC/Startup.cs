using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Newtonsoft.Json;
using GoL.MVC.App_Infrastructure;
using GoL.MVC.Models;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(GoL.MVC.Startup))]

namespace GoL.MVC
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
            app.MapSignalR();
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => JsonSerializerFactory.Value);
        }
    }
}
