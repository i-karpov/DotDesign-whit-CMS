using DotDesign.Domain.Entities;
using PagedList;
using System;

namespace DotDesign.WebUI.ViewModels
{
    /// <summary>
    /// Represents page containing list of some pages' brief reviews.
    /// </summary>
    public class CategoryPageModel : IPage
    {
        public IPagedList<BriefReview> BriefReviews
        {
            get;
            set;
        }

        public String CategoryName
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

        public CategoryPageModel(PagesCommonPart commonPart, String currentCategoryUrl)
        {
            ((IPage) this).CommonPart = commonPart ?? new PagesCommonPart();
            if (((IPage)this).CommonPart.Subscriber == null)
            {
                ((IPage)this).CommonPart.Subscriber = new Subscriber();
            }
            ((IPage)this).CurrentCategoryUrl = currentCategoryUrl;
        }
    }
}