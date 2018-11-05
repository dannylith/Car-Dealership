using CarDealerShip.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace CarDealerShip.Models
{
    public class AdminAddCarVM : IValidatableObject
    {
        public Car Car { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (!(Car.ModelId >= 0))
            {
                errors.Add(new ValidationResult("Model is required"));
            }

            if (!(Car.BuildYear >= 0))
            {
                errors.Add(new ValidationResult("Build Year is required"));
            }
            else if (Car.BuildYear < 2000 || Car.BuildYear > DateTime.Now.AddYears(1).Year)
            {
                errors.Add(new ValidationResult("Build Year has to be between 2000 and " + DateTime.Now.AddYears(1).Year));
            }


            if (!(Car.MSRP >= 0))
            {
                errors.Add(new ValidationResult("MSRP is required"));
            }

            if (!(Car.SalePrice >= 0))
            {
                errors.Add(new ValidationResult("Sale Price is required"));
            }

            if (Car.SalePrice > Car.MSRP)
            {
                errors.Add(new ValidationResult("Sale Price cannot be greater than MSRP"));
            }

            if (string.IsNullOrEmpty(Car.BodyStyle))
            {
                errors.Add(new ValidationResult("Body Style is required"));
            }

            if (string.IsNullOrEmpty(Car.Color))
            {
                errors.Add(new ValidationResult("Color is required"));
            }

            if (string.IsNullOrEmpty(Car.Interior))
            {
                errors.Add(new ValidationResult("Interior Color is required"));
            }

            if (!(Car.Mileage >= 0))
            {
                errors.Add(new ValidationResult("Mileage is required and needs to be 0 or greater"));
            } else if (Car.Mileage >1000 && Car.CarType == "New")
            {
                errors.Add(new ValidationResult("Mileage greater than 1000 cannot have Car Type 'New'."));
            }
            else if (Car.Mileage < 1000 && Car.CarType == "Used" && Car.Mileage >= 0)
            {
                errors.Add(new ValidationResult("Mileage between 0-1000 cannot have Car Type 'Used'."));
            }

            if (string.IsNullOrEmpty(Car.Vin))
            {
                errors.Add(new ValidationResult("VIN is required"));
            }

            if (ImageUpload != null && ImageUpload.ContentLength > 0)
            {
                var extensions = new string[] { ".jpg", ".png", ".jpeg" };

                var extension = Path.GetExtension(ImageUpload.FileName);

                if (!extensions.Contains(extension))
                {
                    errors.Add(new ValidationResult("Image file must be a jpg, png, gif, or jpeg."));
                }
            }
            else
            {
                errors.Add(new ValidationResult("Image file is required"));
            }

            if (string.IsNullOrEmpty(Car.CarType))
            {
                errors.Add(new ValidationResult("Car Type is required"));
            }

            if (string.IsNullOrEmpty(Car.Transmission))
            {
                errors.Add(new ValidationResult("Transmission is required"));
            }

            if (Car.Vin.Length > 17)
            {
                errors.Add(new ValidationResult("VIN must be 17 or less in characters"));
            }



            return errors;
        }
    }
}