using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AslWebCartAPI.Controllers
{
    public class ApiCartMemberController : ApiController
    {
        private AslWebCartContext db = new AslWebCartContext();

        [Route("api/main/SignUp")]
        [HttpPost]
        //public IEnumerable<CartMemberDTO> PostMember(JObject jsonData)
        public HttpResponseMessage PostMember(JObject jsonData)
        {

            //  string jData = "[{\"logby\":ci@mh.cm,\"userpw\":app,\"emailid\":sdf}]";

            string jData = jsonData.GetValue("jsonData").Value<string>();
            //Int64 levelgid = Convert.ToInt64(memberid);

            var jArray = JArray.Parse(jData);
            int length = jArray.Count;

            string logby = (jArray[0]["logby"]).ToString();
            string userpw = (jArray[0]["userpw"]).ToString();
            string emailid = (jArray[0]["emailid"]).ToString();

            Int64 memberid = 0;
            //Int64 maxMemberId = 0;


            var res = (from n in db.KART_MEMBER where n.EMAILID == emailid && n.USERID == emailid select n).ToList();

            if (res.Count == 0)
            {
                var checkMaxMemberId = (from n in db.KART_MEMBER select (int?)n.MEMBERID).Max() ?? 0;

                if (checkMaxMemberId == 0 || checkMaxMemberId == null)
                {
                    memberid = 50000001;
                }
                else memberid = checkMaxMemberId + 1;

                CartMember mem = new CartMember
                {
                    MEMBERID = memberid,
                    LOGBY = logby,
                    USERPW = userpw,
                    EMAILID = emailid,
                    USERID = emailid
                };
                db.KART_MEMBER.Add(mem);
                db.SaveChanges();

            }

            IEnumerable<CartMemberDTO> member = from t in db.KART_MEMBER
                                                where t.USERID == emailid && t.USERPW == userpw
                                                select new CartMemberDTO
                                                {
                                                    MemberId = t.MEMBERID,
                                                    MemberNm = t.MEMBERNM,
                                                    EmailId = t.EMAILID,
                                                    MobileNo = t.MOBILENO,
                                                    Address = t.ADDRESS,
                                                    Logby = t.LOGBY,
                                                    UserId = t.USERID,
                                                    UserPw = t.USERPW
                                                };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, member);
            return response;
            // return member;

        }

        [Route("api/main/SignIn")]
        [HttpGet]
        public IEnumerable<CartMemberDTO> SignIn(string username, string password, string logby)
        {
            IEnumerable<CartMemberDTO> member = null;

            if (logby == "apps")
            {
                member = from t in db.KART_MEMBER
                         where t.USERID == username && t.USERPW == password
                         select new CartMemberDTO
                         {
                             MemberId = t.MEMBERID,
                             MemberNm = t.MEMBERNM,
                             EmailId = t.EMAILID,
                             MobileNo = t.MOBILENO,
                             Address = t.ADDRESS,
                             Logby = t.LOGBY,
                             UserId = t.USERID,
                             UserPw = t.USERPW
                         };


                // return member;
            }

            else if (logby == "fb")
            {
                var res = (from n in db.KART_MEMBER where n.EMAILID == username && n.USERID == username select n).ToList();

                if (res.Count == 0)
                {
                    var checkMaxMemberId = (from n in db.KART_MEMBER select (int?)n.MEMBERID).Max() ?? 0;
                    Int64 memberid = checkMaxMemberId + 1;

                    CartMember mem = new CartMember
                    {
                        MEMBERID = memberid,
                        LOGBY = logby,
                        USERPW = password,
                        EMAILID = username,
                        USERID = username
                    };
                    db.KART_MEMBER.Add(mem);
                    db.SaveChanges();



                    member = from t in db.KART_MEMBER
                             where t.USERID == username && t.USERPW == password
                             select new CartMemberDTO
                             {
                                 MemberId = t.MEMBERID,
                                 MemberNm = t.MEMBERNM,
                                 EmailId = t.EMAILID,
                                 MobileNo = t.MOBILENO,
                                 Address = t.ADDRESS,
                                 Logby = t.LOGBY,
                                 UserId = t.USERID,
                                 UserPw = t.USERPW
                             };


                    //return member;
                }
                else
                {
                    member = from t in db.KART_MEMBER
                             where t.USERID == username && t.USERPW == password
                             select new CartMemberDTO
                             {
                                 MemberId = t.MEMBERID,
                                 MemberNm = t.MEMBERNM,
                                 EmailId = t.EMAILID,
                                 MobileNo = t.MOBILENO,
                                 Address = t.ADDRESS,
                                 Logby = t.LOGBY,
                                 UserId = t.USERID,
                                 UserPw = t.USERPW
                             };


                    //return member;
                }

            }


            return member;
        }
    }
}
