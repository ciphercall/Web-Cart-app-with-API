using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;

namespace AslWebCartAPI.Controllers
{
    public class ItemController : AppController
    {

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;


        AslWebCartContext db = new AslWebCartContext();


        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        // GET: Item
        public ActionResult Create()
        {
            return View();
        }


        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CartItemDTO model, HttpPostedFileBase photo1, HttpPostedFileBase photo2, HttpPostedFileBase photo3, HttpPostedFileBase photo4, HttpPostedFileBase photo5)
        {
            var find_that_id = (from n in db.KART_ITEM where n.ITEMID == model.ItemId select n).ToList();

            if (find_that_id.Count != 0)
            {


                foreach (var x in find_that_id)
                {
                    x.ITEMNM = model.ItemName;
                    x.STOCKTP = model.StockType;
                    x.RATEBDT = model.RateBdt;
                    x.RATEUSD = model.RateUsd;

                    if (photo1 != null || photo2 != null || photo3 != null || photo4 != null || photo5 != null)
                    {
                        string filename1 = "", filename2 = "", filename3 = "", filename4 = "", filename5="";
                        if (photo1!= null)
                        {
                            filename1 = photo1.FileName;
                        }
                        if (photo2 != null)
                        {
                            filename2 = photo2.FileName;
                        }
                        if (photo3 != null)
                        {
                            filename3 = photo3.FileName;
                        }
                        if (photo4 != null)
                        {
                            filename4 = photo4.FileName;
                        }
                        if (photo5 != null)
                        {
                            filename5 = photo5.FileName;
                        }

                      

                        string exten1 = Path.GetExtension(filename1);
                        string exten2 = Path.GetExtension(filename2);
                        string exten3 = Path.GetExtension(filename3);
                        string exten4 = Path.GetExtension(filename4);
                        string exten5 = Path.GetExtension(filename5);


                        exten1.ToLower();
                        exten2.ToLower();
                        exten3.ToLower();
                        exten4.ToLower();
                        exten5.ToLower();

                        string[] acceptedFiletype1 = new string[1];


                        acceptedFiletype1[0] = ".png";

                        bool acceptFile1 = false;
                        bool acceptFile2 = false;
                        bool acceptFile3 = false;
                        bool acceptFile4 = false;
                        bool acceptFile5 = false;
                        for (int i = 0; i < 1; i++)
                        {

                            if (exten1 == acceptedFiletype1[i])
                            {
                                acceptFile1 = true;
                            }
                            if (exten2 == acceptedFiletype1[i])
                            {
                                acceptFile2 = true;
                            }
                            if (exten3 == acceptedFiletype1[i])
                            {
                                acceptFile3 = true;
                            }
                            if (exten4 == acceptedFiletype1[i])
                            {
                                acceptFile4 = true;
                            }
                            if (exten5 == acceptedFiletype1[i])
                            {
                                acceptFile5 = true;
                            }
                        }
                        if (!acceptFile1 && !acceptFile2 && !acceptFile3 && !acceptFile4 && !acceptFile5)
                        {

                        }

                        else
                        {
                            x.IMAGEID1 = Convert.ToInt64(Convert.ToString(x.ITEMID) + "1");
                            x.IMAGEID2 = Convert.ToInt64(Convert.ToString(x.ITEMID) + "2");
                            x.IMAGEID3 = Convert.ToInt64(Convert.ToString(x.ITEMID) + "3");
                            x.IMAGEID4 = Convert.ToInt64(Convert.ToString(x.ITEMID) + "4");
                            x.IMAGEID5 = Convert.ToInt64(Convert.ToString(x.ITEMID) + "5");

                            if (photo1 != null)
                            {
                                photo1.SaveAs(HttpContext.Server.MapPath("~/image-item/" + x.IMAGEID1 + ".png"));
                            }
                            if (photo2 != null)
                            {
                                photo2.SaveAs(HttpContext.Server.MapPath("~/image-item/" + x.IMAGEID2 + ".png"));
                            }
                            if (photo3 != null)
                            {
                                photo3.SaveAs(HttpContext.Server.MapPath("~/image-item/" + x.IMAGEID3 + ".png"));
                            }
                            if (photo4 != null)
                            {
                                photo4.SaveAs(HttpContext.Server.MapPath("~/image-item/" + x.IMAGEID4 + ".png"));
                            }
                            if (photo5 != null)
                            {
                                photo5.SaveAs(HttpContext.Server.MapPath("~/image-item/" + x.IMAGEID5 + ".png"));
                            }
                           
                           
                           
                           
                           

                        }
                    }


                    db.SaveChanges();


                }
                TempData["ItemId"] = "";





            }
            return RedirectToAction("Update");
        }


