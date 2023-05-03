using System.Web.Mvc;
using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace AslWebCartAPI.Controllers
{
    public class ApiLevelEntryController : ApiController
    {
        private AslWebCartContext db = new AslWebCartContext();


        [System.Web.Http.HttpGet]
        public IEnumerable<CartLevelMstDTO> Get(string LevelGroupType, string LevelGroupId, string LevelGroupName)
        {

           


            var cartdto = (from t1 in db.KART_LEVEL
                           join t2 in db.KART_CATEGORY on t1.LEVELGID equals t2.CATID
                           where t1.LEVELGID == 10000001 && t1.LEVELGTP == "CATEGORY"
                           select new CartLevelMstDTO
                           {
                               LevelGroupId = t1.LEVELGID,
                               LevelGroupName = t2.CATNM,
                               LevelGroupType = t1.LEVELGTP,
                               LevelId = t1.LEVELID,
                               LevelName = t1.LEVELNM,
                               LevelType = t1.LEVELTP

                           });

            return cartdto;

            //return db.KART_ITEM.AsEnumerable();

        }


        [System.Web.Http.Route("api/grid/GetData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartLevelMstDTO> GetLevelData(string LevelGroupType, string LevelGroupId, string LevelGroupName)
        {
            Int64 LevelGId = Convert.ToInt64(LevelGroupId);

            var res = db.KART_LEVELMST.Where(a => a.LEVELGID == LevelGId && a.LEVELGTP == LevelGroupType).Count(a => a.LEVELGID == LevelGId) == 0;

            if (res == true)
            {
                CartLevelMst ord = new CartLevelMst
                {
                    LEVELGID = LevelGId,
                    LEVELGTP = LevelGroupType

                };

                db.KART_LEVELMST.Add(ord);
                db.SaveChanges();

                yield return new CartLevelMstDTO()
                {
                    LevelGroupType = LevelGroupType,
                    LevelGroupId = LevelGId,
                    LevelGroupName = LevelGroupName,
                    LevelId = 0
                };
            }

            else
            {

                var cartdto = (from t1 in db.KART_LEVEL

                               join t2 in db.KART_CATEGORY on t1.LEVELID equals t2.CATID

                               where t1.LEVELGID == LevelGId && t1.LEVELGTP == LevelGroupType && t1.LEVELTP == "CATEGORY"
                               select new
                               {
                                   id = t1.ID,
                                   LevelGroupId = t1.LEVELGID,
                                   LevelGroupName = LevelGroupName,
                                   LevelGroupType = t1.LEVELGTP,
                                   LevelId = t1.LEVELID,
                                   LevelName=t2.CATNM,
                                   LevelNamenew = t1.LEVELNM,
                                   LevelType = t1.LEVELTP

                               }
                         ).Union
                         (from t1 in db.KART_LEVEL

                                 join t2 in db.KART_ITEM on t1.LEVELID equals t2.ITEMID
                              
                          where t1.LEVELGID == LevelGId && t1.LEVELGTP == LevelGroupType && (t1.LEVELTP == "ITEM-SINGLE" || t1.LEVELTP == "ITEM-GROUP")
                                 select new
                                 {
                                     id = t1.ID,
                                     LevelGroupId = t1.LEVELGID,
                                     LevelGroupName = LevelGroupName,
                                     LevelGroupType = t1.LEVELGTP,
                                     LevelId = t1.LEVELID,
                                     LevelName = t2.ITEMNM,
                                     LevelNamenew = t1.LEVELNM,
                                     LevelType = t1.LEVELTP

                                 }).ToList();

                foreach (var item in cartdto)
                {

                    yield return new CartLevelMstDTO
                    {
                        Id = item.id,
                        LevelGroupId = item.LevelGroupId,
                        LevelGroupName = item.LevelGroupName,
                        LevelGroupType = item.LevelGroupType,
                        LevelId = item.LevelId,
                        LevelName = item.LevelName,
                        LevelNameNew = item.LevelNamenew,
                        LevelType = item.LevelType
                    };
                }
            }
        }



        [System.Web.Http.Route("api/grid/GridRowData")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GridRowData(CartLevelMstDTO model)
        {
            //Int64 id = Convert.ToInt64(model.Id);
            Int64 levelgid = Convert.ToInt64(model.LevelGroupId);
            Int64 levelid = Convert.ToInt64(model.LevelId);

            if (ModelState.IsValid)
            {
                var duplicate_data_test =
                    (from n in db.KART_LEVEL where n.LEVELGID == levelgid && n.LEVELID == levelid select n).ToList()
                        .Count();
                if (duplicate_data_test == 0)
                {
                    CartLevel ct = new CartLevel();
                    ct.LEVELGID = model.LevelGroupId;
                    ct.LEVELGTP = model.LevelGroupType;
                    ct.LEVELID = model.LevelId;
                    ct.LEVELNM = model.LevelNameNew;
                    ct.LEVELTP = model.LevelType;

                    db.KART_LEVEL.Add(ct);
                    db.SaveChanges();

                    var dd = from t in db.KART_LEVEL
                             where t.LEVELGID == levelgid && t.LEVELID == levelid
                             select new
                             {

                                 LevelGroupId = t.LEVELGID,
                                 Id = t.ID,
                                 LevelType = t.LEVELTP,
                                 LevelId = t.LEVELID,
                                 LevelName = model.LevelName,
                                 LevelNameNew = t.LEVELNM

                             };

                    foreach (var item in dd)
                    {
                        model.LevelGroupId = item.LevelGroupId;
                        model.Id = item.Id;
                        model.LevelType = item.LevelType;
                        model.LevelId = item.LevelId;
                        model.LevelName = item.LevelName;
                        model.LevelNameNew = item.LevelNameNew;
                    }

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }
                else
                {
                    model.LevelId = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }

               
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [System.Web.Http.Route("api/ApiLevelEntry/DeleteData")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteData(CartLevelMstDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebCartDbContext"].ToString());
            string query = string.Format("DELETE FROM KART_LEVEL WHERE ID='{0}'", model.Id);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            cmd.ExecuteNonQuery();

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            conn.Close();
            CartLevel cartSpec = new CartLevel();

            
            return Request.CreateResponse(HttpStatusCode.OK, cartSpec);
        }


        [System.Web.Http.Route("api/ApiLevelEntry/SaveData")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SaveData(CartLevelMstDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.KART_LEVEL where n.LEVELGID == model.LevelGroupId && n.LEVELID == model.LevelId select n).ToList();

            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.Id)
                {
                    //CartLevel ct = new CartLevel();
                    var data_find = (from n in db.KART_LEVEL where n.ID == model.Id select n).ToList();
                    foreach (var ct in data_find)
                    {
                        ct.ID = model.Id;
                        ct.LEVELGID = model.LevelGroupId;
                        ct.LEVELGTP = model.LevelGroupType;
                        ct.LEVELID = model.LevelId;

                        ct.LEVELNM = model.LevelNameNew;
                        ct.LEVELTP = model.LevelType;
                    }
                    


                    //db.Entry(ct).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                    }

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }
                else
                {
                    model.LevelId = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }
            }
            else
            {
                CartLevel ct = new CartLevel();
                ct.ID = model.Id;
                ct.LEVELGID = model.LevelGroupId;
                ct.LEVELGTP = model.LevelGroupType;
                ct.LEVELID = model.LevelId;

                ct.LEVELNM = model.LevelNameNew;
                ct.LEVELTP = model.LevelType;


                db.Entry(ct).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }
           
        }






        [System.Web.Http.Route("api/CategoryWiseLevel")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage CategoryWiseLevel(string query = "")
        {
            //using (var db = new AslWebCartContext())
            //{
            //    var siteLst = db.KART_CATEGORY.Where(y => y.LEVELGR == "LAST")
            //              .Select(x => x.CATID);

            //    return String.IsNullOrEmpty(query) ? db.KART_CATEGORY.AsEnumerable().Select(b => new CartLevelMstDTO { }).ToList() :
            //    db.KART_CATEGORY.Where(e => !siteLst.Contains(e.CATID) && e.CATNM.Contains(query)).Select(
            //              x =>
            //              new
            //              {
            //                  filtergroupname = x.CATNM,
            //                  filtergroupid = x.CATID,
            //              })
            //    .AsEnumerable().Select(a => new CartLevelMstDTO
            //    {
            //        LevelGroupName = a.filtergroupname,
            //        LevelGroupId = Convert.ToInt64(a.filtergroupid)
            //    }).ToList();
            //}
            using (var db = new AslWebCartContext())
            {

                var data_fetch = (from n in db.KART_CATEGORY where n.LEVELGR != "LAST" && n.CATNM.StartsWith(query) select new { n.CATNM, n.CATID }).ToList();
                List<SelectListItem> valuehold = new List<SelectListItem>();
                foreach (var x in data_fetch)
                {
                    valuehold.Add(new SelectListItem { Text = x.CATNM, Value = Convert.ToString(x.CATID) });
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, valuehold);
                return response;
            }
        }


        [System.Web.Http.Route("api/CategoryWiseLevelLast")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartLevelMstDTO> CategoryWiseLevelLast(string query = "")
        {
            using (var db = new AslWebCartContext())
            {
                var siteLst = db.KART_CATEGORY.Where(y => y.LEVELGR == "FIRST")
                          .Select(x => x.CATID);

                return String.IsNullOrEmpty(query) ? db.KART_CATEGORY.AsEnumerable().Select(b => new CartLevelMstDTO { }).ToList() :
                db.KART_CATEGORY.Where(e => !siteLst.Contains(e.CATID) && e.CATNM.Contains(query)).Select(
                          x =>
                          new
                          {
                              filtergroupname = x.CATNM,
                              filtergroupid = x.CATID,
                          })
                .AsEnumerable().Select(a => new CartLevelMstDTO
                {
                    LevelGroupName = a.filtergroupname,
                    LevelGroupId = Convert.ToInt64(a.filtergroupid)
                }).ToList();
            }
        }




        [System.Web.Http.Route("api/ItemsingleWiseLevel")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartLevelMstDTO> ItemsingleWiseLevel(string query = "")
        {
            using (var db = new AslWebCartContext())
            {
                var siteLst = db.KART_ITEM.Where(y => y.STOCKTP == "GROUP")
                          .Select(x => x.ITEMID);

                return String.IsNullOrEmpty(query) ? db.KART_ITEM.AsEnumerable().Select(b => new CartLevelMstDTO { }).ToList() :
                db.KART_ITEM.Where(p => !siteLst.Contains(p.ITEMID) && p.ITEMNM.Contains(query)).Select(
                          x =>
                          new
                          {
                              filtergroupname = x.ITEMNM,
                              filtergroupid = x.ITEMID,
                          })
                .AsEnumerable().Select(a => new CartLevelMstDTO
                {
                    LevelGroupName = a.filtergroupname,
                    LevelGroupId = Convert.ToInt64(a.filtergroupid)
                }).ToList();
            }
        }

        [System.Web.Http.Route("api/grid/Item")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Item(string LevelType, string LevelName)
        {
            using (var db = new AslWebCartContext())
            {
                //  List<string> data_fetch;
                List<string> stringList = new List<string>();

                if (LevelType == "CATEGORY")
                {
                    var data_fetch = (from n in db.KART_CATEGORY where n.LEVELGR == "LAST" && n.CATNM.StartsWith(LevelName) select n.CATNM).ToList();

                    foreach (var it in data_fetch)
                    {
                        stringList.Add(it);
                    }
                    string[] stringArray = stringList.ToArray();           

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, stringArray);
                    return response;

                }
                else if (LevelType == "ITEM-GROUP")
                {
                    var data_fetch = (from n in db.KART_ITEM where n.STOCKTP == "GROUP" && n.ITEMNM.StartsWith(LevelName) select n.ITEMNM).ToList();
                   // string s = string.Join("','", stringList.Select(i => i.Replace("'", "''")).ToArray());

                    foreach (var it in data_fetch)
                    {
                        stringList.Add(it);
                    }
                    string[] stringArray = stringList.ToArray();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, stringArray);
                    return response;
                }
                else if (LevelType == "ITEM-SINGLE")
                {
                    var data_fetch = (from n in db.KART_ITEM where n.STOCKTP == "SINGLE" && n.ITEMNM.StartsWith(LevelName) select n.ITEMNM).ToList();
                   // string s = string.Join("','", stringList.Select(i => i.Replace("'", "''")).ToArray());

                    foreach (var it in data_fetch)
                    {
                        stringList.Add(it);
                    }
                    string[] stringArray = stringList.ToArray();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, stringArray);
                    return response;
                }
                return null;
                //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, data_fetch);
                //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));



            }
        }


        [System.Web.Http.Route("api/grid/LevelIdCatch")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage LevelIdCatch(string LevelType, string LevelName)
        {
            using (var db = new AslWebCartContext())
            {

                List<Int64> idfind = new List<Int64>();

                if (LevelType == "CATEGORY")
                {
                    var data_fetch = (from n in db.KART_CATEGORY where n.LEVELGR == "LAST" && n.CATNM==LevelName select Convert.ToInt64(n.CATID)).ToList();

                    foreach (var it in data_fetch)
                    {
                        idfind.Add(it);
                    }
                    Int64[] intArray = idfind.ToArray();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, intArray);
                    return response;

                }
                else if (LevelType == "ITEM-GROUP")
                {
                    var data_fetch = (from n in db.KART_ITEM where n.STOCKTP == "GROUP" && n.ITEMNM==LevelName select Convert.ToInt64(n.ITEMID)).ToList();
                  

                    foreach (var it in data_fetch)
                    {
                        idfind.Add(it);
                    }
                    Int64[] intArray = idfind.ToArray();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, intArray);
                    return response;
                }
                else if (LevelType == "ITEM-SINGLE")
                {
                    var data_fetch = (from n in db.KART_ITEM where n.STOCKTP == "SINGLE" && n.ITEMNM==LevelName select Convert.ToInt64(n.ITEMID)).ToList();
                    // string s = string.Join("','", stringList.Select(i => i.Replace("'", "''")).ToArray());

                    foreach (var it in data_fetch)
                    {
                        idfind.Add(it);
                    }
                    Int64[] intArray = idfind.ToArray();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, intArray);
                    return response;
                }
                return null;
                //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, data_fetch);
                //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));



            }
        }

        [System.Web.Http.Route("api/ItemWiseLevel")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage ItemWise(string query)
        {
            using (var db = new AslWebCartContext())
            {

                var data_fetch = (from n in db.KART_ITEM where n.STOCKTP == "GROUP" && n.ITEMNM.StartsWith(query) select new { n.ITEMNM, n.ITEMID }).ToList();
                List<SelectListItem> valuehold = new List<SelectListItem>();
                foreach (var x in data_fetch)
                {
                    valuehold.Add(new SelectListItem { Text = x.ITEMNM, Value = Convert.ToString(x.ITEMID) });
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, valuehold);
                return response;
            }
            
        }

        [System.Web.Http.Route("api/test")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage abcD(string query)
        {
            using (var db = new AslWebCartContext())
            {

                var data_fetch = (from n in db.KART_CATEGORY where n.LEVELGR != "FIRST" && n.CATNM.StartsWith(query) select new { n.CATNM, n.CATID }).ToList();
                List<SelectListItem> valuehold = new List<SelectListItem>();
                foreach (var x in data_fetch)
                {
                    valuehold.Add(new SelectListItem { Text = x.CATNM, Value = Convert.ToString(x.CATID) });
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, valuehold);
                return response;
            }

        }


        [System.Web.Http.Route("api/ItemsingleWiseLevel")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage ItemsingleWise(string query)
        {
            using (var db = new AslWebCartContext())
            {

                var data_fetch = (from n in db.KART_ITEM where n.STOCKTP == "SINGLE" && n.ITEMNM.StartsWith(query) select new { n.ITEMNM, n.ITEMID }).ToList();
                List<SelectListItem> valuehold = new List<SelectListItem>();
                foreach (var x in data_fetch)
                {
                    valuehold.Add(new SelectListItem { Text = x.ITEMNM, Value = Convert.ToString(x.ITEMID) });
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, valuehold);
                return response;
            }

        }

        [System.Web.Http.Route("api/grid/toggleEdit")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartLevelMstDTO> toggleDataTransfer(Int64 PID)
        {
            using (var db = new AslWebCartContext())
            {
               

                return db.KART_LEVEL.Where(e =>e.ID==PID).Select(
                          x =>
                          new
                          {
                              Levelid = x.LEVELID,
                              
                          })
                .AsEnumerable().Select(a => new CartLevelMstDTO
                {
                    LevelId = a.Levelid
                    
                }).ToList();
            }
        }


        [System.Web.Http.Route("api/DynamicautocompleteLevel")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartLevelMstDTO> Dynamicautocomplete(string changedText, string changedText2)
        {
            using (var db = new AslWebCartContext())
            {

                String name = "";

                if (changedText2 == "CATEGORY")
                {
                    var rt =
                        db.KART_CATEGORY.Where(n => n.CATNM.StartsWith(changedText) && n.LEVELGR != "LAST")
                            .Select(n => new
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
                                    categoryid = x.CATID,

                                })
                            .AsEnumerable().Select(a => new CartLevelMstDTO
                            {
                                LevelGroupName = a.categoryname,
                                LevelGroupId = Convert.ToInt64(a.categoryid),

                            }).ToList();
                    }
                    else
                    {
                        return db.KART_CATEGORY.AsEnumerable().Select(a => new CartLevelMstDTO
                        {
                            LevelGroupId = 0,
                            LevelGroupName = name

                        }).ToList();
                    }


                }

                else if (changedText2 == "ITEM-GROUP")
                {
                    var rt =
                        db.KART_ITEM.Where(n => n.ITEMNM.StartsWith(changedText) && n.STOCKTP == "GROUP")
                            .Select(n => new
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

                    var findid = (from n in db.KART_ITEM where n.ITEMNM == name && n.STOCKTP == "GROUP" select n).ToList();
                    if (findid.Count != 0)
                    {


                        return db.KART_ITEM.Where(p => p.ITEMNM == name).Select(
                            x =>
                                new
                                {
                                    itemname = x.ITEMNM,
                                    itemid = x.ITEMID,

                                })
                            .AsEnumerable().Select(a => new CartLevelMstDTO
                            {
                                LevelGroupName = a.itemname,
                                LevelGroupId = Convert.ToInt64(a.itemid),

                            }).ToList();
                    }
                    else
                    {
                        return db.KART_ITEM.AsEnumerable().Select(a => new CartLevelMstDTO
                        {
                            LevelGroupId = 0,
                            LevelGroupName = name

                        }).ToList();
                    }


                }
                else
                {
                    return null;
                }


             }
        }


    }
}
