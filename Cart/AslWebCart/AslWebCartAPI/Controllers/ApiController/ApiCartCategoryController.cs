using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;
using System.Data.SqlClient;
using System.Web;

namespace AslWebCartAPI.Controllers
{
    public class ApiCartCategoryController : ApiController
    {
        private AslWebCartContext db = new AslWebCartContext();

        // GET: api/ApiCartCategory

        /// <summary>
        /// Documentation for 'GET' method
        /// </summary>
        /// <returns>Returns a list of category in the requested format</returns>
        /// 
        [Route("api/main/firstcategory")]
        [HttpGet]
        public IEnumerable<MergeDTOs> GetKART_CATEGORY()
        {

            //var firstCategory = from cat in db.KART_CATEGORY
            //                    where cat.LEVELGR == "FIRST"
            //                    select new CartCategoryDTO()
            //                    {
            //                        Id = cat.ID,
            //                        CategoryId = cat.CATID,
            //                        CategoryName = cat.CATNM,
            //                        LevelGroup = cat.LEVELGR
            //                    };

            //var firstCategory = (from t1 in db.KART_CATEGORY
            //                     join t2 in db.KART_LEVEL
            //                     on t1.CATID equals t2.LEVELGID
            //                     where t1.LEVELGR == "FIRST"
            //                     select new
            //                      {
            //                          CategoryId = t1.CATID,
            //                          CategoryName = t1.CATNM,
            //                          LevelGroup = t1.LEVELGR,
            //                          LevelId = t2.LEVELID,
            //                          LevelName = t2.LEVELNM,
            //                          LevelType = t2.LEVELTP,
            //                          LevelGroupType = t2.LEVELGTP
            //                      });

            // }).AsEnumerable().Select(x => new CartItemDTO
            //{

            //    CategoryId = x.CategoryId,
            //    CategoryName = x.CategoryName,
            //    LevelGroup = x.LevelGroup,
            //    LevelId = x.LevelId,
            //    LevelName = x.LevelName,
            //    LevelType = x.LevelType,
            //    LevelGroupType = x.LevelGroupType
            //}).ToList(); ;



            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebCartDbContext"].ToString());
            string query = string.Format("SELECT LEVELGID, LEVELID, LEVELNM, LEVELGR FROM(  " +
                                      "  SELECT CATID LEVELGID, CATNM LEVELNM, '10000000' LEVELID, LEVELGR FROM KART_CATEGORY WHERE LEVELGR = 'FIRST'  " +
                                      "  UNION  " +
                                      "  SELECT A.LEVELGID, A.LEVELNM, A.LEVELID,   " +
                                      "  (CASE WHEN COUNT(DISTINCT KART_LEVEL.LEVELGID) = 0 THEN 'LAST' WHEN COUNT(DISTINCT KART_LEVEL.LEVELGID) > 0 THEN 'MIDDLE' END) LEVELGR FROM (    " +
                                      "  SELECT KART_LEVEL.LEVELGID, LEVELNM, LEVELID FROM   KART_LEVEL   " +
                                      "  INNER JOIN KART_CATEGORY ON KART_CATEGORY.CATID = KART_LEVEL.LEVELGID AND LEVELGR = 'FIRST' GROUP BY KART_LEVEL.LEVELGID, LEVELNM, LEVELID  " +
                                      "  ) A LEFT OUTER JOIN KART_LEVEL ON KART_LEVEL.LEVELGID = A.LEVELID AND LEVELTP = 'CATEGORY'  " +
                                      "  GROUP BY A.LEVELGID, A.LEVELNM, A.LEVELID  " +
                                      "  ) B ORDER BY B.LEVELGID, LEVELID");

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            // var cartCategoryItems = new List<MergeDTOs>();
            foreach (DataRow row in ds.Rows)
            {
                yield return new MergeDTOs
                {
                    LevelGroupId = Convert.ToInt64(row["LEVELGID"]),
                    LevelId = Convert.ToInt64(row["LEVELID"]),
                    LevelName = row["LEVELNM"].ToString(),
                    LevelGroup = row["LEVELGR"].ToString(),



                };
                // cartCategoryItems.Add(tankReading);
            }

            //   return cartCategoryItems.AsEnumerable();

        }

