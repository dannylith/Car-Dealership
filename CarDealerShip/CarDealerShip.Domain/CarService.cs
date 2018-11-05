using CarDealerShip.Domain.Interfaces;
using CarDealerShip.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Domain
{
    public class CarService
    {
        private ICarRepository carRepo;
        private IContactRepository contactRepo;
        private IMakeRepository makeRepo;
        private IModelRepository modelRepo;
        private ISaleRepository saleRepo;
        private ISpecialRepository specialRepo;
        public CarService(ICarRepository carRepo, IContactRepository contactRepo, IMakeRepository makeRepo, IModelRepository modelRepo, ISaleRepository saleRepo, ISpecialRepository specialRepo)
        {
            this.carRepo = carRepo;
            this.contactRepo = contactRepo;
            this.makeRepo = makeRepo;
            this.modelRepo = modelRepo;
            this.saleRepo = saleRepo;
            this.specialRepo = specialRepo;
        }

        public List<CarDetails> GetFeatured()
        {
            return carRepo.GetFeatured().ToList();
        }

        public List<CarDetails> Search(CarSearchParameters parameters)
        {
            return carRepo.Search(parameters).ToList();
        }

        public CarDetails GetCarDetailsById(int id)
        {
            var car = carRepo.FindById(id);
            var model = modelRepo.FindById(car.ModelId);
            var make = makeRepo.FindById(model.MakeId);

            CarDetails carDetails = new CarDetails {
                CarId= car.CarId,
                SaleId = car.SaleId,
                BuildYear = car.BuildYear,
                MakeName = make.MakeName,
                ModelName = model.ModelName,
                BodyStyle = car.BodyStyle,
                Transmission = car.Transmission,
                Color = car.Color,
                Interior = car.Interior,
                Mileage = car.Mileage,
                Vin = car.Vin,
                SalePrice = car.SalePrice,
                MSRP = car.MSRP,
                CarDescription = car.CarDescription,
                PictureUrl = car.PictureUrl
            };

            return carDetails;
        }

        public List<Special> GetAllSpecials()
        {
            return specialRepo.All().ToList();
        }

        public void SaveContact(Contact contact)
        {
            contactRepo.Save(contact);
        }

        public void SavePurchase(Sale sale, int carId)
        {
            sale = saleRepo.Save(sale);

            var car = carRepo.FindById(carId);
            car.SaleId = sale.SaleId;
            carRepo.Save(car);
        }

        public List<Model> GetModelsByMakeName(string makeName)
        {
            var make = makeRepo.All().Where(m => m.MakeName == makeName).First();
            var allModelbyMakeId = modelRepo.All().Where(m => m.MakeId == make.MakeId);

            return allModelbyMakeId.ToList();
        }
        public List<Make> GetAllMake()
        {
            return makeRepo.All().ToList();
        }

        public Car FindCarById(int id)
        {
            return carRepo.FindById(id);
        }

        public void SaveMake(Make make)
        {
            makeRepo.Save(make);
        }

        public Car SaveCar(Car car)
        {
            return carRepo.Save(car);
        }

        public List<Model> GetAllModel()
        {
            return modelRepo.All().ToList();
        }

        public void SaveModel(Model model)
        {
            modelRepo.Save(model);
        }

        public void SaveSpecial(Special special)
        {
            specialRepo.Save(special);
        }

        public bool DeleteSpecial(int id)
        {
            return specialRepo.Delete(id);
        }

        public void DeleteCar(int id)
        {
            var contact = contactRepo.FindByCarId(id);
            if (contact != null)
            {
                contactRepo.Delete(contact.ContactId);
            }
            carRepo.Delete(id);
        }

        public List<SalesReport> GetSalesReports(SalesReportSearchParameters parameters)
        {
            return saleRepo.GetSalesReport(parameters).ToList();
        }

        public List<InventoryReport> GetNewInventoryReport()
        {
            return carRepo.GetNewInventoryReport().ToList();
        }

        public List<InventoryReport> GetUsedInventoryReport()
        {
            return carRepo.GetUsedInventoryReport().ToList();
        }
    }
}
