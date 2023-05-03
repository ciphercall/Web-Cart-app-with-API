using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace AslWebCartAPI.Controllers
{
    public class ApiCartTransactionController : ApiController
    {
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");

        private AslWebCartContext db = new AslWebCartContext();

        [Route("api/main/GenerateVoucher")]
        [HttpPost]
        public HttpResponseMessage GenerateVoucher(JObject jsonData)
        {
            string jData = jsonData.GetValue("jsonData").Value<string>();
            var jArray = JArray.Parse(jData);
            int length = jArray.Count;

            IList<string> result = new List<string>();


            string shipmentDistrict = (jArray[0]["shipmentDistrict"]).ToString();
            string transactionType = (jArray[0]["transactionType"]).ToString();
            DateTime shipmentDate = DateTime.Parse((jArray[0]["shipmentDate"]).ToString());

            decimal totalNet = Convert.ToDecimal(jArray[0]["totalNet"]);
            decimal totalAmount = Convert.ToDecimal(jArray[0]["totalAmount"]);
            decimal shippingCost = Convert.ToDecimal(jArray[0]["shippingCost"]);

            string shipmentAddress = (jArray[0]["shipmentAddress"]).ToString();
            Int64 memberId = Convert.ToInt64(jArray[0]["memberId"]);
            string transactionMode = (jArray[0]["transactionMode"]).ToString();

            decimal discountAmount = Convert.ToDecimal(jArray[0]["discountAmount"]);
            decimal mobile = Convert.ToDecimal(jArray[0]["mobile"]);

            DateTime transdate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(transdate.ToString("dd-MM-yy"));

            String dy = transdate.ToString("dd");
            String mn = transdate.ToString("MM");
            String yy = transdate.ToString("yy");

            string transactionNumber;
            string srl = null;


            var sl = (from n in db.KART_TRANSMST where EntityFunctions.TruncateTime((DateTime)n.TRANSDT) == DateTime.Today select n.TRANSNO).ToList();

            if (sl.Count == 0)
            {
                //if (Convert.ToInt64(dy) < 10)
                //{
                //    dy = "0" + dy;
                //}
                //if (Convert.ToInt64(mn) < 10)
                //{
                //    mn = "0" + mn;
                //}


                transactionNumber = dy + mn + yy + "0001";
            }
            else
            {
                var maxtranssl = (from n in db.KART_TRANSMST where EntityFunctions.TruncateTime((DateTime)n.TRANSDT) == DateTime.Today select n.TRANSNO).Max();
                var suffix = maxtranssl.Substring(maxtranssl.Length - 4);

                Int64 serialno = Convert.ToInt64(suffix) + 1;

                if (serialno < 1000)
                {
                    if (serialno < 10)
                    {
                        srl = "000" + serialno;
                    }
                    else if (serialno < 100)
                    {
                        srl = "00" + serialno;
                    }

                    else srl = "0" + serialno;
                }

                transactionNumber = dy + mn + yy + srl;
            } 


            CartTransMst model = new CartTransMst
                    {
                        TRANSTP = transactionType,
                        TRANSDT = transdate,
                        TRANSNO = transactionNumber,
                        MEMBERID = memberId,
                        SHIPDIST = shipmentDistrict,
                        SHIPADDR = shipmentAddress,
                        SHIPDT = shipmentDate,
                        CONTACTNO = mobile,
                        TRANSMODE = transactionMode,
                        TOTAMT = totalAmount,
                        DISAMT = discountAmount,
                        SCCOST = shippingCost,
                        TOTNET = totalNet

                    };
            db.KART_TRANSMST.Add(model);
            db.SaveChanges();


            for (int i = 1; i <= length - 1; i++)
            {

                Int64 itemId = Convert.ToInt64(jArray[i]["itemId"]);
                decimal quantity = Convert.ToDecimal(jArray[i]["quantity"]);
                decimal rate = Convert.ToDecimal(jArray[i]["rate"]);
                decimal amount = Convert.ToDecimal(jArray[i]["amount"]);

                CartTrans trans = new CartTrans
                   {
                       TRANSTP = transactionType,
                       TRANSDT = transdate,
                       TRANSNO = transactionNumber,
                       MEMBERID = memberId,
                       ITEMID = Convert.ToInt64(jArray[i]["itemId"]),
                       QTY = Convert.ToDecimal(jArray[i]["quantity"]),
                       RATE = Convert.ToDecimal(jArray[i]["rate"]),
                       AMOUNT = Convert.ToDecimal(jArray[i]["amount"])
                   };

                db.KART_TRANS.Add(trans);
                db.SaveChanges();

            }

            var formatter = new JsonMediaTypeFormatter();
            var json = formatter.SerializerSettings;

            result.Add(transactionNumber);

            // return transactionNumber;
            return Request.CreateResponse(HttpStatusCode.OK, result, formatter);

        }


        [Route("api/main/OrderDetails")]
        [HttpGet]
        public IEnumerable<CartTransMstDTO> OrderDetails(string memberid)
        {
            Int64 member = Convert.ToInt64(memberid);

            IEnumerable<CartTransMstDTO> some = (from t1 in db.KART_TRANSMST
                                                 where t1.MEMBERID == member
                                                 select new CartTransMstDTO
                                                 {
                                                     TRANSTP = t1.TRANSTP,
                                                     TRANSDT = t1.TRANSDT,
                                                     TRANSNO = t1.TRANSNO,
                                                     MEMBERID = t1.MEMBERID,

                                                     SHIPDIST = t1.SHIPDIST,
                                                     SHIPADDR = t1.SHIPADDR,
                                                     SHIPDT = t1.SHIPDT,
                                                     CONTACTNO = t1.CONTACTNO,
                                                     TRANSMODE = t1.TRANSMODE,
                                                     TOTAMT = t1.TOTAMT,
                                                     DISAMT = t1.DISAMT,
                                                     SCCOST = t1.SCCOST,
                                                     TOTNET = t1.TOTNET,
                                                     REMARKS = t1.REMARKS,
                                                     ORDERSTATUS = t1.ORDERSTATUS,
                                                     CARTTRANS = (from s1 in db.KART_TRANSMST
                                                                  join s2 in db.KART_TRANS on new { memID = s1.MEMBERID, transsl = s1.TRANSNO } equals new { memID = s2.MEMBERID, transsl = s2.TRANSNO }
                                                                  join s3 in db.KART_ITEM on s2.ITEMID equals s3.ITEMID
                                                                  where s1.MEMBERID == member && s1.TRANSNO == t1.TRANSNO
                                                                  select new CartTransDTO
                                                                  {

                                                                      ITEMID = s2.ITEMID,
                                                                      ITEMNM = s3.ITEMNM,
                                                                      QTY = s2.QTY,
                                                                      RATE = s2.RATE,
                                                                      AMOUNT = s2.AMOUNT
                                                                  })
                                                 });


            return some;
        }

    }
}


//TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
//            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
//            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy");
//            var time = Convert.ToString(PrintDate.ToString("hh:mms tt");
