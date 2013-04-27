
using System;
namespace DotDesign.WebUI.ViewModels
{
    /// <summary>
    /// Cantains property defining page parts that are common for all client pages.
    /// Must be implemented by all pages that use _ClientLayout master page.
    /// </summary>
    public interface IPage
    {
        PagesCommonPart CommonPart
        {
            get;
            set;
        }

        String CurrentCategoryUrl
        {
            get;
            set;
        }
    }
}
