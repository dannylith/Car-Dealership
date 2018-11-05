using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain
{
    public class Sale
    {
        public int SaleId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbrv { get; set; }
        public int Zipcode { get; set; }
        public decimal PurchasePrice { get; set; }
        public string PurchaseType { get; set; }
        public string SalesUserId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
