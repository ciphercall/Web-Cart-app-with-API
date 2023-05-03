using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models
{
    [Table("KART_ITEM")]
    public class CartItem
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public Int64?ITEMID { get; set; }
        public string ITEMNM { get; set; }
        public string STOCKTP { get; set; }
        public decimal RATEBDT { get; set; }
        public decimal RATEUSD { get; set; }

        public Int64?IMAGEID1 { get; set; }
        public Int64?IMAGEID2 { get; set; }
        public Int64?IMAGEID3 { get; set; }
        public Int64?IMAGEID4 { get; set; }
        public Int64?IMAGEID5 { get; set; }


    }
}