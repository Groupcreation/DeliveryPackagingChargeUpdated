using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
                return BadRequest("Invalid count");
            }
            var CSVFile = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true
            };
            try
            {
                int Charge = 0;
                var CSV = new CsvContext();
                var Charges = from values in CSV.Read<Item>(@"./Items.csv", CSVFile)
                              where (values.ItemType.Trim().ToUpper() == item.ToUpper())
                              select new
                              {
                                  DeliveryCharge = values.Delivery,
                                  PackagingCharge = values.Packaging
                              };
                var Fee = Charges.Select(charge => charge.DeliveryCharge + charge.PackagingCharge).ToList();
                foreach (int value in Fee)
                {
                    Charge += value;
                }

                return Charge * count;
            }
            catch (Exception exception)
            {
                return BadRequest("Exception occurred "+ exception);
            }
        }
    }

}
