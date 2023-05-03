using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.DTOs
{
    public class CartMemberDTO
    {
        public Int64 Id { get; set; }
        public Int64 MemberId { get; set; }
        public string MemberNm { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Logby { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string UserPw { get; set; }
    }
}