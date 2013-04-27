using DotDesign.WebUI.ViewModels;
using DotDesign.Domain.Entities;
using System.Collections.Generic;

namespace DotDesign.WebUI.ViewModels
{
    /// <summary>
    /// Contains navbar and footer data like categories, latest posts etc.
    /// </summary>
    public class PagesCommonPart
    {
        public List<Category> Categories
        {
            get;
            set;
        }

        public List<LatestPostModel> LatestPosts
        {
            get;
            set;
        }

        public List<TwitterUpdateModel> TwitterUpdates
        {
            get;
            set;
        }

        public Subscriber Subscriber
        {
            get;
            set;
        }

        public PagesCommonPart()
        {
            Categories = new List<Category>();
            LatestPosts = new List<LatestPostModel>();
            TwitterUpdates = new List<TwitterUpdateModel>();
        }
    }
}