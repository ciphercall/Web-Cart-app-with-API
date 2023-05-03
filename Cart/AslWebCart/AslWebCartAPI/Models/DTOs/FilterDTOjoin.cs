using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{
    public class FilterDTOjoin
    {
        // public FilterDTOjoin()
        //{

        //    this.CartFilterMst = new CartFilterMstDTO();
        //    this.CartFilter = new CartFilterDTO();
        //}
        //public CartFilterDTO CartFilter { get; set; }
        //public CartFilterMstDTO CartFilterMst { get; set; }

        public Int64 FilterGroupid { get; set; }
        public string FilterGroupName { get; set; }
        public Int64 FilterId { get; set; }
        public string FilterName { get; set; }
        public Int64 FilterNumericId { get; set; }
        public string FilterShortId { get; set; }
    }
}