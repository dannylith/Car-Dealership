using CarDealerShip.Domain;
using CarDealerShip.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarDealerShip.Controllers
{
    public class CarAPIController : ApiController
    {
        private CarService carService;
        public CarAPIController(CarService carService)
        {
            this.carService = carService;
        }

        [Route("search")]
        [HttpGet]
        public IHttpActionResult Search(string quickSearch, decimal? minPrice, decimal? maxPrice, int? minYear, int? maxYear, string carType)
        {
            var parameters = new CarSearchParameters {
                QuickSearch = quickSearch,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinYear = minYear, 
                MaxYear = maxYear,
                CarType = carType
            };

            return Ok(carService.Search(parameters));
        }

        [Route("search/make")]
        [HttpGet]
        public IHttpActionResult SearchAllModelsByMakeName(string makeName)
        {            
            return Ok(carService.GetModelsByMakeName(makeName));
        }

        [Route("search/sales")]
        [HttpPost]
        public IHttpActionResult SearchSalesReport(SalesReportSearchParameters parameters)
        {
            return Ok(carService.GetSalesReports(parameters));
        }
    }
}
