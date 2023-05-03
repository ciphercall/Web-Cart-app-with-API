using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AslWebCartAPI.Models
{
    public class AslWebCartContext : DbContext
    {
        public AslWebCartContext()
            : base("name=WebCartDbContext")
        {
            //base.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<CartCategory> KART_CATEGORY { get; set; }
        public DbSet<CartFilterMst> KART_FILTERMST { get; set; }
        public DbSet<CartFilter> KART_FILTER { get; set; }
        public DbSet<CartItem> KART_ITEM { get; set; }
        public DbSet<CartItemFilt> KART_ITEMFILT { get; set; }
        public DbSet<CartLevelMst> KART_LEVELMST { get; set; }
        public DbSet<CartLevel> KART_LEVEL { get; set; }
        public DbSet<CartSpecMst> KART_SPECMST { get; set; }
        public DbSet<CartSpec> KART_SPEC { get; set; }
        public DbSet<CartMember> KART_MEMBER { get; set; }
        public DbSet<CartTransMst> KART_TRANSMST { get; set; }
        public DbSet<CartTrans> KART_TRANS { get; set; }


        public DbSet<AslCompany> AslCompanyDbSet { get; set; }
        public DbSet<AslUserco> AslUsercoDbSet { get; set; }
        public DbSet<ASL_LOG> AslLogDbSet { get; set; }
        public DbSet<ASL_DELETE> AslDeleteDbSet { get; set; }
        public DbSet<ASL_MENUMST> AslMenumstDbSet { get; set; }
        public DbSet<ASL_MENU> AslMenuDbSet { get; set; }
        public DbSet<ASL_ROLE> AslRoleDbSet { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

    }
}