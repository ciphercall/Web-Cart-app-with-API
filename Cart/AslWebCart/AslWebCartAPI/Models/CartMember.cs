using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models
{
    [Table("KART_MEMBER")]
    public class CartMember
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Key, Column(Order = 1)]
        public Int64 MEMBERID { get; set; }
        public string MEMBERNM { get; set; }
        public string EMAILID { get; set; }
        public string MOBILENO { get; set; }
        public string ADDRESS { get; set; }
        public string LOGBY { get; set; }
        public string TOKEN { get; set; }
        public string USERID { get; set; }
        public string USERPW { get; set; }
    }
}