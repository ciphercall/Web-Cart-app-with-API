using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models
{
    [Table("KART_ITEMFILT")]
    public class CartItemFilt
    {


        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }


        [Key, Column(Order = 1)]
        public Int64 ITEMID { get; set; }
        public Int64? FILTERGID { get; set; }

      
        public Int64 FILTERID { get; set; }
    }
}