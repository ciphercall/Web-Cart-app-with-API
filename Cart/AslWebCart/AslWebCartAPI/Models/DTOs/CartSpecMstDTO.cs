using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{
    public class CartSpecMstDTO
    {
        public Int64 Id { get; set; }
        public Int64 ItemId { get; set; }
        public string ItemName { get; set; }
        public Int64 SpecificationSL { get; set; }
        public string SpecificationName { get; set; }
        public Int64 ViewSL { get; set; }
        public Int64 FeatureSL { get; set; }
        public string FeatureType { get; set; }
        public string Feature { get; set; }

        public IEnumerable<CartSpecificationDTO> Spec { get; set; }
    }
}