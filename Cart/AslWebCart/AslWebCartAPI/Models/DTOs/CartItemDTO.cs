using System;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{
    public class CartItemDTO
    {
        public Int64 Id { get; set; }
        public Int64 ItemId { get; set; }
        public string ItemName { get; set; }
        public string StockType { get; set; }
        public decimal RateBdt { get; set; }
        public decimal RateUsd { get; set; }


        public CartCategoryDTO CartCategoryDTO { get; set; }
        public CartFilterDTO CartFilterDTO { get; set; }
        public CartFilterMstDTO CartFilterMstDTO { get; set; }
        public CartLevelMstDTO CartLevelMstDTO { get; set; }
        public CartLevelDTO CartLevelDTO { get; set; }
        public CartItemFiltDTO CartItemFiltDTO { get; set; }



    }
}