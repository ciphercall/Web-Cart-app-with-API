using System;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{
    public class CartLevelDTO
    {
        public Int64 Id { get; set; }
        public string LevelGroupType { get; set; }
        public Int64 LevelGroupId { get; set; }
        public string LevelType { get; set; }
        public Int64 LevelId { get; set; }
        public string LevelName { get; set; }

    }
}