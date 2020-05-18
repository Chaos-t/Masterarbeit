using BackgroundApplication2.Controller;
using Restup.Webserver.Attributes;
using Restup.Webserver.Http;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BackgroundApplication2
{
    public sealed class Server
    {
        public static IAsyncAction Task_Run()
        {
            return Task_RunAsync().AsAsyncAction();
        }

        private static async Task Task_RunAsync()
        {
            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<RestController>();

            var configuration = new HttpServerConfiguration()
              .ListenOnPort(8081)
              .RegisterRoute("api", restRouteHandler)
              .EnableCors();

            var httpServer = new HttpServer(configuration);
            await httpServer.StartServerAsync();
        }

        
    }
}