        [Route("api/main/cartlevel")]
        [HttpGet]
        public IEnumerable<MergeDTOs> GetCartLevel(string matchid)
        {
            Int64 mat = Convert.ToInt64(matchid);

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebCartDbContext"].ToString());
            string query = string.Format("SELECT B.LEVELGID, B.LEVELID, B.LEVELNM, B.LEVELGR " +
                                     "   FROM (   " +
                                     "   SELECT LEVELID LEVELGID, '10000000' LEVELID, LEVELNM, 'SECOND' LEVELGR FROM  KART_LEVEL  " +
                                     "   WHERE  LEVELGID = '" + mat + "' " +
                                     "   UNION " +
                                     "   SELECT A.LEVELID LEVELGID, KART_LEVEL.LEVELID, KART_LEVEL.LEVELNM, (CASE WHEN COUNT(DISTINCT LEVELGID) = 0 THEN 'LAST' WHEN COUNT(DISTINCT LEVELGID) > 0 THEN 'MIDDLE' END) LEVELGR FROM (  " +
                                     "   SELECT LEVELID FROM KART_LEVEL WHERE LEVELGID = '" + mat + "' " +
                                     "   ) A INNER JOIN KART_LEVEL ON A.LEVELID = KART_LEVEL.LEVELGID AND LEVELTP = 'CATEGORY' " +
                                     "   GROUP BY A.LEVELID, KART_LEVEL.LEVELID, KART_LEVEL.LEVELNM " +
                                     "   ) B GROUP BY B.LEVELGID, B.LEVELNM, B.LEVELID, B.LEVELGR " +
                                     "   ORDER BY B.LEVELGID, B.LEVELID");

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            // var cartCategoryItems = new List<MergeDTOs>();
            foreach (DataRow row in ds.Rows)
            {
                yield return new MergeDTOs
                {
                    LevelGroupId = Convert.ToInt64(row["LEVELGID"]),
                    LevelId = Convert.ToInt64(row["LEVELID"]),
                    LevelName = row["LEVELNM"].ToString(),
                    LevelGroup = row["LEVELGR"].ToString(),

                };
            }
        }


        [Route("api/main/CartJson")]
        [HttpGet]
        public IEnumerable<CartCategoryDTO> CartJson()
        {
            List<CartCategoryDTO> cartitemdto = new List<CartCategoryDTO>();

            IEnumerable<CartCategoryDTO> firstCategory = (from cat in db.KART_CATEGORY
                                                          where cat.LEVELGR == "FIRST"
                                                          select new CartCategoryDTO
                                                          {
                                                              CategoryId = Convert.ToInt64(cat.CATID),
                                                              CategoryName = cat.CATNM,
                                                              LevelGroup = cat.LEVELGR,
                                                              CartLevelDTO = (
                                                              from d in db.KART_LEVEL
                                                              where d.LEVELGID == cat.CATID
                                                              select new CartLevelDTO
                                                              {
                                                                  LevelId = d.LEVELID,
                                                                  LevelName = d.LEVELNM,
                                                                  LevelType = d.LEVELTP,
                                                                  LevelGroupType = d.LEVELGTP,
                                                                  LevelGroupId = d.LEVELGID

                                                              }).ToList()
                                                          });
            return firstCategory;
        }

        [Route("api/itemlist")]
        [HttpGet]
        public IEnumerable<CartItemDTO> GetItemList(string query = "")
        {
            using (var db = new AslWebCartContext())
            {
                return String.IsNullOrEmpty(query) ? db.KART_ITEM.AsEnumerable().Select(b => new CartItemDTO { }).ToList() :
                db.KART_ITEM.Where(p => p.ITEMNM.Contains(query) && p.STOCKTP == "SINGLE").Select(
                          x =>
                          new
                          {
                              itemname = x.ITEMNM,
                              itemid = x.ITEMID,
                          })
                .AsEnumerable().Select(a => new CartItemDTO
                                 {
                                     ItemName = a.itemname,
                                     ItemId = Convert.ToInt64(a.itemid)
                                 }).ToList(); 
            }
        }

       

        [Route("api/filterlist")]
        [HttpGet]
        public IEnumerable<CartFilterDTO> GetFilterList(string filtergroupid, string query = "")
        {
            var id = Convert.ToInt64(filtergroupid);
            using (var db = new AslWebCartContext())
            {
                return String.IsNullOrEmpty(query) ? db.KART_FILTER.AsEnumerable().Select(b => new CartFilterDTO { }).ToList() :
                db.KART_FILTER.Where(p => p.FILTERNM.Contains(query) && p.FILTERGID == id).Select(
                          x =>
                          new
                          {
                              filtername = x.FILTERNM,
                              filterid = x.FILTERID,
                          })
                .AsEnumerable().Select(a => new CartFilterDTO
                {
                    FilterName = a.filtername,
                    FilterId = Convert.ToInt64(a.filterid)
                }).ToList(); ;
            }
        }


