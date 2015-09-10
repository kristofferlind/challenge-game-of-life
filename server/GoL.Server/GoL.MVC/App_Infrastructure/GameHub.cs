using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using GoL.MVC.Models;

namespace GoL.MVC.App_Infrastructure
{
    public class GameHub : Hub
    {
        public List<HubUser> users = new List<HubUser>();
        public List<Universe> Universes = new List<Universe>();
        public static Universe CurrentUniverse { get; set; }

        public HubUser User
        {
            get
            {
                var user = Context.User;

                if (user.Identity.IsAuthenticated)
                {
                    var hubUser = new HubUser
                    {
                        Username = user.Identity.Name,
                        Universe = Universe.Start(),
                        ConnectionId = Context.ConnectionId
                    };

                    return User;
                } else
                {
                    return null;
                }
            }
        }

        public override Task OnConnected()
        {
            //var user = Context.User;
            var user = User;

            if (user != null)
            {
                //var hubUser = new HubUser
                //{
                //    Username = user.Identity.Name,
                //    Universe = Universe.Start(),
                //    ConnectionId = Context.ConnectionId
                //};

                users.Add(user);

                Groups.Add(Context.ConnectionId, user.Username);
            } else {
                
                //if connecting to users stream
                
                //TODO: connect to stream as observer
                //increase viewercount

                //else

                //TODO: watch root simulation?
            }


            //old
            //var universe = Universe.Start();
            //Universes.Add(universe);
            //CurrentUniverse = universe;

            //CurrentUniverse.ViewerCount++;

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //if (CurrentUniverse.)
            //{
            //    CurrentUniverse.ViewerCount--;

            //    if (CurrentUniverse.ViewerCount < 1)
            //    {
            //        CurrentUniverse.Stop();
            //    }
            //}

            return base.OnDisconnected(stopCalled);
        }

        [Authorize]
        public void startSimulation(List<Cell> cells, int generation)
        {
            CurrentUniverse = Universe.Start(cells, generation);
        }

        [Authorize]
        public void pauseSimulation()
        {
            CurrentUniverse.Stop();
        }

        [Authorize]
        public List<Generation> getHistoryBatch(int startGeneration, int endGeneration)
        {
            return CurrentUniverse.GetHistoryBatch(startGeneration, endGeneration);
        }

        public void watch(string username)
        {
            Groups.Add(Context.ConnectionId, username);
        }
    }
}