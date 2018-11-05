using CarDealerShip.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain.Interfaces
{
    public interface ISaleRepository
    {
        IEnumerable<Sale> All();
        Sale FindById(int id);
        Sale Save(Sale sale);
        bool Delete(int id);
        IEnumerable<SalesReport> GetSalesReport(SalesReportSearchParameters parameters);
    }
}
