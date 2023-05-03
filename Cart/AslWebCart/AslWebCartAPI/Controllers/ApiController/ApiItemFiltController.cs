using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;

namespace AslWebCartAPI.Controllers
{
    public class ApiItemFiltController : ApiController
    {
        private AslWebCartContext db = new AslWebCartContext();

        [HttpGet]
        public IEnumerable<CartItemFiltDTO> Get(string ItemName, string ItemId)
        {

            Int64 itemId = Convert.ToInt64(ItemId);


            var ache_kina_data = (from n in db.KART_ITEMFILT where n.ITEMID == itemId select n).ToList();

            if (ache_kina_data.Count == 0)
            {

                yield return new CartItemFiltDTO
                    {

                        ItemId = itemId,
                        ItemName = ItemName,
                        FilterId = 0


                    };






            }
            else
            {



                var itemFilt = (from t1 in db.KART_ITEMFILT

                                where t1.ITEMID == itemId
                                select new
                                {
                                    Id = t1.ID,
                                    ItemId = t1.ITEMID,
                                    FilterId = t1.FILTERID,
                                    FilterGroupId = t1.FILTERGID,



                                });

                var filtername_find = (from n in db.KART_FILTER select n).ToList();
                var find_filterGroupname = (from n in db.KART_FILTERMST select n).ToList();
                string filtername = "", filterGroupname = "";

                foreach (var item in itemFilt)
                {
                    foreach (var x in filtername_find)
                    {
                        if (x.FILTERID == item.FilterId)
                        {
                            filtername = x.FILTERNM;
                            break;
                        }
                    }
                    foreach (var y in find_filterGroupname)
                    {
                        if (y.FILTERGID == item.FilterGroupId)
                        {
                            filterGroupname = y.FILTERGNM;
                            break;
                        }
                    }
                    yield return new CartItemFiltDTO
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        ItemName = ItemName,
                        FilterId = item.FilterId,
                        FilterName = filtername,
                        FilterGroupName = filterGroupname,
                        FilterGroupid = Convert.ToInt64(item.FilterGroupId)

                    };
                }




            }

        }

        [Route("api/FilterGroupNameSearch")]
        [HttpGet]
        public IEnumerable<CartItemFiltDTO> GetFilterGList(string query = "")
        {
            using (var db = new AslWebCartContext())
            {
                return String.IsNullOrEmpty(query) ? db.KART_FILTERMST.AsEnumerable().Select(b => new CartItemFiltDTO { }).ToList() :
                db.KART_FILTERMST.Where(p => p.FILTERGNM.StartsWith(query)).Select(
                          x =>
                          new
                          {
                              FilterGname = x.FILTERGNM,
                              FilterGid = x.FILTERGID,
                          })
                .AsEnumerable().Select(a => new CartItemFiltDTO
                {
                    FilterGroupName = a.FilterGname,
                    FilterGroupid = Convert.ToInt64(a.FilterGid)
                }).ToList();
            }
        }

