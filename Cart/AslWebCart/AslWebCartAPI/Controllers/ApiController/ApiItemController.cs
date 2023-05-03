using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;

namespace AslWebCartAPI.Controllers
{
    public class ApiItemController : ApiController
    {
        private AslWebCartContext db = new AslWebCartContext();

        //[Route("api/ItemCreate")]
        //[HttpGet]
        //public IEnumerable<CartItemDTO> ItemCreate(string ItemName, string StockType, string RateBDT, string RateUSD)
        //{
        //    var findifexist =
        //        (from n in db.KART_ITEM where n.ITEMNM == ItemName && n.STOCKTP == StockType select n).ToList();
        //    CartItem objCartItem = new CartItem();
        //    if (findifexist.Count == 0)
        //    {
        //        var maxdatafind = (from n in db.KART_ITEM select n.ITEMID).Max();
        //        if (maxdatafind == 0)
        //        {
        //            objCartItem.ITEMID = 20000001;
        //            objCartItem.ITEMNM = ItemName;
        //            objCartItem.STOCKTP = StockType;
        //            objCartItem.RATEBDT = Convert.ToDecimal(RateBDT);
        //            objCartItem.RATEUSD = Convert.ToDecimal(RateUSD);

        //            db.KART_ITEM.Add(objCartItem);
        //            db.SaveChanges();

        //            yield return new CartItemDTO()
        //            {
        //                ItemId = objCartItem.ITEMID,
        //                ItemName = objCartItem.ITEMNM,
        //                StockType = objCartItem.STOCKTP,
        //                RateBdt=Convert.ToDecimal(objCartItem.RATEBDT),
        //                RateUsd=Convert.ToDecimal(objCartItem.RATEUSD)

        //            };

        //        }
        //        else
        //        {
        //            objCartItem.ITEMID = maxdatafind + 1;
        //           objCartItem.ITEMNM = ItemName;
        //            objCartItem.STOCKTP = StockType;
        //            objCartItem.RATEBDT = Convert.ToDecimal(RateBDT);
        //            objCartItem.RATEUSD = Convert.ToDecimal(RateUSD);

        //            db.KART_ITEM.Add(objCartItem);
        //            db.SaveChanges();

        //            yield return new CartItemDTO()
        //            {
        //               ItemId = objCartItem.ITEMID,
        //                ItemName = objCartItem.ITEMNM,
        //                StockType = objCartItem.STOCKTP,
        //                RateBdt=Convert.ToDecimal(objCartItem.RATEBDT),
        //                RateUsd=Convert.ToDecimal(objCartItem.RATEUSD)
        //            };
        //        }
        //    }





        //}


        //[Route("api/AutoCompleteitemlist")]
        //[HttpGet]
        //public IEnumerable<CartItemDTO> GetItemList(string query = "")
        //{
        //    using (var db = new AslWebCartContext())
        //    {



        //        return db.KART_ITEM.Where(p => p.ITEMNM.StartsWith(query)).Select(
        //                     x =>
        //                     new
        //                     {
        //                         itemname = x.ITEMNM,
        //                         itemid = x.ITEMID,
        //                         stocktp = x.STOCKTP,
        //                         ratebdt=x.RATEBDT,
        //                         rateusd=x.RATEUSD
        //                     })
        //           .AsEnumerable().Select(a => new CartItemDTO
        //           {
        //               ItemName = a.itemname,
        //               ItemId = Convert.ToInt64(Convert.ToInt64(a.itemid)),
        //               StockType = a.stocktp,
        //               RateBdt = a.ratebdt,
        //               RateUsd = a.rateusd
        //           }).ToList();

        //    }
        //}

        //[Route("api/DynamicautocompleteItem")]
        //[HttpGet]
        //public IEnumerable<CartItemDTO> GetItemNameNID(string changedText, string changedText2)
        //{
        //    using (var db = new AslWebCartContext())
        //    {

        //        String name = "";

