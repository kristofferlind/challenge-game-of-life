﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoL.MVC.Models
{
    public class HubUser
    {
        public string Username { get; set; }
        public Universe Universe { get; set; }
        public string ConnectionId { get; set; }
    }
}