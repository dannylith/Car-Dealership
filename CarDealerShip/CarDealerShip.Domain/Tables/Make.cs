using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain
{
    public class Make
    {
        public int MakeId { get; set; }
        public string MakeName { get; set; }
        public DateTime DateAdded { get; set; }
        public string AdminUserId { get; set; }
    }
}
