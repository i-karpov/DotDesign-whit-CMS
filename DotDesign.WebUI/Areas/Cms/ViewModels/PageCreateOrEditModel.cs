using DotDesign.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class PageCreateOrEditModel : ICloneable
    {
        public int Id
        {
            get;
            set;
        }

        [Required]
        [MaxLength(100, ErrorMessage = "Provided title is too long (max {1} characters).")]
        public String Title
        {
            get;
            set;
        }

        [Required]
        [RegularExpression("[a-zA-Zа-яА-Я0-9- ]+",
            ErrorMessage = "Page url can contain letters, numbers, hyphens and whitespaces only.")]
        [MaxLength(100, ErrorMessage = "Provided url is too long (max {1} characters).")]
        public String Url
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        [AllowHtml]
        [UIHint("tinymce_jquery")]
        public String ContentsMarkup
        {
            get;
            set;
        }

        public bool IsDisplayedInLatestPosts
        {
            get;
            set;
        }

        public int? CategoryId
        {
            get;
            set;
        }

        public bool IsLargeSlideshowEnabled
        {
            get;
            set;
        }

        public bool IsPublic
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

        public List<SelectListItem> AvailableCategories
        {
            get;
            set;
        }

        public bool IsHomePage
        {
            get;
            set;
        }

        public PageCreateOrEditModel()
        {
            LargeSlideshowImages = new List<Image>();
            SmallSlideshowImages = new List<Image>();
        }

        public PageCreateOrEditModel(Page page,
            bool isHomePage,
            IEnumerable<SelectListItem> availableCategories)
        {
            this.IsHomePage = isHomePage;

            this.LargeSlideshowImages = new List<Image>();
            this.SmallSlideshowImages = new List<Image>();

            this.Id = page.Id;
            this.Title = page.Title;
            this.Url = page.Url;
            this.ContentsMarkup = page.ContentsMarkup;
            this.AvailableCategories = new List<SelectListItem>
                                            {
                                                new SelectListItem
                                                    {
                                                        Selected = true,
                                                        Text = "NONE",
                                                        Value = "-1"
                                                    }
                                            };
            this.AvailableCategories.AddRange(availableCategories);
            this.IsDisplayedInLatestPosts = page.IsDisplayedInLatestPosts;
            this.IsPublic = page.IsPublic;
            this.Description = page.Description;
            this.CategoryId = page.CategoryId;
            this.IsLargeSlideshowEnabled = page.IsLargeSlideshowEnabled;
            this.LargeSlideshowImages = page.PageLargeSlideshowImages.
                    Select(pi => pi.Image).ToList();
            this.IsSmallSlideshowEnabled = page.IsSmallSlideshowEnabled;
            this.SmallSlideshowImages = page.PageSmallSlideshowImages.
                Select(pi => pi.Image).ToList();
        }

        // TODO: refactor
        Object ICloneable.Clone()
        {
            PageCreateOrEditModel clone = new PageCreateOrEditModel
            {
                Id = this.Id,
                Url = this.Url,
                Title = this.Title,
                Description = this.Description,
                CategoryId = this.CategoryId,
                ContentsMarkup = this.ContentsMarkup,
                AvailableCategories = this.AvailableCategories,
                IsLargeSlideshowEnabled = this.IsLargeSlideshowEnabled,
                IsSmallSlideshowEnabled = this.IsSmallSlideshowEnabled,
                LargeSlideshowImages = this.LargeSlideshowImages,
                SmallSlideshowImages = this.SmallSlideshowImages,
                IsDisplayedInLatestPosts = this.IsDisplayedInLatestPosts,
                IsPublic = this.IsPublic
            };
            return clone;
        }
    }
}