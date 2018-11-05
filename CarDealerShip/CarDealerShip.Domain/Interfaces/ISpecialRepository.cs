using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain.Interfaces
{
    public interface ISpecialRepository
    {
        IEnumerable<Special> All();
        Special FindById(int id);
        Special Save(Special special);
        bool Delete(int id);
    }
}
