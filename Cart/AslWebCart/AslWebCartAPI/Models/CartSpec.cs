using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models
{
    [Table("KART_SPEC")]
    public class CartSpec
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public Int64 ITEMID { get; set; }

        [Key, Column(Order = 2)]
        public Int64 SPECSL { get; set; }

        [Key, Column(Order = 3)]
        public Int64 FEATSL { get; set; }
        public string FEATTP { get; set; }
        public string FEATURE { get; set; }




    }
}