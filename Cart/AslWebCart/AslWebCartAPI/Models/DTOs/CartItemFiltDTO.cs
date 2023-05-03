using System;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{
    public class CartItemFiltDTO
    {
        public Int64 Id { get; set; }
        public Int64 ItemId { get; set; }
        public string ItemName { get; set; }
        public Int64 FilterGroupid { get; set; }
        public string FilterGroupName { get; set; }
        public Int64 FilterId { get; set; }
        public string FilterName { get; set; }
    }
}