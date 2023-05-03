using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models
{
    [Table("KART_TRANSMST")]
    public class CartTransMst
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public string TRANSTP { get; set; }
        public DateTime TRANSDT { get; set; }
        public string TRANSNO { get; set; }
        public Int64 MEMBERID { get; set; }
        public string SHIPDIST { get; set; }
        public string SHIPADDR { get; set; }
        public DateTime SHIPDT { get; set; }
        public decimal CONTACTNO { get; set; }
        public string TRANSMODE { get; set; }
        public decimal TOTAMT { get; set; }
        public decimal DISAMT { get; set; }
        public decimal SCCOST { get; set; }
        public decimal TOTNET { get; set; }
        public string REMARKS { get; set; }
        public string ORDERSTATUS { get; set; }
    }
}