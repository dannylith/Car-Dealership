using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain.Interfaces
{
    public interface IMakeRepository
    {
        IEnumerable<Make> All();
        Make FindById(int id);
        Make Save(Make make);
        bool Delete(int id);
    }
}
