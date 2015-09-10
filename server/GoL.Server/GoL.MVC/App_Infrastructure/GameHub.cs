using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using GoL.MVC.Models;

namespace GoL.MVC.App_Infrastructure
{
    public class GameHub : Hub
    {
        public List<Universe> Universes = new List<Universe>();
        public static Universe CurrentUniverse { get; set; }

        public override Task OnConnected()
        {
            if (Universe.ViewerCount < 1)
            {
                var universe = Universe.Start();
                Universes.Add(universe);
                CurrentUniverse = universe;
            }

            Universe.ViewerCount++;

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Universe.ViewerCount--;

            if (Universe.ViewerCount < 1)
            {
                CurrentUniverse.Stop();
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
            CurrentUniverse.Stop();
        }

        public List<Generation> getHistoryBatch(int startGeneration, int endGeneration)
        {
            return CurrentUniverse.GetHistoryBatch(startGeneration, endGeneration);
        }
    }
}