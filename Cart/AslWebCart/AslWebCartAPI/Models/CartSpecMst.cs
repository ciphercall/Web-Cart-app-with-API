using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models
{
    [Table("KART_SPECMST")]
    public class CartSpecMst
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public Int64 ITEMID { get; set; }

        [Key, Column(Order = 2)]
        public Int64 SPECSL { get; set; }

        public string SPECNM { get; set; }
        public Int64 VIEWSL { get; set; }


    }
}