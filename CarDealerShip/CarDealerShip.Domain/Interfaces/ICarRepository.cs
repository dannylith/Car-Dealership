using CarDealerShip.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain.Interfaces
{
    public interface ICarRepository
    {
        IEnumerable<Car> All();
        Car FindById(int id);
        Car Save(Car car);
        bool Delete(int id);
        IEnumerable<CarDetails> Search(CarSearchParameters parameters);
        IEnumerable<CarDetails> GetFeatured();
        IEnumerable<InventoryReport> GetNewInventoryReport();
        IEnumerable<InventoryReport> GetUsedInventoryReport();
    }
}
