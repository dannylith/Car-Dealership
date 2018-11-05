using CarDealerShip.Domain;
using CarDealerShip.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealerShip.Controllers
{
    [Authorize(Roles ="Salesman")]
    public class SalesController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private CarService carService;
        public SalesController(CarService carService)
        {
            this.carService = carService;
        }
        [Authorize]
        // GET: Sales
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Purchase(int id)
        {
            SalesPurchaseVM model = new SalesPurchaseVM();
            model.CarDetails = carService.GetCarDetailsById(id);
            model.Sale = new Sale();
            return View(model);
        }

        [HttpPost]
        public ActionResult Purchase(SalesPurchaseVM model)
        {
            
            model.Sale.SalesUserId = UserManager.FindByName(User.Identity.Name).Id;
            if (ModelState.IsValid)
            {
                carService.SavePurchase(model.Sale, model.CarDetails.CarId);
                return RedirectToAction("Index");
            }

            return View(model);
            
        }
    }
}