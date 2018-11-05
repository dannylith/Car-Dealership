using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain
{
    public class Special
    {
        public int SpecialId { get; set; }


        [Required]
        public string SpecialTitle { get; set; }
        [Required]
        public string SpecialDesc { get; set; }
    }
}
