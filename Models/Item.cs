using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LINQtoCSV;

namespace PackagingAndDelivery.Models
{
    public class Item
    {
        [CsvColumn(FieldIndex = 1)]
        public string ItemType { get; set; }
        [CsvColumn(FieldIndex = 2)]
        public int Packaging { get; set; }
        [CsvColumn(FieldIndex = 3)]
        public int Delivery { get; set; }
        [CsvColumn(FieldIndex = 4)]
        public int Sheath { get; set; }
    }
}