        [HttpPost]
        public ActionResult Submit(CartItemDTO model, HttpPostedFileBase photo1, HttpPostedFileBase photo2, HttpPostedFileBase photo3, HttpPostedFileBase photo4, HttpPostedFileBase photo5)
        {
            if (model.ItemName != null && model.StockType != null)
            {
                var findifexist =
                (from n in db.KART_ITEM where n.ITEMNM == model.ItemName select n).ToList();
                CartItem objCartItem = new CartItem();
                if (findifexist.Count == 0)
                {
                    var maxdatafind = (from n in db.KART_ITEM select n.ITEMID).Max();
                    if (maxdatafind == null)
                    {
                        objCartItem.ITEMID = 20000001;
                        objCartItem.ITEMNM = model.ItemName;
                        objCartItem.STOCKTP = model.StockType;
                        objCartItem.RATEBDT = Convert.ToDecimal(model.RateBdt);
                        objCartItem.RATEUSD = Convert.ToDecimal(model.RateUsd);
                        if (photo1 != null && photo2 != null && photo3 != null && photo4 != null && photo5 != null)
                        {
                            objCartItem.IMAGEID1 = 200000011;
                            objCartItem.IMAGEID2 = 200000012;
                            objCartItem.IMAGEID3 = 200000013;
                            objCartItem.IMAGEID4 = 200000014;
                            objCartItem.IMAGEID5 = 200000015;
                        }


                        db.KART_ITEM.Add(objCartItem);
                        db.SaveChanges();



                    }
                    else
                    {
                        objCartItem.ITEMID = maxdatafind + 1;
                        objCartItem.ITEMNM = model.ItemName;
                        objCartItem.STOCKTP = model.StockType;
                        objCartItem.RATEBDT = Convert.ToDecimal(model.RateBdt);
                        objCartItem.RATEUSD = Convert.ToDecimal(model.RateUsd);
                        if (photo1 != null && photo2 != null && photo3 != null && photo4 != null && photo5 != null)
                        {
                            objCartItem.IMAGEID1 = Convert.ToInt64(Convert.ToString(objCartItem.ITEMID) + "1");
                            objCartItem.IMAGEID2 = Convert.ToInt64(Convert.ToString(objCartItem.ITEMID) + "2");
                            objCartItem.IMAGEID3 = Convert.ToInt64(Convert.ToString(objCartItem.ITEMID) + "3");
                            objCartItem.IMAGEID4 = Convert.ToInt64(Convert.ToString(objCartItem.ITEMID) + "4");
                            objCartItem.IMAGEID5 = Convert.ToInt64(Convert.ToString(objCartItem.ITEMID) + "5");
                        }


                        db.KART_ITEM.Add(objCartItem);
                        db.SaveChanges();


                    }
                }
                else
                {
                    TempData["ItemMessage"] = "Duplicate Data Couldn't Be Created";
                }
                if (photo1 != null || photo2 != null || photo3 != null || photo4 != null || photo5 != null)
                {
                    string filename1 = "", filename2 = "", filename3 = "", filename4 = "", filename5 = "";
                    if (photo1 != null)
                    {
                        filename1 = photo1.FileName;
                    }
                    if (photo2 != null)
                    {
                        filename2 = photo2.FileName;
                    }
                    if (photo3 != null)
                    {
                        filename3 = photo3.FileName;
                    }
                    if (photo4 != null)
                    {
                        filename4 = photo4.FileName;
                    }
                    if (photo5 != null)
                    {
                        filename5 = photo5.FileName;
                    }

                    string exten1 = Path.GetExtension(filename1);
                    string exten2 = Path.GetExtension(filename2);
                    string exten3 = Path.GetExtension(filename3);
                    string exten4 = Path.GetExtension(filename4);
                    string exten5 = Path.GetExtension(filename5);


                    exten1.ToLower();
                    exten2.ToLower();
                    exten3.ToLower();
                    exten4.ToLower();
                    exten5.ToLower();

                    string[] acceptedFiletype1 = new string[1];


                    acceptedFiletype1[0] = ".png";

                    bool acceptFile1 = false;
                    bool acceptFile2 = false;
                    bool acceptFile3 = false;
                    bool acceptFile4 = false;
                    bool acceptFile5 = false;
                    for (int i = 0; i < 1; i++)
                    {

                        if (exten1 == acceptedFiletype1[i])
                        {
                            acceptFile1 = true;
                        }
                        if (exten2 == acceptedFiletype1[i])
                        {
                            acceptFile2 = true;
                        }
                        if (exten3 == acceptedFiletype1[i])
                        {
                            acceptFile3 = true;
                        }
                        if (exten4 == acceptedFiletype1[i])
                        {
                            acceptFile4 = true;
                        }
                        if (exten5 == acceptedFiletype1[i])
                        {
                            acceptFile5 = true;
                        }
                    }
                    if (!acceptFile1 && !acceptFile2 && !acceptFile3 && !acceptFile4 && !acceptFile5)
                    {

                    }

                    else
                    {


                        if (photo1 != null)
                        {
                            photo1.SaveAs(HttpContext.Server.MapPath("~/image-item/" + objCartItem.IMAGEID1 + ".png"));
                        }
                        if (photo2 != null)
                        {
                            photo2.SaveAs(HttpContext.Server.MapPath("~/image-item/" + objCartItem.IMAGEID2 + ".png"));
                        }
                        if (photo3 != null)
                        {
                            photo3.SaveAs(HttpContext.Server.MapPath("~/image-item/" + objCartItem.IMAGEID3 + ".png"));
                        }
                        if (photo4 != null)
                        {
                            photo4.SaveAs(HttpContext.Server.MapPath("~/image-item/" + objCartItem.IMAGEID4 + ".png"));
                        }
                        if (photo5 != null)
                        {
                            photo5.SaveAs(HttpContext.Server.MapPath("~/image-item/" + objCartItem.IMAGEID5 + ".png"));
                        }
                           

                    }
                }

            }






            return RedirectToAction("Create");
        }



