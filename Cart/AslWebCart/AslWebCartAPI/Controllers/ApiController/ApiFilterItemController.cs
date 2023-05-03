using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AslWebCartAPI.Controllers
{
    public class ApiFilterItemController : ApiController
    {
        private AslWebCartContext db = new AslWebCartContext();


        [Route("api/main/GetItemByLevelId")]
        [HttpGet]
        public IEnumerable<ItemDTO> GetItemByLevelId(string matchid)
        {
            Int64 mat = Convert.ToInt64(matchid);
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebCartDbContext"].ToString());
            string query = string.Format("SELECT LEVELID, LEVELNM, RATEBDT, RATEUSD FROM KART_LEVEL INNER JOIN KART_ITEM ON KART_ITEM.ITEMID = LEVELID " +
                                          "  WHERE LEVELGID = '" + mat + "' AND LEVELTP = 'ITEM-SINGLE' " +
                                          "  UNION " +
                                          "  SELECT KART_LEVEL.LEVELID, KART_LEVEL.LEVELNM, RATEBDT, RATEUSD FROM( " +
                                          "  SELECT LEVELID, LEVELNM FROM KART_LEVEL WHERE LEVELGID = '" + mat + "' AND LEVELTP = 'ITEM-GROUP' " +
                                          "  ) A INNER JOIN KART_LEVEL ON KART_LEVEL.LEVELGID = A.LEVELID " +
                                          "  INNER JOIN KART_ITEM ON KART_ITEM.ITEMID = KART_LEVEL.LEVELID");

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            // var cartCategoryItems = new List<MergeDTOs>();
            foreach (DataRow row in ds.Rows)
            {
                yield return new ItemDTO
                {
                    LevelId = Convert.ToInt64(row["LEVELID"]),
                    LevelName = row["LEVELNM"].ToString(),
                    RateBdt = Convert.ToDecimal(row["RATEBDT"].ToString()),
                    RateUsd = Convert.ToDecimal(row["RATEUSD"].ToString()),
                };
            }
            conn.Close();
        }

        [Route("api/main/GetItemFilterByLevelGid")]
        [HttpGet]
        public IEnumerable<ItemFilterDTO> GetItemFilterByLevelGid(string matchid)
        {
            Int64 levelgid = Convert.ToInt64(matchid);

            var cartdto = ((from t1 in db.KART_LEVEL
                            join t2 in db.KART_ITEMFILT
                            on t1.LEVELID equals t2.ITEMID
                            join t3 in db.KART_FILTERMST
                            on t2.FILTERGID equals t3.FILTERGID
                            where t1.LEVELGID == levelgid && t1.LEVELTP == "ITEM-SINGLE"
                            select new ItemFilterDTO
                            {
                                FilterGroupName = t3.FILTERGNM,
                                FilterGroupid = t3.FILTERGID
                            })
                              .Union((from t1 in db.KART_LEVEL
                                      join t2 in db.KART_LEVEL
                                      on t1.LEVELID equals t2.LEVELGID
                                      join t3 in db.KART_ITEMFILT
                                      on t2.LEVELID equals t3.ITEMID
                                      join t4 in db.KART_FILTERMST
                                      on t3.FILTERGID equals t4.FILTERGID
                                      where t1.LEVELGID == levelgid && t1.LEVELTP == "ITEM-GROUP"
                                      select new ItemFilterDTO
                                      {
                                          FilterGroupName = t4.FILTERGNM,
                                          FilterGroupid = t4.FILTERGID
                                      }))).ToList();


            var cartdto2 = (from st in cartdto
                            select new ItemFilterDTO
                            {
                                FilterGroupName = st.FilterGroupName,
                                FilterGroupid = st.FilterGroupid,
                                FilterDTO = (
                               (from t1 in db.KART_LEVEL
                                join t2 in db.KART_ITEMFILT
                                on t1.LEVELID equals t2.ITEMID
                                join t3 in db.KART_FILTER
                                on t2.FILTERID equals t3.FILTERID
                                where t1.LEVELGID == levelgid && t1.LEVELTP == "ITEM-SINGLE" && t2.FILTERGID == st.FilterGroupid
                                select new FilterDTO
                                {
                                    FilterName = t3.FILTERNM,
                                    FilterId = t3.FILTERID
                                })
                               .Union(from t1 in db.KART_LEVEL
                                      join t2 in db.KART_LEVEL
                                      on t1.LEVELID equals t2.LEVELGID
                                      join t3 in db.KART_ITEMFILT
                                      on t2.LEVELID equals t3.ITEMID
                                      join t4 in db.KART_FILTER
                                      on t3.FILTERID equals t4.FILTERID
                                      where t1.LEVELGID == levelgid && t1.LEVELTP == "ITEM-GROUP" && t3.FILTERGID == st.FilterGroupid
                                      select new FilterDTO
                                      {
                                          FilterName = t4.FILTERNM,
                                          FilterId = t4.FILTERID
                                      })).ToList()
                            });
            return cartdto2;
        }

        //[Route("api/main/JsonF")]
        //[HttpGet]
        //public IEnumerable<ItemFilterDTO> JsonF(string matchid)
        //{
        //    Int64 levelgid = Convert.ToInt64(matchid);
        //    var cartitemdto = (from t1 in db.KART_LEVEL
        //                       join t2 in db.KART_ITEMFILT
        //                       on t1.LEVELID equals t2.ITEMID
        //                       join t3 in db.KART_FILTERMST
        //                       on t2.FILTERGID equals t3.FILTERGID
        //                       where t1.LEVELGID == levelgid && t1.LEVELTP == "ITEM-SINGLE"
        //                       select new ItemFilterDTO
        //                       {
        //                           FilterGroupName = t3.FILTERGNM,
        //                           FilterGroupid = t3.FILTERGID,

        //                           FilterDTO = (
        //                           (from s1 in db.KART_LEVEL
        //                            join s2 in db.KART_ITEMFILT
        //                            on s1.LEVELID equals s2.ITEMID
        //                            join s3 in db.KART_FILTER
        //                            on s2.FILTERID equals s3.FILTERID
        //                            where s1.LEVELGID == levelgid && s1.LEVELTP == "ITEM-SINGLE" && s2.FILTERGID == t3.FILTERGID
        //                            select new FilterDTO
        //                            {
        //                                FilterName = s3.FILTERNM,
        //                                FilterId = s3.FILTERID
        //                            }
        //                            )
        //                               //.Union(from s1 in db.KART_LEVEL
        //                               //       join s2 in db.KART_LEVEL
        //                               //       on s1.LEVELID equals s2.LEVELGID
        //                               //       join s3 in db.KART_ITEMFILT
        //                               //       on s2.LEVELID equals s3.ITEMID
        //                               //       join s4 in db.KART_FILTER
        //                               //       on s3.FILTERID equals s4.FILTERID
        //                               //       where s1.LEVELGID == levelgid && s1.LEVELTP == "ITEM-GROUP" && s3.FILTERGID == t3.FILTERGID
        //                               //       select new FilterDTO
        //                               //       {
        //                               //           FilterName = s4.FILTERNM,
        //                               //           FilterId = s4.FILTERID
        //                               //       })
        //                            ).ToList()
        //                       });

        //    var cartitemdto2 = ((from t1 in db.KART_LEVEL
        //                         join t2 in db.KART_LEVEL
        //                         on t1.LEVELID equals t2.LEVELGID
        //                         join t3 in db.KART_ITEMFILT
        //                         on t2.LEVELID equals t3.ITEMID
        //                         join t4 in db.KART_FILTERMST
        //                         on t3.FILTERGID equals t4.FILTERGID
        //                         where t1.LEVELGID == levelgid && t1.LEVELTP == "ITEM-GROUP"
        //                         select new ItemFilterDTO
        //                         {
        //                             FilterGroupName = t4.FILTERGNM,
        //                             FilterGroupid = t4.FILTERGID,

        //                             FilterDTO = (
        //                                 //      (from s1 in db.KART_LEVEL
        //                                 //       join s2 in db.KART_ITEMFILT
        //                                 //       on s1.LEVELID equals s2.ITEMID
        //                                 //       join s3 in db.KART_FILTER
        //                                 //       on s2.FILTERID equals s3.FILTERID
        //                                 //       where s1.LEVELGID == levelgid && s1.LEVELTP == "ITEM-SINGLE" && s2.FILTERGID == t4.FILTERGID
        //                                 //       select new FilterDTO
        //                                 //       {
        //                                 //           FilterName = s3.FILTERNM,
        //                                 //           FilterId = s3.FILTERID
        //                                 //       }
        //                                 //       )
        //                                 //.Union
        //            (from s1 in db.KART_LEVEL
        //             join s2 in db.KART_LEVEL
        //             on s1.LEVELID equals s2.LEVELGID
        //             join s3 in db.KART_ITEMFILT
        //             on s2.LEVELID equals s3.ITEMID
        //             join s4 in db.KART_FILTER
        //             on s3.FILTERID equals s4.FILTERID
        //             where s1.LEVELGID == levelgid && s1.LEVELTP == "ITEM-GROUP" && s3.FILTERGID == t4.FILTERGID
        //             select new FilterDTO
        //             {
        //                 FilterName = s4.FILTERNM,
        //                 FilterId = s4.FILTERID
        //             }))
        //                         }));

        //    var nanan = cartitemdto.Union(cartitemdto2);

        //    return cartitemdto2;

        //}


        [Route("api/filtergrouplist")]
        [HttpGet]
        public IEnumerable<CartFilterMstDTO> GetFilterGroupList(string query = "")
        {
            using (var db = new AslWebCartContext())
            {
                return String.IsNullOrEmpty(query) ? db.KART_FILTERMST.AsEnumerable().Select(b => new CartFilterMstDTO { }).ToList() :
                db.KART_FILTERMST.Where(p => p.FILTERGNM.StartsWith(query)).Select(
                          x =>
                          new
                          {
                              filtergroupname = x.FILTERGNM,
                              filtergroupid = x.FILTERGID,
                          })
                .AsEnumerable().Select(a => new CartFilterMstDTO
                {
                    FilterGroupName = a.filtergroupname,
                    FilterGroupid = Convert.ToInt64(a.filtergroupid)
                }).ToList();
            }
        }



        [Route("api/main/GetItemByFilterId")]
        [HttpPost]
        public IEnumerable<ItemDTO> PostAlbum(string matchid, JObject jsonData)
        {
            string jData = jsonData.GetValue("jsonData").Value<string>();


            //string jData = "  [{ \"FilterId\": 301001 },{ \"FilterId\": 303001 },{ \"FilterId\": 303001 }] ";

            Int64 levelgid = Convert.ToInt64(matchid);
            var jArray = JArray.Parse(jData);
            int length = jArray.Count;

            Int64 firstItem = Convert.ToInt64(jArray[0]["FilterId"]);


            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebCartDbContext"].ToString());

            string query = string.Format("SELECT DISTINCT KART_LEVEL.LEVELID, KART_LEVEL.LEVELNM, RATEBDT, RATEUSD FROM( " +
                                       " SELECT LEVELID, LEVELNM FROM KART_LEVEL WHERE LEVELGID = '" + levelgid + "' " +
                                       " ) A LEFT OUTER JOIN KART_LEVEL ON (KART_LEVEL.LEVELGID = A.LEVELID OR KART_LEVEL.LEVELID = A.LEVELID) " +
                                       " INNER JOIN KART_ITEM	  ON KART_ITEM.ITEMID = KART_LEVEL.LEVELID " +
                                       " LEFT OUTER JOIN KART_ITEMFILT ON KART_ITEMFILT.ITEMID = KART_LEVEL.LEVELID " +
                                       " WHERE KART_ITEMFILT.FILTERID IN ('" + firstItem + "')");
            for (int i = 1; i <= length - 1; i++)
            {
                Int64 item = Convert.ToInt64(jArray[i]["FilterId"]);
                query = string.Format(" " + query + " INTERSECT SELECT DISTINCT KART_LEVEL.LEVELID, KART_LEVEL.LEVELNM, RATEBDT, RATEUSD FROM( " +
                                       " SELECT LEVELID, LEVELNM FROM KART_LEVEL WHERE LEVELGID = '" + levelgid + "' " +
                                       " ) A LEFT OUTER JOIN KART_LEVEL ON (KART_LEVEL.LEVELGID = A.LEVELID OR KART_LEVEL.LEVELID = A.LEVELID) " +
                                       " INNER JOIN KART_ITEM	  ON KART_ITEM.ITEMID = KART_LEVEL.LEVELID " +
                                       " LEFT OUTER JOIN KART_ITEMFILT ON KART_ITEMFILT.ITEMID = KART_LEVEL.LEVELID " +
                                       " WHERE KART_ITEMFILT.FILTERID IN ('" + item + "')");
            }


            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);

            // var cartCategoryItems = new List<MergeDTOs>();
            foreach (DataRow row in ds.Rows)
            {
                yield return new ItemDTO
                {
                    LevelId = Convert.ToInt64(row["LEVELID"]),
                    LevelName = (row["LEVELNM"]).ToString(),
                    RateBdt = Convert.ToInt64(row["RATEBDT"]),
                    RateUsd = Convert.ToInt64(row["RATEUSD"]),

                };
            }

            conn.Close();
        }


        [System.Web.Http.Route("api/ApiFilterItem/GetData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartFilterMstDTO> Get(string GroupName,string Groupid)
        {

              Int64 groupid = Convert.ToInt64(Groupid);
              var ache_kina_data = (from n in db.KART_FILTERMST where n.FILTERGNM == GroupName && n.FILTERGID == groupid select n).ToList();
            if(ache_kina_data.Count==0)
            {
                 var name_ache_kina = (from n in db.KART_FILTERMST where n.FILTERGNM == GroupName select n).ToList();

                if (name_ache_kina.Count == 0)
                {
                    Int64 max_idta_berkorbo = Convert.ToInt64((from n in db.KART_FILTERMST select n.FILTERGID).Max());

                    if (max_idta_berkorbo == 0)
                    {

                        CartFilterMst ord = new CartFilterMst
                        {
                            FILTERGID = 301,
                            FILTERGNM = GroupName

                        };

                        db.KART_FILTERMST.Add(ord);
                        db.SaveChanges();
                        yield return new CartFilterMstDTO
                        {
                           
                            FilterGroupid = 301,
                            FilterId = 0,
                            FilterGroupName = GroupName

                        };
                       

                    }
                    else
                    {
                        CartFilterMst ord = new CartFilterMst
                        {
                            FILTERGID = max_idta_berkorbo + 1,
                            FILTERGNM = GroupName

                        };

                        db.KART_FILTERMST.Add(ord);
                        db.SaveChanges();
                        yield return new CartFilterMstDTO
                        {

                            FilterGroupid = max_idta_berkorbo + 1,
                            FilterId = 0,
                            FilterGroupName = GroupName

                        };
                       


                    }
                }

               
            }
            else
            {
                var ff = (from n in db.KART_FILTER where n.FILTERGID == groupid select n).ToList();

                if (ff.Count == 0)
                {

                    yield return new CartFilterMstDTO
                    {
                       
                        FilterGroupid = groupid,
                        FilterId = 0,
                        FilterGroupName = GroupName
                       

                    };

                }
                else
                {



                    var filtermstDto = (from t1 in db.KART_FILTER
                                        //join t2 in db.KART_CATEGORY on t1.LEVELGID equals t2.CATID
                                        where t1.FILTERGID == groupid
                                        select new
                                        {
                                            Id = t1.ID,
                                            FilterGroupid = t1.FILTERGID,
                                            FilterId = t1.FILTERID,
                                            FilterName = t1.FILTERNM,
                                            FilterNumericId = t1.FILTERNID,
                                            FilterShortId = t1.FILTERSID


                                        });
                    foreach (var item in filtermstDto)
                    {
                        yield return new CartFilterMstDTO
                        {
                            Id = item.Id,
                            FilterGroupid = Convert.ToInt64(item.FilterGroupid),
                             FilterGroupName = GroupName,
                            FilterId = Convert.ToInt64(item.FilterId),
                            FilterName = item.FilterName,
                            FilterNumericId = Convert.ToInt64(item.FilterNumericId),
                            FilterShortId = item.FilterShortId
                        };
                    }



                }
            }
             


            




         

        }


        [Route("api/ApiFilterItem/SaveData")]
        [HttpPost]
        public HttpResponseMessage SaveData(CartFilterMstDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var duplicate_data_test =
                    (from n in db.KART_FILTER where n.FILTERGID == model.FilterGroupid && n.FILTERNM == model.FilterName select n).ToList();
            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.Id)
                {
                    //CartFilter cartFilter = new CartFilter();
                    var data_find = (from n in db.KART_FILTER where n.ID == model.Id select n).ToList();
                    foreach (var cartFilter in data_find)
                    {
                        cartFilter.ID = model.Id;
                        cartFilter.FILTERGID = model.FilterGroupid;
                        cartFilter.FILTERID = model.FilterId;
                        cartFilter.FILTERNM = model.FilterName;
                        cartFilter.FILTERNID = Convert.ToInt64(model.FilterNumericId);
                        cartFilter.FILTERSID = model.FilterShortId;

                    }
                   

                    //db.Entry(cartFilter).State = EntityState.Modified;

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
                CartFilter cartFilter = new CartFilter();
                cartFilter.ID = model.Id;
                cartFilter.FILTERGID = model.FilterGroupid;
                cartFilter.FILTERID = model.FilterId;
                cartFilter.FILTERNM = model.FilterName;
                cartFilter.FILTERNID = Convert.ToInt64(model.FilterNumericId);
                cartFilter.FILTERSID = model.FilterShortId;

                db.Entry(cartFilter).State = EntityState.Modified;

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






      
        [Route("api/grid/FilterChild")]
        [HttpPost]
        public HttpResponseMessage AddChildData(CartFilterMstDTO model)
        {

            var check_data = (from n in db.KART_FILTER where n.FILTERGID == model.FilterGroupid && n.FILTERNM == model.FilterName select n).ToList();
            if(check_data.Count==0)
            {
                var ache_kina_data = (from n in db.KART_FILTER where n.FILTERGID == model.FilterGroupid select n).ToList();

                if (ache_kina_data.Count == 0)
                {


                    CartFilter cartFilter = new CartFilter();
                    cartFilter.FILTERGID = model.FilterGroupid;
                    cartFilter.FILTERID = Convert.ToInt64(Convert.ToString(model.FilterGroupid) + "001");
                    cartFilter.FILTERNM = model.FilterName;
                    cartFilter.FILTERNID = model.FilterNumericId;
                    cartFilter.FILTERSID = model.FilterShortId;



                    db.KART_FILTER.Add(cartFilter);

                    db.SaveChanges();
                    model.Id = cartFilter.ID;
                    model.FilterGroupid = Convert.ToInt64(cartFilter.FILTERGID);
                    model.FilterId = Convert.ToInt64(cartFilter.FILTERID);
                    model.FilterName = cartFilter.FILTERNM;
                    model.FilterNumericId = Convert.ToInt64(cartFilter.FILTERNID);
                    model.FilterShortId = cartFilter.FILTERSID;
                }
                else
                {
                    Int64 max_idta_berkorbo = Convert.ToInt64((from n in db.KART_FILTER where n.FILTERGID == model.FilterGroupid select n.FILTERID).Max());
                    CartFilter cartFilter = new CartFilter();
                    cartFilter.FILTERGID = model.FilterGroupid;
                    cartFilter.FILTERID = max_idta_berkorbo + 1;
                    cartFilter.FILTERNM = model.FilterName;
                    cartFilter.FILTERNID = model.FilterNumericId;
                    cartFilter.FILTERSID = model.FilterShortId;



                    db.KART_FILTER.Add(cartFilter);
                    db.SaveChanges();
                    model.Id = cartFilter.ID;
                    model.FilterGroupid = Convert.ToInt64(cartFilter.FILTERGID);
                    model.FilterId = Convert.ToInt64(cartFilter.FILTERID);
                    model.FilterName = cartFilter.FILTERNM;
                    model.FilterNumericId = Convert.ToInt64(cartFilter.FILTERNID);
                    model.FilterShortId = cartFilter.FILTERSID;
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

        [Route("api/ApiFilterItem/DeleteData")]
        [HttpPost]
        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteData(CartFilterMstDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebCartDbContext"].ToString());
            string query = string.Format("DELETE FROM KART_FILTER WHERE ID='{0}'",model.Id);
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
            CartFilter cartfilter = new CartFilter();






            return Request.CreateResponse(HttpStatusCode.OK, cartfilter);
        }

      

        [System.Web.Http.Route("api/DynamicautocompleteFilter")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartFilterMstDTO> Dynamicautocomplete(string changedText)
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
                    return db.KART_FILTERMST.Where(p => p.FILTERGNM == name).Select(
                        x =>
                            new
                            {
                                filtergname = x.FILTERGNM,
                                filtergid = x.FILTERGID,

                            })
                        .AsEnumerable().Select(a => new CartFilterMstDTO
                        {
                            FilterGroupName = a.filtergname,
                            FilterGroupid = Convert.ToInt64(a.filtergid)

                        }).ToList();
                }
                else
                {
                    return db.KART_FILTERMST.AsEnumerable().Select(a => new CartFilterMstDTO
                    {
                        FilterGroupName = name,
                        FilterGroupid = 0

                    }).ToList();
                }



            }

        }
    }
}



