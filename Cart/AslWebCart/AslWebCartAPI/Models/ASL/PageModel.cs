using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace AslWebCartAPI.Models
{
    public class PageModel
    {

        public PageModel()
        {
            this.aslMenumst = new ASL_MENUMST();
            this.aslMenu = new ASL_MENU();
            this.aslUserco = new AslUserco();
            this.aslCompany = new AslCompany();
            this.aslLog = new ASL_LOG();
         



           
         


          
        }

        public ASL_MENUMST aslMenumst { get; set; }
        public ASL_MENU aslMenu { get; set; }
        public AslUserco aslUserco { get; set; }
        public AslCompany aslCompany { get; set; }
        public ASL_LOG aslLog { get; set; }


      


       


        [Display(Name = "HeadType")]
        public string HeadType { get; set; }


        [Required(ErrorMessage = "From date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date field can not be empty!")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }   

    }
}