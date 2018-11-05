using CarDealerShip.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealerShip.Models
{
    public class AdminInventoryReportVM
    {
        public IEnumerable<InventoryReport> NewInventoryReports { get; set; }
        public IEnumerable<InventoryReport> UsedInventoryReports { get; set; }
    }
}