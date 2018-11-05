using CarDealerShip.Data;
using CarDealerShip.Domain.Interfaces;
using System.Web.Http;
using Unity;
using Unity.WebApi;
using Unity.Mvc5;
using System.Web.Mvc;
using CarDealerShip.Controllers;
using Unity.Injection;

namespace CarDealerShip
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ICarRepository, CarRepository>();
            container.RegisterType<IContactRepository, ContactRepository>();
            container.RegisterType<IMakeRepository, MakeRepository>();
            container.RegisterType<IModelRepository, ModelRepository>();
            container.RegisterType<ISaleRepository, SaleRepository>();
            container.RegisterType<ISpecialRepository, SpecialRepository>();

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());


            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}