        //        var rt = db.KART_ITEM.Where(n => n.ITEMNM.StartsWith(changedText)).Select(n => new
        //        {
        //            headname = n.ITEMNM

        //        }).ToList();
        //        int lenChangedtxt = changedText.Length;

        //        Int64 cont = rt.Count();
        //        Int64 k = 0;
        //        string status = "";
        //        if (changedText != "" && cont != 0)
        //        {
        //            while (status != "no")
        //            {
        //                k = 0;
        //                foreach (var n in rt)
        //                {
        //                    string ss = Convert.ToString(n.headname);
        //                    string subss = "";
        //                    if (ss.Length >= lenChangedtxt)
        //                    {
        //                        subss = ss.Substring(0, lenChangedtxt);
        //                        subss = subss.ToUpper();
        //                    }
        //                    else
        //                    {
        //                        subss = "";
        //                    }


        //                    if (subss == changedText.ToUpper())
        //                    {
        //                        k++;
        //                    }
        //                    if (k == cont)
        //                    {
        //                        status = "yes";
        //                        lenChangedtxt++;
        //                        if (ss.Length > lenChangedtxt - 1)
        //                        {
        //                            changedText = changedText + ss[lenChangedtxt - 1];
        //                        }

        //                    }
        //                    else
        //                    {
        //                        status = "no";
        //                        //lenChangedtxt--;
        //                    }

        //                }

        //            }
        //            if (lenChangedtxt == 1)
        //            {
        //                name = changedText.Substring(0, lenChangedtxt);
        //            }

        //            else
        //            {
        //                name = changedText.Substring(0, lenChangedtxt - 1);
        //            }



        //        }
        //        else
        //        {
        //            name = changedText;
        //        }

        //        var findid = (from n in db.KART_ITEM where n.ITEMNM == name select n).ToList();
        //        if (findid.Count != 0)
        //        {
        //            return db.KART_ITEM.Where(p => p.ITEMNM == name).Select(
        //                x =>
        //                    new
        //                    {
        //                        itemName = x.ITEMNM,
        //                        itemId = x.ITEMID,
        //                        stockTp = x.STOCKTP,
        //                        rateBdt=x.RATEBDT,
        //                        rateUsd=x.RATEUSD
        //                    })
        //                .AsEnumerable().Select(a => new CartItemDTO
        //                {
        //                    ItemName = a.itemName,
        //                    ItemId = Convert.ToInt64(a.itemId),
        //                    StockType = a.stockTp,
        //                    RateBdt = a.rateBdt,
        //                    RateUsd = a.rateUsd
        //                }).ToList();
        //        }
        //        else
        //        {
        //            return db.KART_ITEM.AsEnumerable().Select(a => new CartItemDTO
        //            {
        //                ItemName = name,
        //                ItemId = Convert.ToInt64(changedText2),
        //                StockType = "",
        //                RateBdt=0,
        //                RateUsd=0
        //            }).ToList();
        //        }


        //    }
        //}

        [Route("api/ItemUpdate")]
        [HttpGet]
        public IEnumerable<CartItemDTO> ItemUpdate(string ItemName, string StockType, Int64 Id,decimal RateDbt,decimal RateUsd)
        {
            var find_that_id = (from n in db.KART_ITEM where n.ITEMID == Id select n).ToList();
         
            if (find_that_id.Count != 0)
            {


                foreach (var x in find_that_id)
                {
                    x.ITEMNM = ItemName;
                    x.STOCKTP = StockType;
                    x.RATEBDT = RateDbt;
                    x.RATEUSD = RateUsd;

                    db.SaveChanges();

                    yield return new CartItemDTO()
                    {
                        ItemId = Convert.ToInt64(x.ITEMID),
                        ItemName = x.ITEMNM,
                        StockType = x.STOCKTP,
                        RateBdt = x.RATEBDT,
                        RateUsd = x.RATEUSD
                    };

                }


            }





        }

    }
}
