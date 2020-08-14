using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PackagingAndDelivery.Models;
using LINQtoCSV;

namespace PackagingAndDelivery.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GetPackagingDeliveryChargeController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(GetPackagingDeliveryChargeController));
        [HttpGet]
        [ActionName("GetPackagingDeliveryCharge")]

        public dynamic GetPackagingDeliveryCharge(string item, int count)
        {
            _log4net.Info("GetPackagingDeliveryCharge() called");
            if (count <= 0)
            {
                //Checking if count is invalid
                return BadRequest("Invalid count");
                //return -1;
            }
            var CSVFile = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true
            };
            try
            {
                // Reading the CSV file for fetching the static values
                int Charge = 0;
                var CSV = new CsvContext();
                var Charges = from values in CSV.Read<Item>(@"./Items.csv", CSVFile)
                              where (values.ItemType.Trim().ToUpper() == item.ToUpper())
                              select new
                              {
                                  DeliveryCharge = values.Delivery,
                                  PackagingCharge = values.Packaging,
                                  SheathCharge = values.Sheath
                              };
                var Fee = Charges.Select(charge => charge.DeliveryCharge + charge.PackagingCharge).ToList();
                var Sheath = Charges.Select(charge => charge.SheathCharge).ToList();
                foreach (int value in Fee)
                {
                    Charge += value*count;
                }
                foreach (int value in Sheath)
                {
                    Charge += value;
                }
                return Charge;
            }
            catch (Exception exception)
            {
                //Handling the exception
                _log4net.Error("Exception occured: "+exception);
                return BadRequest("Exception occurred "+ exception);
            }
        }
    }

}
