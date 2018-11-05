using CarDealerShip.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealerShip.Models
{
    public class AdminAddModelVM
    {
        public Model Model { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<Make> Makes { get; set; }
    }
}