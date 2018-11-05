using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain.Interfaces
{
    public interface IModelRepository
    {

        IEnumerable<Model> All();
        Model FindById(int id);
        Model Save(Model model);
        bool Delete(int id);
    }
}
