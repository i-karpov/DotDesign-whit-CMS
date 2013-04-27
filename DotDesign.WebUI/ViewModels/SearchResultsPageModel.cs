using System;
using DotDesign.Domain.Entities;
using PagedList;

namespace DotDesign.WebUI.ViewModels
{
    public class SearchResultsPageModel : IPage
    {
        public IPagedList<BriefReview> BriefReviews
        {
            get;
            set;
        }

        public String SearchQuery
        {
            get;
            set;
        }

        PagesCommonPart IPage.CommonPart
        {
            get;
            set;
        }

        String IPage.CurrentCategoryUrl
        {
            get;
            set;
        }

        public SearchResultsPageModel(PagesCommonPart commonPart, String searchQuery)
        {
            this.SearchQuery = searchQuery;
            ((IPage) this).CommonPart = commonPart ?? new PagesCommonPart();
            if (((IPage)this).CommonPart.Subscriber == null)
            {
                ((IPage)this).CommonPart.Subscriber = new Subscriber();
            }
            ((IPage)this).CurrentCategoryUrl = null;
        }
    }
}