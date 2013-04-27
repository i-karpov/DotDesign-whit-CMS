using System.Linq;
using DotDesign.WebUI.ViewModels;
using DotDesign.Domain.Entities;
using System;
using System.Collections.Generic;
using DotDesign.WebUI.Utility;

namespace DotDesign.WebUI.ViewModels
{
    /// <summary>
    /// Defines page, created by CMS Admin.
    /// </summary>
    public class ContentsPageModel : IPage
    {
        public String Title
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        public String ContentsMarkup
        {
            get;
            set;
        }        

        public bool IsLargeSlideshowEnabled
        {
            get;
            set;
        }

        public bool IsSmallSlideshowEnabled
        {
            get;
            set;
        }

        public List<Image> LargeSlideshowImages
        {
            get;
            set;
        }

        public List<Image> SmallSlideshowImages
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

        public ContentsPageModel()
        {
            LargeSlideshowImages = new List<Image>();
            SmallSlideshowImages = new List<Image>();
        }


        public ContentsPageModel(Page page, PagesCommonPart commonPart)
        {
            if (page != null)
            {
                this.Title = page.Title;
                this.Description = page.Description;
                this.ContentsMarkup = page.ContentsMarkup;
                if (page.Category != null)
                {
                    ((IPage) this).CurrentCategoryUrl = page.Category.Url;
                }
                if (page.IsLargeSlideshowEnabled)
                {
                    this.IsLargeSlideshowEnabled = page.IsLargeSlideshowEnabled;
                    this.LargeSlideshowImages = page.PageLargeSlideshowImages.
                        Select(pi => pi.Image).ToList();
                }
                if (page.IsSmallSlideshowEnabled)
                {
                    this.IsSmallSlideshowEnabled = page.IsSmallSlideshowEnabled;
                    this.SmallSlideshowImages = page.PageSmallSlideshowImages.
                        Select(pi => pi.Image).ToList();
                }
            }
            else
            {
                this.Title = Config.DefaultPageName;
                this.ContentsMarkup = String.Empty;
            }
            ((IPage)this).CommonPart = commonPart ?? new PagesCommonPart();
            if (((IPage)this).CommonPart.Subscriber == null)
            {
                ((IPage)this).CommonPart.Subscriber = new Subscriber();
            }
        }
    }
}