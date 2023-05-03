using System;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{
 
    public class CartFilterDTO
    {

        public Int64 Id { get; set; }
        public Int64 FilterGroupId { get; set; }
        public Int64 FilterId { get; set; }
        public string FilterName { get; set; }
        public Int64 FilterNumericId { get; set; }
        public string FilterShortId { get; set; }

    }
}