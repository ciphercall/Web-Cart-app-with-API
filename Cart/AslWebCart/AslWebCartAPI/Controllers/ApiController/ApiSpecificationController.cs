using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AslWebCartAPI.Models;
using AslWebCartAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AslWebCartAPI.Controllers
{
    public class ApiSpecificationController : ApiController
    {
        private AslWebCartContext db = new AslWebCartContext();


        [Route("api/main/GetSpecificationByItemID")]
        [HttpGet]
        public IEnumerable<CartSpecMstDTO> GetSpecificationByItemID(string matchid)
        {
            Int64 itemid = Convert.ToInt64(matchid);


            var some = (from t1 in db.KART_SPECMST
                        where t1.ITEMID == itemid
                        select new CartSpecMstDTO
                        {
                            SpecificationName = t1.SPECNM,
                            SpecificationSL = t1.SPECSL,
                            Spec = (from s1 in db.KART_SPECMST
                                    join s2 in db.KART_SPEC on new { item = s1.ITEMID, specsl = s1.SPECSL } equals new { item = s2.ITEMID, specsl = s2.SPECSL }
                                    where s1.ITEMID == itemid && s1.SPECSL == t1.SPECSL
                                    select new CartSpecificationDTO
                                    {
                                        FEATTP = s2.FEATTP,
                                        FEATURE = s2.FEATURE
                                    }
                                      )
                        }
                              );

            return some;
            //return null;
        }

        [Route("api/SpecItemSearch")]
        [HttpGet]
        public IEnumerable<CartSpecMstDTO> GetItemList(string query = "")
        {
            using (var db = new AslWebCartContext())
            {
                return String.IsNullOrEmpty(query) ? db.KART_ITEM.AsEnumerable().Select(b => new CartSpecMstDTO { }).ToList() :
                db.KART_ITEM.Where(p => p.ITEMNM.StartsWith(query)).Select(
                          x =>
                          new
                          {
                              ItemName = x.ITEMNM,
                              ItemId = x.ITEMID,
                          })
                .AsEnumerable().Select(a => new CartSpecMstDTO
                {
                    ItemName = a.ItemName,
                    ItemId =Convert.ToInt64(a.ItemId)
                }).ToList();
            }
        }


        [Route("api/DynamicAutoSpecItem")]
        [HttpGet]
        public IEnumerable<CartSpecMstDTO> Dynamicautocomplete(string changedText)
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



                var findid = (from n in db.KART_ITEM where n.ITEMNM == name select n).ToList();
                if (findid.Count != 0)
                {
                    return db.KART_ITEM.Where(p => p.ITEMNM == name).Select(
                        x =>
                            new
                            {
                                ItemName = x.ITEMNM,
                                ItemId = x.ITEMID,

                            })
                        .AsEnumerable().Select(a => new CartSpecMstDTO
                        {
                            ItemId = Convert.ToInt64(a.ItemId),
                            ItemName = a.ItemName

                        }).ToList();
                }
                else
                {
                    return db.KART_ITEM.AsEnumerable().Select(a => new CartSpecMstDTO
                    {
                        ItemId = 0,
                        ItemName = name

                    }).ToList();
                }



            }

        }


        [Route("api/SpecNameSearch")]
        [HttpGet]
        public IEnumerable<CartSpecMstDTO> GetSpecNameList(string query, string ItemId)
        {
            using (var db = new AslWebCartContext())
            {
                Int64 itemId = Convert.ToInt64(ItemId);
                return String.IsNullOrEmpty(query) ? db.KART_SPECMST.AsEnumerable().Select(b => new CartSpecMstDTO { }).ToList() :
                db.KART_SPECMST.Where(p => p.SPECNM.StartsWith(query) && p.ITEMID == itemId).Select(
                          x =>
                          new
                          {
                              SpecificationName = x.SPECNM,
                              SpecificationSl = x.SPECSL,
                          })
                .AsEnumerable().Select(a => new CartSpecMstDTO
                {
                    SpecificationName = a.SpecificationName,
                    SpecificationSL = a.SpecificationSl
                }).ToList();
            }
        }


        [System.Web.Http.Route("api/DynamicAutoSpecName")]
        [System.Web.Http.HttpGet]
        public IEnumerable<CartSpecMstDTO> DynamicSpecName(string changedText, string changedText2)
        {
            using (var db = new AslWebCartContext())
            {

                String name = "";
                Int64 itemId = Convert.ToInt64(changedText2);

                var rt = db.KART_SPECMST.Where(n => n.SPECNM.StartsWith(changedText) && n.ITEMID == itemId).Select(n => new
                {
                    headname = n.SPECNM

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



                var findid = (from n in db.KART_SPECMST where n.SPECNM == name && n.ITEMID == itemId select n).ToList();
                if (findid.Count != 0)
                {
                    return db.KART_SPECMST.Where(p => p.SPECNM == name && p.ITEMID == itemId).Select(
                        x =>
                            new
                            {
                                SpecificationName = x.SPECNM,
                                SpecificationSL = x.SPECSL,

                            })
                        .AsEnumerable().Select(a => new CartSpecMstDTO
                        {
                            SpecificationSL = a.SpecificationSL,
                            SpecificationName = a.SpecificationName

                        }).ToList();
                }
                else
                {
                    return db.KART_SPECMST.AsEnumerable().Select(a => new CartSpecMstDTO
                    {
                        SpecificationSL = 0,
                        SpecificationName = name

                    }).ToList();
                }



            }

        }


        [HttpGet]
        public IEnumerable<CartSpecMstDTO> Get(string ItemName, string ItemId, string SpecificationSL, string SpecificationName, string ViewSL)
        {

            Int64 itemId = Convert.ToInt64(ItemId);
            Int64 specificationSl = Convert.ToInt64(SpecificationSL);
            Int64 viewSl = Convert.ToInt64(ViewSL);

            var ache_kina_data = (from n in db.KART_SPECMST where n.ITEMID == itemId && n.SPECSL == specificationSl && n.SPECNM == SpecificationName select n).ToList();

            if (ache_kina_data.Count == 0)
            {
                var item_ache_kina = (from n in db.KART_SPECMST where n.ITEMID == itemId select n).ToList();
                if (item_ache_kina.Count != 0)
                {
                    var max_idta_berkorbo = (from n in db.KART_SPECMST where n.ITEMID == itemId select n.SPECSL).Max();
                    CartSpecMst model = new CartSpecMst
                    {
                        ITEMID = itemId,
                        SPECSL = max_idta_berkorbo + 1,
                        SPECNM = SpecificationName,
                        VIEWSL = viewSl

                    };
                    db.KART_SPECMST.Add(model);
                    db.SaveChanges();

                    yield return new CartSpecMstDTO
                    {

                        ItemId = itemId,
                        SpecificationName = SpecificationName,

                        SpecificationSL = max_idta_berkorbo + 1,
                        ViewSL = viewSl

                    };

                }
                else
                {
                    CartSpecMst model = new CartSpecMst
                    {
                        ITEMID = itemId,
                        SPECSL = Convert.ToInt64((Convert.ToString(itemId) + "01")),
                        SPECNM = SpecificationName,
                        VIEWSL = viewSl

                    };

                    db.KART_SPECMST.Add(model);
                    db.SaveChanges();
                    yield return new CartSpecMstDTO
                    {
                        ItemId = itemId,
                        SpecificationName = SpecificationName,

                        SpecificationSL = Convert.ToInt64((Convert.ToString(itemId) + "01")),
                        ViewSL = viewSl

                    };
                }

               
              

            }
            else
            {
                var gridData = (from n in db.KART_SPEC where n.ITEMID == itemId && n.SPECSL==specificationSl select n).ToList();

                if (gridData.Count == 0)
                {

                    yield return new CartSpecMstDTO
                    {

                        ItemId = itemId,
                        ItemName = ItemName,
                        SpecificationName = SpecificationName,
                        SpecificationSL = specificationSl,
                        ViewSL = viewSl


                    };

                }
                else
                {



                    var specMstDto = (from t1 in db.KART_SPEC
                                       
                                        where t1.ITEMID == itemId && t1.SPECSL==specificationSl
                                        select new
                                        {
                                            Id = t1.ID,
                                            ItemId = t1.ITEMID,
                                            SpecificationSL = t1.SPECSL,
                                            FeatSl = t1.FEATSL,
                                            FeatType = t1.FEATTP,
                                            Feature = t1.FEATURE


                                        });
                    foreach (var item in specMstDto)
                    {
                        yield return new CartSpecMstDTO
                        {
                            Id = item.Id,
                            ItemId = item.ItemId,
                            SpecificationName = SpecificationName,
                            SpecificationSL = item.SpecificationSL,
                            FeatureSL = item.FeatSl,
                            FeatureType = item.FeatType,
                            Feature = item.Feature
                        };
                    }



                }
            }

        }

        [Route("api/grid/SpecChild")]
        [HttpPost]
        public HttpResponseMessage AddChildData(CartSpecMstDTO model)
        {

            var check_data = (from n in db.KART_SPEC where n.ITEMID == model.ItemId && n.SPECSL == model.SpecificationSL && n.FEATTP == model.FeatureType select n).ToList();
           
            if (check_data.Count == 0)
            {
                var ache_kina_data = (from n in db.KART_SPEC where n.ITEMID == model.ItemId && n.SPECSL==model.SpecificationSL select n).ToList();

                if (ache_kina_data.Count == 0)
                {


                    CartSpec cartSpec = new CartSpec();

                    cartSpec.ITEMID = model.ItemId;
                    cartSpec.SPECSL = model.SpecificationSL;
                    cartSpec.FEATSL = Convert.ToInt64(Convert.ToString(model.SpecificationSL) + "01");
                    cartSpec.FEATTP = model.FeatureType;
                    cartSpec.FEATURE = model.Feature;
                   



                    db.KART_SPEC.Add(cartSpec);

                    db.SaveChanges();

                    model.Id = cartSpec.ID;
                    model.ItemId = cartSpec.ITEMID;
                    model.SpecificationSL = cartSpec.SPECSL;
                    model.FeatureSL = cartSpec.FEATSL;
                    model.FeatureType = cartSpec.FEATTP;
                    model.Feature = cartSpec.FEATURE;
                }
                else
                {
                    Int64 max_idta_berkorbo = (from n in db.KART_SPEC where n.ITEMID == model.ItemId && n.SPECSL == model.SpecificationSL select n.FEATSL).Max();

                    CartSpec cartSpec = new CartSpec();

                    cartSpec.ITEMID = model.ItemId;
                    cartSpec.SPECSL = model.SpecificationSL;
                    cartSpec.FEATSL = max_idta_berkorbo + 1; ;
                    cartSpec.FEATTP = model.FeatureType;
                    cartSpec.FEATURE = model.Feature;



                    db.KART_SPEC.Add(cartSpec);
                    db.SaveChanges();

                    model.Id = cartSpec.ID;
                    model.ItemId = cartSpec.ITEMID;
                    model.SpecificationSL = cartSpec.SPECSL;
                    model.FeatureSL = cartSpec.FEATSL;
                    model.FeatureType =cartSpec.FEATTP;
                    model.Feature = cartSpec.FEATURE;
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                //response.Headers.Location = new Uri(Url.Link("api/ApiFilterItemController", new { gid = model.FILTERGID }));

                return response;
            }
            else
            {
                model.FeatureSL = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }
            

        }

        [Route("api/ApiSpecification/SaveData")]
        [HttpPost]
        public HttpResponseMessage SaveData(CartSpecMstDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var duplicate_data_test =
                   (from n in db.KART_SPEC where n.ITEMID == model.ItemId && n.SPECSL == model.SpecificationSL && n.FEATTP==model.FeatureType select n).ToList();
            Int64 id = 0;
            if (duplicate_data_test.Count > 0)
            {
                foreach (var x in duplicate_data_test)
                {
                    id = x.ID;
                }
                if (id == model.Id)
                {
                    //CartSpec cartSpec = new CartSpec();
                    var data_find = (from n in db.KART_SPEC where n.ID == model.Id select n).ToList();
                    foreach (var cartSpec in data_find)
                    {
                        cartSpec.ID = model.Id;
                        cartSpec.ITEMID = model.ItemId;
                        cartSpec.SPECSL = model.SpecificationSL;
                        cartSpec.FEATSL = model.FeatureSL;
                        cartSpec.FEATTP = model.FeatureType;
                        cartSpec.FEATURE = model.Feature;
                        
                    }
                    

                    //db.Entry(cartSpec).State = EntityState.Modified;
                    db.SaveChanges();


                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }
                else
                {
                    model.FeatureSL = 0;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                    return response;
                }
            }
            else
            {
                CartSpec cartSpec = new CartSpec();

                cartSpec.ID = model.Id;
                cartSpec.ITEMID = model.ItemId;
                cartSpec.SPECSL = model.SpecificationSL;
                cartSpec.FEATSL = model.FeatureSL;
                cartSpec.FEATTP = model.FeatureType;
                cartSpec.FEATURE = model.Feature;

                db.Entry(cartSpec).State = EntityState.Modified;
                db.SaveChanges();


                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);


                return response;
            }
           
        }

        [Route("api/ApiSpecification/DeleteData")]
        [HttpPost]
        public HttpResponseMessage DeleteData(CartSpecMstDTO model)
        {

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebCartDbContext"].ToString());
            string query = string.Format("DELETE FROM KART_SPEC WHERE ID='{0}'", model.Id);
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
            CartSpec cartSpec = new CartSpec();






            return Request.CreateResponse(HttpStatusCode.OK, cartSpec);
        }


    }
}
