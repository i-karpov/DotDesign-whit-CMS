using DotDesign.Domain.Entities;
using PagedList;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class AllPagesModel
    {
        public IPagedList<Page> Pages
        {
            get;
            set;
        }

        public int HomePageId
        {
            get;
            set;
        }
    }
}