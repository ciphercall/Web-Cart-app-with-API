namespace AslWebCartAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AslCompanies",
                c => new
                    {
                        AslCompanyId = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        COMPNM = c.String(nullable: false),
                        ADDRESS = c.String(nullable: false),
                        CONTACTNO = c.String(nullable: false),
                        EMAILID = c.String(nullable: false),
                        WEBID = c.String(),
                        STATUS = c.String(nullable: false),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => t.AslCompanyId);
            
            CreateTable(
                "dbo.ASL_DELETE",
                c => new
                    {
                        Asl_DeleteID = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        USERID = c.Long(),
                        DELSLNO = c.Long(),
                        DELDATE = c.String(),
                        DELTIME = c.String(),
                        DELIPNO = c.String(),
                        DELLTUDE = c.String(),
                        TABLEID = c.String(),
                        DELDATA = c.String(),
                        USERPC = c.String(),
                    })
                .PrimaryKey(t => t.Asl_DeleteID);
            
            CreateTable(
                "dbo.ASL_LOG",
                c => new
                    {
                        Asl_LOGid = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        USERID = c.Long(),
                        LOGTYPE = c.String(),
                        LOGSLNO = c.Long(),
                        LOGDATE = c.DateTime(),
                        LOGTIME = c.String(),
                        LOGIPNO = c.String(),
                        LOGLTUDE = c.String(),
                        TABLEID = c.String(),
                        LOGDATA = c.String(),
                        USERPC = c.String(),
                    })
                .PrimaryKey(t => t.Asl_LOGid);
            
            CreateTable(
                "dbo.ASL_MENU",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MODULEID = c.String(),
                        SERIAL = c.Long(),
                        MENUTP = c.String(),
                        MENUID = c.String(),
                        MENUNM = c.String(),
                        ACTIONNAME = c.String(),
                        CONTROLLERNAME = c.String(),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ASL_MENUMST",
                c => new
                    {
                        MODULEID = c.String(nullable: false, maxLength: 128),
                        MODULENM = c.String(nullable: false),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                    })
                .PrimaryKey(t => t.MODULEID);
            
            CreateTable(
                "dbo.ASL_ROLE",
                c => new
                    {
                        ASL_ROLEId = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(nullable: false),
                        USERID = c.Long(nullable: false),
                        MODULEID = c.String(nullable: false),
                        SERIAL = c.Long(),
                        MENUTP = c.String(nullable: false),
                        MENUID = c.String(),
                        STATUS = c.String(),
                        INSERTR = c.String(),
                        UPDATER = c.String(),
                        DELETER = c.String(),
                        MENUNAME = c.String(),
                        ACTIONNAME = c.String(),
                        CONTROLLERNAME = c.String(),
                        USERPC = c.String(),
                        INSUSERID = c.Long(),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        UPDUSERID = c.Long(),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                    })
                .PrimaryKey(t => t.ASL_ROLEId);
            
            CreateTable(
                "dbo.AslUsercoes",
                c => new
                    {
                        AslUsercoId = c.Long(nullable: false, identity: true),
                        COMPID = c.Long(),
                        USERID = c.Long(),
                        USERNM = c.String(nullable: false),
                        DEPTNM = c.String(),
                        OPTP = c.String(nullable: false),
                        ADDRESS = c.String(nullable: false),
                        MOBNO = c.String(nullable: false),
                        EMAILID = c.String(),
                        LOGINBY = c.String(nullable: false),
                        LOGINID = c.String(nullable: false),
                        LOGINPW = c.String(nullable: false),
                        TIMEFR = c.String(nullable: false),
                        TIMETO = c.String(nullable: false),
                        STATUS = c.String(nullable: false),
                        USERPC = c.String(),
                        INSUSERID = c.Long(nullable: false),
                        INSTIME = c.DateTime(),
                        INSIPNO = c.String(),
                        INSLTUDE = c.String(),
                        UPDUSERID = c.Long(nullable: false),
                        UPDTIME = c.DateTime(),
                        UPDIPNO = c.String(),
                        UPDLTUDE = c.String(),
                    })
                .PrimaryKey(t => t.AslUsercoId);
            
            CreateTable(
                "dbo.KART_CATEGORY",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CATID = c.Long(nullable: false),
                        CATNM = c.String(),
                        LEVELGR = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.CATID });
            
            CreateTable(
                "dbo.KART_FILTER",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        FILTERGID = c.Long(nullable: false),
                        FILTERID = c.Long(nullable: false),
                        FILTERNM = c.String(),
                        FILTERNID = c.Long(),
                        FILTERSID = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.FILTERGID, t.FILTERID });
            
            CreateTable(
                "dbo.KART_FILTERMST",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        FILTERGID = c.Long(nullable: false),
                        FILTERGNM = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.FILTERGID });
            
            CreateTable(
                "dbo.KART_ITEM",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ITEMID = c.Long(nullable: false),
                        ITEMNM = c.String(),
                        STOCKTP = c.String(),
                        RATEBDT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RATEUSD = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IMAGEID1 = c.Long(),
                        IMAGEID2 = c.Long(),
                        IMAGEID3 = c.Long(),
                        IMAGEID4 = c.Long(),
                        IMAGEID5 = c.Long(),
                    })
                .PrimaryKey(t => new { t.ID, t.ITEMID });
            
            CreateTable(
                "dbo.KART_ITEMFILT",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ITEMID = c.Long(nullable: false),
                        FILTERGID = c.Long(),
                        FILTERID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.ITEMID });
            
            CreateTable(
                "dbo.KART_LEVEL",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        LEVELGID = c.Long(nullable: false),
                        LEVELGTP = c.String(),
                        LEVELTP = c.String(),
                        LEVELID = c.Long(nullable: false),
                        LEVELNM = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.LEVELGID });
            
            CreateTable(
                "dbo.KART_LEVELMST",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        LEVELGID = c.Long(nullable: false),
                        LEVELGTP = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.LEVELGID });
            
            CreateTable(
                "dbo.KART_MEMBER",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        MEMBERID = c.Long(nullable: false),
                        MEMBERNM = c.String(),
                        EMAILID = c.String(),
                        MOBILENO = c.String(),
                        ADDRESS = c.String(),
                        LOGBY = c.String(),
                        TOKEN = c.String(),
                        USERID = c.String(),
                        USERPW = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.MEMBERID });
            
            CreateTable(
                "dbo.KART_SPEC",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ITEMID = c.Long(nullable: false),
                        SPECSL = c.Long(nullable: false),
                        FEATSL = c.Long(nullable: false),
                        FEATTP = c.String(),
                        FEATURE = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.ITEMID, t.SPECSL, t.FEATSL });
            
            CreateTable(
                "dbo.KART_SPECMST",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ITEMID = c.Long(nullable: false),
                        SPECSL = c.Long(nullable: false),
                        SPECNM = c.String(),
                        VIEWSL = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.ITEMID, t.SPECSL });
            
            CreateTable(
                "dbo.KART_TRANS",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        TRANSTP = c.String(nullable: false, maxLength: 128),
                        TRANSDT = c.DateTime(nullable: false),
                        TRANSNO = c.String(),
                        MEMBERID = c.Long(nullable: false),
                        ITEMID = c.Long(nullable: false),
                        QTY = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RATE = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AMOUNT = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.ID, t.TRANSTP });
            
            CreateTable(
                "dbo.KART_TRANSMST",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        TRANSTP = c.String(nullable: false, maxLength: 128),
                        TRANSDT = c.DateTime(nullable: false),
                        TRANSNO = c.String(),
                        MEMBERID = c.Long(nullable: false),
                        SHIPDIST = c.String(),
                        SHIPADDR = c.String(),
                        SHIPDT = c.DateTime(nullable: false),
                        CONTACTNO = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TRANSMODE = c.String(),
                        TOTAMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DISAMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SCCOST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TOTNET = c.Decimal(nullable: false, precision: 18, scale: 2),
                        REMARKS = c.String(),
                        ORDERSTATUS = c.String(),
                    })
                .PrimaryKey(t => new { t.ID, t.TRANSTP });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.KART_TRANSMST");
            DropTable("dbo.KART_TRANS");
            DropTable("dbo.KART_SPECMST");
            DropTable("dbo.KART_SPEC");
            DropTable("dbo.KART_MEMBER");
            DropTable("dbo.KART_LEVELMST");
            DropTable("dbo.KART_LEVEL");
            DropTable("dbo.KART_ITEMFILT");
            DropTable("dbo.KART_ITEM");
            DropTable("dbo.KART_FILTERMST");
            DropTable("dbo.KART_FILTER");
            DropTable("dbo.KART_CATEGORY");
            DropTable("dbo.AslUsercoes");
            DropTable("dbo.ASL_ROLE");
            DropTable("dbo.ASL_MENUMST");
            DropTable("dbo.ASL_MENU");
            DropTable("dbo.ASL_LOG");
            DropTable("dbo.ASL_DELETE");
            DropTable("dbo.AslCompanies");
        }
    }
}
