using CarDealerShip.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealerShip.Models
{
    public class AdminAddOrRemoveSpecialVM
    {
        public Special Special { get; set; }
        public IEnumerable<Special> Specials { get; set; }
    }
}