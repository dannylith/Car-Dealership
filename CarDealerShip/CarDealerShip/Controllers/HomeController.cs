using CarDealerShip.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealerShip.Controllers
{
    public class HomeController : Controller
    {
        private CarService carService;
        public HomeController(CarService carService)
        {
            this.carService = carService;
        }

        public ActionResult Index()
        {
            var listFeatured = carService.GetFeatured();
            return View(listFeatured);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact(int id = 0)
        {
            ViewBag.CarId = id;
            var model = new Contact();
            if (id > 0)
            {
                model.ContactMessage = carService.GetCarDetailsById(id).Vin + "- ";

            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            contact.Email = string.IsNullOrEmpty(contact.Email) ? null : contact.Email;
            contact.CarId = contact.CarId == 0 ? null : contact.CarId;
            carService.SaveContact(contact);
            return RedirectToAction("Index");
        }

        public ActionResult UsedInventory()
        {
            return View();
        }

        public ActionResult NewInventory()
        {
            return View();
        }

        public ActionResult VehicleDetails(int Id)
        {
            var model = carService.GetCarDetailsById(Id);
            return View(model);
        }

        public ActionResult Specials()
        {
            return View(carService.GetAllSpecials());
        }

    }
}