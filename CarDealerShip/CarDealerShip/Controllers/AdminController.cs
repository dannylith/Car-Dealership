using CarDealerShip.Domain;
using CarDealerShip.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarDealerShip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private CarService carService;
        public AdminController(CarService carService)
        {
            this.carService = carService;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        // GET: Admin
        public ActionResult Vehicles()
        {
            return View();
        }

        public ActionResult AddVehicle()
        {
            var car = new AdminAddCarVM();

            var AllMake = carService.GetAllMake();


            List<SelectListItem> listAllMake = new List<SelectListItem>();
            foreach (var make in AllMake)
            {
                listAllMake.Add(new SelectListItem
                {
                    Text = make.MakeName,
                    Value = make.MakeName
                });
            }
            ViewBag.AllMake = listAllMake;

            var AllModelsByMake = carService.GetModelsByMakeName(listAllMake.First().Text);

            List<SelectListItem> listAllModels = new List<SelectListItem>();
            foreach (var model in AllModelsByMake)
            {
                listAllModels.Add(new SelectListItem
                {
                    Text = model.ModelName,
                    Value = model.ModelId.ToString()
                });
            }
            ViewBag.AllModelsByMake = listAllModels;
            return View(car);
        }

        [HttpPost]
        public ActionResult AddVehicle(AdminAddCarVM model1)
        {
            var AllMake = carService.GetAllMake();


            List<SelectListItem> listAllMake = new List<SelectListItem>();
            foreach (var make in AllMake)
            {
                listAllMake.Add(new SelectListItem
                {
                    Text = make.MakeName,
                    Value = make.MakeName
                });
            }
            ViewBag.AllMake = listAllMake;

            var AllModelsByMake = carService.GetModelsByMakeName(listAllMake.First().Text);

            List<SelectListItem> listAllModels = new List<SelectListItem>();
            foreach (var model in AllModelsByMake)
            {
                listAllModels.Add(new SelectListItem
                {
                    Text = model.ModelName,
                    Value = model.ModelId.ToString()
                });
            }
            ViewBag.AllModelsByMake = listAllModels;
            if (ModelState.IsValid)
            {

                if (model1.ImageUpload != null && model1.ImageUpload.ContentLength > 0)
                {
                    var savepath = Server.MapPath("~/Images");

                    string fileName = Path.GetFileNameWithoutExtension(model1.ImageUpload.FileName);
                    string extension = Path.GetExtension(model1.ImageUpload.FileName);

                    var filePath = Path.Combine(savepath, fileName + extension);

                    int counter = 1;
                    while (System.IO.File.Exists(filePath))
                    {
                        filePath = Path.Combine(savepath, fileName + counter.ToString() + extension);
                        counter++;
                    }

                    model1.ImageUpload.SaveAs(filePath);
                    model1.Car.PictureUrl = Path.GetFileName(filePath);
                }

                model1.Car = carService.SaveCar(model1.Car);

                return RedirectToAction("EditVehicle", new { id = model1.Car.CarId });

            }
            else
            {
                return View(model1);
            }


        }

        public ActionResult EditVehicle(int id)
        {
            var model = new AdminEditCarVM();
            var car = carService.FindCarById(id);
            model.Car = car;
            var currentCarMakeName = carService.GetCarDetailsById(id).MakeName;
            var AllMake = carService.GetAllMake();

            List<SelectListItem> listAllMake = new List<SelectListItem>();
            foreach (var make in AllMake)
            {
                listAllMake.Add(new SelectListItem
                {
                    Text = make.MakeName,
                    Value = make.MakeName,
                    Selected = (currentCarMakeName == make.MakeName)

                });
            }

            ViewBag.AllMake = listAllMake;

            var AllModelsByMake = carService.GetModelsByMakeName(listAllMake.Where(m=>m.Selected == true).First().Text);

            List<SelectListItem> listAllModels = new List<SelectListItem>();
            foreach (var m in AllModelsByMake)
            {
                listAllModels.Add(new SelectListItem
                {
                    Text = m.ModelName,
                    Value = m.ModelId.ToString(),
                    Selected = (m.ModelId == car.ModelId)
                });
            }
            ViewBag.AllModelsByMake = listAllModels;


            return View(model);
        }

        [HttpPost]
        public ActionResult EditVehicle(AdminEditCarVM model)
        {
            var currentCarMakeName = carService.GetCarDetailsById(model.Car.CarId).MakeName;
            var AllMake = carService.GetAllMake();

            List<SelectListItem> listAllMake = new List<SelectListItem>();
            foreach (var make in AllMake)
            {
                listAllMake.Add(new SelectListItem
                {
                    Text = make.MakeName,
                    Value = make.MakeName,
                    Selected = (currentCarMakeName == make.MakeName)

                });
            }

            ViewBag.AllMake = listAllMake;

            var AllModelsByMake = carService.GetModelsByMakeName(listAllMake.Where(m => m.Selected == true).First().Text);

            List<SelectListItem> listAllModels = new List<SelectListItem>();
            foreach (var m in AllModelsByMake)
            {
                listAllModels.Add(new SelectListItem
                {
                    Text = m.ModelName,
                    Value = m.ModelId.ToString(),
                    Selected = (m.ModelId == model.Car.ModelId)
                });
            }
            ViewBag.AllModelsByMake = listAllModels;

            if (ModelState.IsValid)
            {

                var oldCar = carService.GetCarDetailsById(model.Car.CarId); //repo.GetById(model.Listing.ListingId);

                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    var savepath = Server.MapPath("~/Images");

                    string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                    string extension = Path.GetExtension(model.ImageUpload.FileName);

                    var filePath = Path.Combine(savepath, fileName + extension);

                    int counter = 1;
                    while (System.IO.File.Exists(filePath))
                    {
                        filePath = Path.Combine(savepath, fileName + counter.ToString() + extension);
                        counter++;
                    }

                    model.ImageUpload.SaveAs(filePath);
                    model.Car.PictureUrl = Path.GetFileName(filePath);

                    // delete old file
                    var oldPath = Path.Combine(savepath, oldCar.PictureUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                else
                {
                    // they did not replace the old file, so keep the old file name
                    model.Car.PictureUrl = oldCar.PictureUrl;
                }

                //repo.Update(model.Listing);
                carService.SaveCar(model.Car);
                return RedirectToAction("Vehicles");

            }
            else
            {

                return View(model);
            }
        }

        public ActionResult DeleteCar(int id)
        {
            carService.DeleteCar(id);
            return RedirectToAction("Vehicles");
        }

        public ActionResult Users()
        {
            var context = new ApplicationDbContext();
            var allUsers = context.Users.ToList();

            return View(allUsers);
        }

        public ActionResult AddUser()
        {
            var addUserVM = new AdminAddUserVM();
            return View(addUserVM);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(AdminAddUserVM model)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                var result = await UserManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    UserManager.AddToRole(UserManager.FindByEmail(user.Email).Id, model.Role);
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                    return RedirectToAction("Users", "Admin");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ActionResult EditUser(string id)
        {
            var user = UserManager.FindById(id);
            var model = new AdminEditUserVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Roles.First().RoleId == "1" ? "Admin" : "Salesman",
                Id = user.Id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(AdminEditUserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(model.Id);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                var roles = await UserManager.GetRolesAsync(user.Id);
                await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                UserManager.AddToRole(user.Id, model.Role);
                if (!string.IsNullOrEmpty(model.Password))
                {
                    string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    IdentityResult passwordChangeResult = await UserManager.ResetPasswordAsync(user.Id, resetToken, model.Password);
                }

                var result = UserManager.Update(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users", "Admin");
                }
                AddErrors(result);
            }
            return View(model);
        }

        public ActionResult AddMake()
        {
            var model = new AdminAddMakeVM();
            model.Make = new Make();
            model.Makes = carService.GetAllMake();
            foreach (var m in model.Makes)
            {
                m.AdminUserId = UserManager.FindById(m.AdminUserId).UserName;
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult AddMake(Make make)
        {
            make.AdminUserId = UserManager.FindByName(make.AdminUserId).Id;
            carService.SaveMake(make);

            return RedirectToAction("AddMake");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddModel()
        {
            var model = new AdminAddModelVM();
            model.Model = new Model();
            model.Models = carService.GetAllModel();
            model.Makes = carService.GetAllMake();

            foreach (var m in model.Models)
            {
                m.AdminUserId = UserManager.FindById(m.AdminUserId).UserName;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddModel(Model model)
        {
            model.AdminUserId = UserManager.FindByName(model.AdminUserId).Id;
            carService.SaveModel(model);

            return RedirectToAction("AddModel");
        }

        public ActionResult AddOrRemoveSpecial()
        {
            var model = new AdminAddOrRemoveSpecialVM();
            model.Special = new Special();
            model.Specials = carService.GetAllSpecials();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddOrRemoveSpecial(Special special)
        {
            if (ModelState.IsValid)
            {
                carService.SaveSpecial(special);
                return RedirectToAction("AddOrRemoveSpecial");
            }
            var model = new AdminAddOrRemoveSpecialVM();
            model.Special = special;
            model.Specials = carService.GetAllSpecials();

            return View(model);
            
            
        }

        public ActionResult RemoveSpecial(int id)
        {
            carService.DeleteSpecial(id);
            return RedirectToAction("AddOrRemoveSpecial");
        }

        public ActionResult Report()
        {
            return View();
        }

        public ActionResult SalesReport()
        {
            var allUsers = UserManager.Users.ToList();
            return View(allUsers);
        }

        public ActionResult InventoryReport()
        {
            var model = new AdminInventoryReportVM();
            model.NewInventoryReports = carService.GetNewInventoryReport();
            model.UsedInventoryReports = carService.GetUsedInventoryReport();
            return View(model);
        }
    }
}