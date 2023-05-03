using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models
{

    [Table("KART_LEVEL")]
    public class CartLevel
    {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        public string LEVELGTP { get; set; }

        [Key, Column(Order = 1)]
        public Int64 LEVELGID { get; set; }
        public string LEVELTP { get; set; }

       
        public Int64 LEVELID { get; set; }
        public string LEVELNM { get; set; }

    }
}