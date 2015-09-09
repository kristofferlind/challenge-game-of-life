using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoL.MVC.Models
{
    public class Generation
    {
        public List<Cell> Cells { get; set; }
        public int GenerationNumber { get; set; }
    }
}