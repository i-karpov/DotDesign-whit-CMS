using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotDesign.DataLayer;
using WebMatrix.WebData;

namespace DotDesign.WebUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DotDesignCmsContext>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<DotDesignCmsContext>());
            
            WebSecurity.InitializeDatabaseConnection(
                "DotDesignCMSConnectionString",
                    "Admins",
                    "Id",
                    "Username",
                    autoCreateTables: true);
        }
    }
}