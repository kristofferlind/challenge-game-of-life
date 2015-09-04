using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace GoL.Server.App_Infrastructure
{
    public class GameHub : Hub
    {
        private int userCount = 0;

        public override Task OnConnected()
        {

            if (userCount < 1)
            {
                var thread = new Thread(Universe.Start);
                thread.Start();
            }


            userCount++;

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            userCount--;

            if (userCount < 1)
            {
                var thread = new Thread(Universe.Stop);
                thread.Start();
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}