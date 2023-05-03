using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AslWebCartAPI.Controllers
{
    public class CartCategoryController : Controller
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




        // GET: CartCategory
        [AcceptVerbs("GET")]
        [ActionName("Index")]
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(CartItemDTO model, string command)
        {
            if (command == "Add")
            {
            }
            return RedirectToAction("Details", "CartCategory", new { Id = model.ItemId });
        }
        public ActionResult Details(int ItemId)
        {
            using (var db = new AslWebCartContext())
            {
                return View(db.KART_ITEM.FirstOrDefault(p => p.ITEMID == ItemId));
            }
        }


    }
}