        [System.Web.Http.Route("api/DynamicAutoFiltGName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartItemFiltDTO> Dynamicautocomplete(string changedText)
        {
            using (var db = new AslWebCartContext())
            {

                String name = "";

                var rt = db.KART_FILTERMST.Where(n => n.FILTERGNM.StartsWith(changedText)).Select(n => new
                {
                    headname = n.FILTERGNM

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



                var findid = (from n in db.KART_FILTERMST where n.FILTERGNM == name select n).ToList();
                if (findid.Count != 0)
                {
                    //Int64 filtergid = 0;
                    //foreach (var x in findid)
                    //{
                    //    filtergid = Convert.ToInt64(x.FILTERGID);

                    //}
                    //var find_groupname = (from n in db.KART_FILTERMST where n.FILTERGID == filtergid select n).ToList();
                    //string filtergname = "";
                    //foreach (var x in find_groupname)
                    //{
                    //    filtergname = x.FILTERGNM;
                    //}

                    return db.KART_FILTERMST.Where(p => p.FILTERGNM == name).Select(
                        x =>
                            new
                            {
                                Filtergname = x.FILTERGNM,
                                Filtergid = x.FILTERGID,
                               
                            })
                        .AsEnumerable().Select(a => new CartItemFiltDTO
                        {
                           
                            FilterGroupid = Convert.ToInt64(a.Filtergid),
                            FilterGroupName = a.Filtergname
                        }).ToList();
                }
                else
                {
                    return db.KART_FILTERMST.AsEnumerable().Select(a => new CartItemFiltDTO
                    {
                        FilterGroupid = 0,
                        FilterGroupName = name

                    }).ToList();
                }



            }

        }

        [Route("api/FilterNameSearch")]
        [HttpGet]
        public IEnumerable<CartItemFiltDTO> GetFilterList(string query,string query2)
        {
            using (var db = new AslWebCartContext())
            {
                Int64 filtergid = Convert.ToInt64(query2);

                return String.IsNullOrEmpty(query) ? db.KART_FILTER.AsEnumerable().Select(b => new CartItemFiltDTO { }).ToList() :
                db.KART_FILTER.Where(p => p.FILTERNM.StartsWith(query) && p.FILTERGID==filtergid).Select(
                          x =>
                          new
                          {
                              Filtername = x.FILTERNM,
                              Filterid = x.FILTERID,
                          })
                .AsEnumerable().Select(a => new CartItemFiltDTO
                {
                    FilterName = a.Filtername,
                    FilterId = Convert.ToInt64(a.Filterid)
                }).ToList();
            }
        }


        [Route("api/FilterGNameSearch")]
        [HttpGet]
        public IEnumerable<CartItemFiltDTO> GetFilterGridGList(string query)
        {
            using (var db = new AslWebCartContext())
            {
               
                return String.IsNullOrEmpty(query) ? db.KART_FILTERMST.AsEnumerable().Select(b => new CartItemFiltDTO { }).ToList() :
                db.KART_FILTERMST.Where(p => p.FILTERGNM.StartsWith(query)).Select(
                          x =>
                          new
                          {
                              FilterGname = x.FILTERGNM,
                              FilterGid = x.FILTERGID,
                          })
                .AsEnumerable().Select(a => new CartItemFiltDTO
                {
                    FilterGroupName = a.FilterGname,
                    FilterGroupid = Convert.ToInt64(a.FilterGid)
                }).ToList();
            }
        }


        [System.Web.Http.Route("api/DynamicAutoFiltName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartItemFiltDTO> DynamicAutoFiltName(string changedText, string changedText2)
        {
            using (var db = new AslWebCartContext())
            {

                String name = "";
                Int64 filtergid = Convert.ToInt64(changedText2);
                var rt = db.KART_FILTER.Where(n => n.FILTERNM.StartsWith(changedText) && n.FILTERGID==filtergid).Select(n => new
                {
                    headname = n.FILTERNM

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



                var findid = (from n in db.KART_FILTER where n.FILTERNM == name && n.FILTERGID==filtergid select n).ToList();
                if (findid.Count != 0)
                {
                    

                    return db.KART_FILTER.Where(p => p.FILTERNM == name && p.FILTERGID==filtergid).Select(
                        x =>
                            new
                            {
                                Filtername = x.FILTERNM,
                                Filterid = x.FILTERID,

                            })
                        .AsEnumerable().Select(a => new CartItemFiltDTO
                        {

                            FilterId = Convert.ToInt64(a.Filterid),
                            FilterName = a.Filtername
                        }).ToList();
                }
                else
                {
                    return db.KART_FILTER.AsEnumerable().Select(a => new CartItemFiltDTO
                    {
                        FilterId = 0,
                        FilterName = name

                    }).ToList();
                }



            }

        }


        [Route("api/grid/addData")]
        [HttpPost]
        public HttpResponseMessage AddChildData(CartItemFiltDTO model)
        {

            var check_data = (from n in db.KART_ITEMFILT where n.ITEMID == model.ItemId && n.FILTERID == model.FilterId && n.FILTERGID == model.FilterGroupid select n).ToList();

            if (check_data.Count == 0)
            {




                CartItemFilt cartItemFilt = new CartItemFilt();

                cartItemFilt.ITEMID = model.ItemId;
                cartItemFilt.FILTERGID = model.FilterGroupid;
                cartItemFilt.FILTERID = model.FilterId;




                db.KART_ITEMFILT.Add(cartItemFilt);

                db.SaveChanges();

                model.Id = cartItemFilt.ID;
                model.ItemId = cartItemFilt.ITEMID;
                model.FilterGroupid = Convert.ToInt64(cartItemFilt.FILTERGID);
                model.FilterId = cartItemFilt.FILTERID;

                var find_groupname =
                    (from n in db.KART_FILTERMST where n.FILTERGID == model.FilterGroupid select n).ToList();
                foreach (var x in find_groupname)
                {
                    model.FilterGroupName = x.FILTERGNM;
                }
                var find_filtername =
                   (from n in db.KART_FILTER where n.FILTERID == model.FilterId select n).ToList();
                foreach (var x in find_filtername)
                {
                    model.FilterName = x.FILTERNM;
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));

                return response;
            }
            else
            {
                model.FilterId = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }


        }


        [System.Web.Http.Route("api/groupfilterName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartItemFiltDTO> filternameGroup(string changedText, string changedText2)
        {
            using (var db = new AslWebCartContext())
            {
                Int64 filterid = Convert.ToInt64(changedText2);

                var findid = (from n in db.KART_FILTER where n.FILTERNM == changedText && n.FILTERID == filterid select n).ToList();
                if (findid.Count != 0)
                {
                    Int64 groupid = 0;
                    foreach (var x in findid)
                    {
                        groupid = Convert.ToInt64(x.FILTERGID);
                    }

                    return db.KART_FILTERMST.Where(p => p.FILTERGID == groupid).Select(
                        x =>
                            new
                            {
                                Filtergroupname = x.FILTERGNM,
                                Filtergroupid = x.FILTERGID,

                            })
                        .AsEnumerable().Select(a => new CartItemFiltDTO
                        {
                            FilterGroupName = a.Filtergroupname,
                            FilterGroupid = Convert.ToInt64(a.Filtergroupid)

                        }).ToList();
                }
                else
                {
                    return db.KART_FILTERMST.AsEnumerable().Select(a => new CartItemFiltDTO
                    {
                        FilterGroupid = 0,
                        FilterGroupName = ""

                    }).ToList();
                }

                

            }

        }

        [System.Web.Http.Route("api/ApiItemFilt/SaveData")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SaveData(CartItemFiltDTO model)
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                   (from n in db.KART_ITEMFILT where n.ITEMID == model.ItemId && n.FILTERGID == model.FilterGroupid && n.FILTERID==model.FilterId select n).ToList();
             Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                 foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.Id)
                {
                    //CartItemFilt cartItemFilt = new CartItemFilt();
                    var data_find = (from n in db.KART_ITEMFILT where n.ID == model.Id select n).ToList();
                    foreach (var cartItemFilt in data_find)
                    {
                        cartItemFilt.ID = model.Id;
                        cartItemFilt.ITEMID = model.ItemId;
                        cartItemFilt.FILTERGID = model.FilterGroupid;
                        cartItemFilt.FILTERID = model.FilterId;

                    }

                   


                    //db.Entry(cartItemFilt).State = EntityState.Modified;

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
                    model.FilterId = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }
            }
            else
            {
                CartItemFilt cartItemFilt = new CartItemFilt();

                cartItemFilt.ID = model.Id;
                cartItemFilt.ITEMID = model.ItemId;
                cartItemFilt.FILTERGID = model.FilterGroupid;
                cartItemFilt.FILTERID = model.FilterId;


                db.Entry(cartItemFilt).State = EntityState.Modified;

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

        [Route("api/ApiItemFilt/DeleteData")]
        [HttpPost]
        public HttpResponseMessage DeleteData(CartItemFiltDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebCartDbContext"].ToString());
            string query = string.Format("DELETE FROM KART_ITEMFILT WHERE ID='{0}'", model.Id);
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
            CartItemFilt cartItemFilt = new CartItemFilt();






            return Request.CreateResponse(HttpStatusCode.OK, cartItemFilt);
        }

    }
}
