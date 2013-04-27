using System;
using System.Web.Mvc;

namespace DotDesign.WebUI.Areas.Cms
{
    public class CmsAreaRegistration : AreaRegistration
    {
        public override String AreaName
        {
            get
            {
                return "Cms";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: null,
                url: "Resources/Image/{imageUrl}",
                defaults: new { controller = "Images", action = "Image" }
            );

            context.MapRoute(
                "Cms_default",
                "Admin/{controller}/{action}",
                new { controller = "Pages", action = "AllPages" }
            );
        }
    }
}
