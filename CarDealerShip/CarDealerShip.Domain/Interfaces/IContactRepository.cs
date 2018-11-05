using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain.Interfaces
{
    public interface IContactRepository
    {
        IEnumerable<Contact> All();
        Contact FindById(int id);
        Contact Save(Contact contact);
        bool Delete(int id);
        Contact FindByCarId(int id);
    }
}
