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

    public class CategoryController : AppController
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

        [AcceptVerbs("GET")]
        [ActionName("Create")]
        // GET: Category
        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs("POST")]
        [ActionName("Create")]
        public ActionResult CreatePost(CartCategoryDTO model)
        {





            return View("Create");

        }




        [AcceptVerbs("GET")]
        [ActionName("Update")]
        // GET: Category
        public ActionResult Update()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CartCategoryDTO model,HttpPostedFileBase photo)
        {
            var find_that_id = (from n in db.KART_CATEGORY where n.CATID == model.CategoryId select n).ToList();
            //var find_nameExist =(from n in db.KART_CATEGORY where n.CATNM == model.CategoryName select n).ToList().Count();
            
            if (find_that_id.Count != 0)
            {


                foreach (var x in find_that_id)
                {
                    x.CATNM = model.CategoryName;
                    x.LEVELGR = model.LevelGroup;
                 

                    db.SaveChanges();


                }
                if (photo != null)
                {
                    string filename = photo.FileName;
                    string exten = Path.GetExtension(filename);
                    exten.ToLower();
                    string[] acceptedFiletype = new string[1];

                    acceptedFiletype[0] = ".png";

                    bool acceptFile = false;
                    for (int i = 0; i < 1; i++)
                    {

                        if (exten == acceptedFiletype[i])
                        {
                            acceptFile = true;
                        }
                    }
                    if (!acceptFile)
                    {

                    }

                    else
                    {


                        photo.SaveAs(HttpContext.Server.MapPath("~/image-category/" + model.CategoryId + ".png"));
                    }
                }
                TempData["CatID"] = "";


            }
            return RedirectToAction("Update");
        }
       

        [HttpPost]
        public ActionResult Submit(CartCategoryDTO model,HttpPostedFileBase photo)
        {
            if (model.CategoryName != null && model.LevelGroup != null)
            {
                var findifexist =
                (from n in db.KART_CATEGORY where n.CATNM == model.CategoryName select n).ToList();
                CartCategory objCartCategory = new CartCategory();
                if (findifexist.Count == 0)
                {
                    var maxdatafind = (from n in db.KART_CATEGORY select n.CATID).Max();
                    if (maxdatafind == null)
                    {
                        objCartCategory.CATID = 10000001;
                        objCartCategory.CATNM = model.CategoryName;
                        objCartCategory.LEVELGR = model.LevelGroup;


                        db.KART_CATEGORY.Add(objCartCategory);
                        db.SaveChanges();


                    }
                    else
                    {
                        objCartCategory.CATID = maxdatafind + 1;
                        objCartCategory.CATNM = model.CategoryName;
                        objCartCategory.LEVELGR = model.LevelGroup;


                        db.KART_CATEGORY.Add(objCartCategory);
                        db.SaveChanges();

                    }
                    if (photo != null)
                    {
                        string filename = photo.FileName;
                        string exten = Path.GetExtension(filename);
                        exten.ToLower();
                        string[] acceptedFiletype = new string[1];

                        acceptedFiletype[0] = ".png";

                        bool acceptFile = false;
                        for (int i = 0; i < 1; i++)
                        {

                            if (exten == acceptedFiletype[i])
                            {
                                acceptFile = true;
                            }
                        }
                        if (!acceptFile)
                        {

                        }

                        else
                        {


                            photo.SaveAs(HttpContext.Server.MapPath("~/image-category/" + objCartCategory.CATID + ".png"));
                        }
                    }



                }
                else
                {
                    TempData["CategoryMessage"] = "Duplicate Name will not create";
                }
            }
           
            
            return RedirectToAction("Create");
        }


        //AutoComplete
        public JsonResult CategoryListTag(string term)
        {
            var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());




            var tags = from p in db.KART_CATEGORY

                       select p.CATNM;

            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);




        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DynamicautocompleteCat(string changedText)
        {
            using (var db = new AslWebCartContext())
            {

                String name = "";

                var rt = db.KART_CATEGORY.Where(n => n.CATNM.StartsWith(changedText)).Select(n => new
                {
                    headname = n.CATNM

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



                String Catid = "";
                string Levelgroup = "";
              
                var rt2 = db.KART_CATEGORY.Where(n => n.CATNM == name).Select(n => new
                {
                    catid = n.CATID,
                    catname = n.CATNM,
                    levelgroup = n.LEVELGR
                   
                });
                foreach (var n in rt2)
                {
                    Catid = Convert.ToString(n.catid);
                    Levelgroup = Convert.ToString(n.levelgroup);
                   
                }



                var result = new { CategoryName = name, CatID = Catid, LevelGroup = Levelgroup };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
        }

    }
}