        //AutoComplete
        public JsonResult ItemListTag(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());




            var tags = from p in db.KART_ITEM

                       select p.ITEMNM;

            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);




        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DynamicautocompleteItem(string changedText)
        {
            using (var db = new AslWebCartContext())
            {

                String name = "";

                var rt = db.KART_ITEM.Where(n => n.ITEMNM.StartsWith(changedText)).Select(n => new
                {
                    headname = n.ITEMNM

                }).ToList();
                int lenChangedtxt = changedText.Length;

                Int64 cont = rt.Count();
                Int64 k = 0;
                string status = "";
                if (changedText != "" && cont != 0)
                {
                    while (status != "no")
                    {
                        k = 0;
                        foreach (var n in rt)
                        {
                            string ss = Convert.ToString(n.headname);
                            string subss = "";
                            if (ss.Length >= lenChangedtxt)
                            {
                                subss = ss.Substring(0, lenChangedtxt);
                                subss = subss.ToUpper();
                            }
                            else
                            {
                                subss = "";
                            }


                            if (subss == changedText.ToUpper())
                            {
                                k++;
                            }
                            if (k == cont)
                            {
                                status = "yes";
                                lenChangedtxt++;
                                if (ss.Length > lenChangedtxt - 1)
                                {
                                    changedText = changedText + ss[lenChangedtxt - 1];
                                }

                            }
                            else
                            {
                                status = "no";
                                //lenChangedtxt--;
                            }

                        }

                    }
                    if (lenChangedtxt == 1)
                    {
                        name = changedText.Substring(0, lenChangedtxt);
                    }

                    else
                    {
                        name = changedText.Substring(0, lenChangedtxt - 1);
                    }



                }
                else
                {
                    name = changedText;
                }



                String itemId2 = "";
                string Stocktp = "";
                decimal ratebdt = 0, rateusd = 0;
                var rt2 = db.KART_ITEM.Where(n => n.ITEMNM == name).Select(n => new
                                                             {
                                                                 itemid = n.ITEMID,
                                                                 stocktp = n.STOCKTP,
                                                                 ratebdt = n.RATEBDT,
                                                                 rateusd = n.RATEUSD
                                                             });
                foreach (var n in rt2)
                {
                    itemId2 = Convert.ToString(n.itemid);
                    Stocktp = Convert.ToString(n.stocktp);
                    ratebdt = n.ratebdt;
                    rateusd = n.rateusd;
                }

                var result = new { ItemName = name, ItemId = itemId2, StockTp = Stocktp, Ratebdt = ratebdt, Rateusd = rateusd };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
        }

    }
}