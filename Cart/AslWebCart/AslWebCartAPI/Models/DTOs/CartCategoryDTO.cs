using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{

    public class CartCategoryDTO
    {
        public Int64 Id { get; set; }
        public Int64 CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string LevelGroup { get; set; }

        public IEnumerable<CartLevelDTO> CartLevelDTO { get; set; }

    }
}