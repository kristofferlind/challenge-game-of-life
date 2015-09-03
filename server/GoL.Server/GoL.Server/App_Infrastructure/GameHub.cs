using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace GoL.Server.App_Infrastructure
{
    public class GameHub : Hub
    {
        private int userCount = 0;
        Universe universe = new Universe(
                new List<Cell>() {
                    new Cell()
                    { X=2, Y=10 },
                    new Cell()
                    { X=3, Y=10 },
                    new Cell()
                    { X=3, Y=11 },
                    new Cell()
                    { X=4, Y=10 },
                    new Cell()
                    { X=4, Y=11 },
                    new Cell()
                    { X=5, Y=11 },
                });

        public override Task OnConnected()
        {
            userCount++;

            if (!universe.Running) 
                universe.Start();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            userCount--;

            if (userCount < 1)
                universe.Running = false;

            return base.OnDisconnected(stopCalled);
        }
    }
}