using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{
    public class ItemDTO
    {
        public Int64 LevelId { get; set; }
        public string LevelName { get; set; }
        public decimal RateBdt { get; set; }
        public decimal RateUsd { get; set; }
    }
}