        //  [Route("api/CategoryCreate")]
        //  [HttpGet]
        //public IEnumerable<CartCategoryDTO> CategoryCreate(string CategoryName, string LevelGroup, HttpPostedFileBase photo)
        //  {
        //      var findifexist =
        //          (from n in db.KART_CATEGORY where n.CATNM == CategoryName && n.LEVELGR == LevelGroup select n).ToList();
        //      CartCategory objCartCategory=new CartCategory();
        //      if (findifexist.Count == 0)
        //      {
        //          var maxdatafind =(from n in db.KART_CATEGORY select n.CATID).Max();
        //          if (maxdatafind == 0)
        //          {
        //              objCartCategory.CATID = 10000001;
        //              objCartCategory.CATNM = CategoryName;
        //              objCartCategory.LEVELGR = LevelGroup;
        //              objCartCategory.ImageId = 10000001;

        //              db.KART_CATEGORY.Add(objCartCategory);
        //              db.SaveChanges();
        //              yield return new CartCategoryDTO()
        //              {
        //                  CategoryId = objCartCategory.CATID,
        //                  CategoryName = objCartCategory.CATNM,
        //                  LevelGroup = objCartCategory.LEVELGR
        //              };

        //          }
        //          else
        //          {
        //              objCartCategory.CATID = maxdatafind+1;
        //              objCartCategory.CATNM = CategoryName;
        //              objCartCategory.LEVELGR = LevelGroup;
        //              objCartCategory.ImageId = maxdatafind + 1;
        //              db.KART_CATEGORY.Add(objCartCategory);
        //              db.SaveChanges();
        //              yield return new CartCategoryDTO()
        //              {
        //                  CategoryId = objCartCategory.CATID,
        //                  CategoryName = objCartCategory.CATNM,
        //                  LevelGroup = objCartCategory.LEVELGR
        //              };
        //          }
        //      }

            
                 
                 
              
        //  }


         

          [Route("api/CategoryUpdate")]
          [HttpGet]
          public IEnumerable<CartCategoryDTO> CategoryUpdate(string CategoryName, string LevelGroup,Int64 Id)
          {
              var find_that_id =(from n in db.KART_CATEGORY where n.CATID == Id select n).ToList();
             // CartCategory objCartCategory = new CartCategory();
              if (find_that_id.Count != 0)
              {


                  foreach (var x in find_that_id)
                  {
                      x.CATNM = CategoryName;
                      x.LEVELGR = LevelGroup;
                      // db.KART_CATEGORY.Add(objCartCategory);
                      db.SaveChanges();
                      yield return new CartCategoryDTO()
                      {
                          CategoryId = Convert.ToInt64(x.CATID),
                          CategoryName = x.CATNM,
                          LevelGroup = x.LEVELGR
                      };

                  }
                      
                 
              }





          }


          [Route("api/categorylist")]
          [HttpGet]
          public IEnumerable<CartCategoryDTO> GetCategoryList(string query = "")
          {
              using (var db = new AslWebCartContext())
              {
                 
              

               return db.KART_CATEGORY.Where(p => p.CATNM.StartsWith(query)).Select(
                            x =>
                            new
                            {
                                categoryname = x.CATNM,
                                catid = x.CATID,
                                levelgroup = x.LEVELGR,
                            })
                  .AsEnumerable().Select(a => new CartCategoryDTO
                  {
                      CategoryName = a.categoryname,
                      CategoryId = Convert.ToInt64(a.catid),
                      LevelGroup = a.levelgroup
                  }).ToList(); 
                
              }
          }

          [Route("api/Dynamicautocomplete")]
          [HttpGet]
          public IEnumerable<CartCategoryDTO> GetCategoryid(string changedText,string changedText2)
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

                  var findid = (from n in db.KART_CATEGORY where n.CATNM == name select n).ToList();
                  if (findid.Count != 0)
                  {
                      return db.KART_CATEGORY.Where(p => p.CATNM == name).Select(
                          x =>
                              new
                              {
                                  categoryname = x.CATNM,
                                  catid = x.CATID,
                                  levelgroup = x.LEVELGR,
                              })
                          .AsEnumerable().Select(a => new CartCategoryDTO
                          {
                              CategoryName = a.categoryname,
                              CategoryId = Convert.ToInt64(a.catid),
                              LevelGroup = a.levelgroup
                          }).ToList();
                  }
                  else
                  {
                       return db.KART_CATEGORY.AsEnumerable().Select(a => new CartCategoryDTO
                          {
                              CategoryName = name,
                              CategoryId = Convert.ToInt64(changedText2),
                              LevelGroup = ""
                          }).ToList();
                  }
                 

              }
          }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KART_CATEGORYExists(long id)
        {
            return db.KART_CATEGORY.Count(e => e.ID == id) > 0;
        }
    }
}


