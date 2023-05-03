using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{
    public class CartTransDTO
    {
        public Int64 ID { get; set; }
        //public string TRANSTP { get; set; }
        //public DateTime TRANSDT { get; set; }
        //public string TRANSNO { get; set; }
        //public Int64 MEMBERID { get; set; }
        public Int64 ITEMID { get; set; }
        public string ITEMNM { get; set; }
        public decimal QTY { get; set; }
        public decimal RATE { get; set; }
        public decimal AMOUNT { get; set; }


    }
}