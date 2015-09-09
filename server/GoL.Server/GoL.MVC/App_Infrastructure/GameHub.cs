using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;

namespace GoL.MVC.App_Infrastructure
{
    public class GameHub : Hub
    {
        public override Task OnConnected()
        {

            if (Universe.ViewerCount < 1)
            {
                var thread = new Thread(() => Universe.Start());
                thread.Start();
            }

            Universe.ViewerCount++;

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Universe.ViewerCount--;

            if (Universe.ViewerCount < 1)
            {
                var thread = new Thread(Universe.Stop);
                thread.Start();
            }

            return base.OnDisconnected(stopCalled);
        }

        public void startSimulation(List<Cell> cells, int generation)
        {
            //TODO: start simulation
            //Universe.Stop();
            //Universe.Start(cells, generation);
            var thread = new Thread(() => Universe.Start(cells, generation));
            thread.Start();
        }

        public void pauseSimulation()
        {
            //Universe.Stop();
            //TODO: pause simulation
            var thread = new Thread(Universe.Stop);
            thread.Start();
        }
    }
}