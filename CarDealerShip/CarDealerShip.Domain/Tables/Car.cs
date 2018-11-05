using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain
{
    public class Car
    {
        public int CarId { get; set; }
        public int ModelId { get; set; }
        public int? SaleId { get; set; }
        public int BuildYear { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal MSRP { get; set; }
        public string BodyStyle { get; set; }
        public string Color { get; set; }
        public string Interior { get; set; }
        public decimal Mileage { get; set; }
        public string Vin { get; set; }
        public string CarDescription { get; set; }
        public string PictureUrl { get; set; }
        public string CarType { get; set; }
        public string Transmission { get; set; }
        public bool IsFeatured { get; set; }
    }
}
