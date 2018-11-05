using CarDealerShip.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealerShip.Models
{
    public class AdminAddMakeVM
    {
        public Make Make { get; set; }
        public IEnumerable<Make> Makes { get; set; }
    }
}