using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{

    public class CartFilterMstDTO
    {
        public Int64 Id { get; set; }
        public Int64 FilterGroupid { get; set; }
        public string FilterGroupName { get; set; }


        public Int64 FilterId { get; set; }
        public string FilterName { get; set; }
        public Int64 FilterNumericId { get; set; }
        public string FilterShortId { get; set; }

        public IEnumerable<CartFilterDTO> CartFilterDTO { get; set; }

    }
}