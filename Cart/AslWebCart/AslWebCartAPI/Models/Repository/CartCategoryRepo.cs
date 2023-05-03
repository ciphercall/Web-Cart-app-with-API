using AslWebCartAPI.Models.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.Repository
{
    public class CartCategoryRepo : ICartCategory
    {
        private AslWebCartContext db = new AslWebCartContext();

        public CartCategory Add(CartCategory cc)
        {
            db.KART_CATEGORY.Add(cc);
            db.SaveChanges();
            return cc;
        }

        public IEnumerable<CartCategory> Get()
        {
            return db.KART_CATEGORY.OrderBy(cart => cart.CATID);
        }

    }
}