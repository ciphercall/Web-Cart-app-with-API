using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Mvc;
using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;

namespace AslWebCartAPI.Controllers
{
    public class FilterController : AppController
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
        public ActionResult IndexPost(CartFilterMstDTO model)
        {
           
               
           
           

            return View("Index");
          
        }
    }
}
