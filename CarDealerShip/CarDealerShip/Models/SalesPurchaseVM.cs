using CarDealerShip.Domain;
using CarDealerShip.Domain.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CarDealerShip.Models
{
    public class SalesPurchaseVM : IValidatableObject
    {
        public CarDetails CarDetails { get; set; }
        public Sale Sale { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var usZipRegEx = new Regex(@"\d{5}$");
            var emailRegEx = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Sale.Email) && string.IsNullOrEmpty(Sale.Phone))
            {
                errors.Add(new ValidationResult("Email or Phone is required"));
            }
            if (string.IsNullOrEmpty(Sale.Street1))
            {
                errors.Add(new ValidationResult("Street 1 is required"));
            }
            if (!usZipRegEx.IsMatch(Sale.Zipcode.ToString()))
            {
                errors.Add(new ValidationResult("Zipcode has to be 5 digits"));
            }
            if (!string.IsNullOrEmpty(Sale.Email))
            {
                if (!emailRegEx.IsMatch(Sale.Email))
                {
                    errors.Add(new ValidationResult("Email is not in correct format"));
                }
            }

            var minPurchasePrice = CarDetails.SalePrice * .95M;
            if (Sale.PurchasePrice < minPurchasePrice)
            {
                errors.Add(new ValidationResult("Purchase price cannot be less than 95% of Car's Sale Price"));
            }
            
            return errors;
        }
    }
}