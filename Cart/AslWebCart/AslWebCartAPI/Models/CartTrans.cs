using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models
{

    [Table("KART_TRANS")]
    public class CartTrans
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public string TRANSTP { get; set; }
        public DateTime TRANSDT { get; set; }

        public string TRANSNO { get; set; }
        public Int64 MEMBERID { get; set; }
        public Int64 ITEMID { get; set; }
        public decimal QTY { get; set; }
        public decimal RATE { get; set; }
        public decimal AMOUNT { get; set; }
    }
}