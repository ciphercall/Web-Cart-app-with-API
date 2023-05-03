using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models.IRepository
{
    public interface ICartCategory
    {
        CartCategory Add(CartCategory cc);
    }
}