using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.Html;
using CoolGoL.Models;

namespace CoolGoL.App_Infrastructure
{
    public static class UserList
    {
        public static List<HubUser> Users = new List<HubUser>();
    }

    public class GameHub : Hub
    {
        private HubUser _user;

        public HubUser User
        {
            get
            {
                if (_user != null)
                {
                    return _user;
                } 
                var user = Context.User;

                if (user.Identity.IsAuthenticated)
                {
                    var universe = new Universe(null, user.Identity.Name);

                    var hubUser = new HubUser
                    {
                        Username = user.Identity.Name,
                        Universe = universe,
                        ConnectionId = Context.ConnectionId,
                    };

                    //UserList.Users.Add(hubUser);

                    _user = hubUser;

                    return hubUser;
                }
                else
                {
                    return null;
                }
            }
        }

        public override Task OnConnected()
        {
            var user = User;

            if (user != null)
            {
                UserList.Users.Add(user);
                Groups.Add(Context.ConnectionId, user.Username);

                UpdateUserlist();
            }
            else
            {
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
            //var user = UserList.Users.Find(u => u.Username == User.Username);
            //UserList.Users.Remove(user);
            UserList.Users.Remove(User);
            UpdateUserlist();
            return base.OnDisconnected(stopCalled);
        }

        //[Authorize]
        public async Task startSimulation(List<Cell> cells, int generation)
        {
            var user = UserList.Users.Find(u => u.Username == User.Username);
            await user.Universe.Start(cells, generation);
            UpdateUserlist();
        }

        //[Authorize]
        public void pauseSimulation()
        {
            var user = UserList.Users.Find(u => u.Username == User.Username);
            user.Universe.Stop();
            UpdateUserlist();
        }

        //[Authorize]
        public List<Generation> getHistoryBatch(int startGeneration, int endGeneration)
        {
            return User.Universe.GetHistoryBatch(startGeneration, endGeneration);
        }

        public void watch(string username)
        {
            Groups.Add(Context.ConnectionId, username);
        }

        public void UpdateUserlist()
        {
            var simpleUsers = from u in UserList.Users
                              select new { u.Username, u.Universe.Running };
            GlobalHost.ConnectionManager.GetHubContext<GameHub>().Clients.All.UpdateUserlist(simpleUsers);
        }

        public Dictionary<string,List<Cell>> GetSeeds()
        {
            var seeds = new Seeds();
            return seeds.AllSeeds;
        }
    }
}