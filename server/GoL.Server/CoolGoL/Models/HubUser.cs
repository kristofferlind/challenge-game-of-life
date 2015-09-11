using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CoolGoL.Models
{
    public class HubUser
    {
        public string Username { get; set; }
        public Universe Universe { get; set; }
        public string ConnectionId { get; set; }
        public Thread Thread { get; set; }
        public Task CurrentUniverse { get; set; }
    }
}