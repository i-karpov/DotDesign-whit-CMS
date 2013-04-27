using System;
using System.Collections.Generic;

namespace DotDesign.Domain.Entities
{
    public class Page
    {
        public int Id
        {
            get;
            set;
        }

        public String Url
        {
            get;
            set;
        }

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

        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// Defines whether page should be displayed in footer in "Latest Posts" section or shouldn't.
        /// </summary>
        public bool IsDisplayedInLatestPosts
        {
            get;
            set;
        }

        /// <summary>
        /// Defines whether page should be available for client or shouldn't.
        /// </summary>
        public bool IsPublic
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

        public int? CategoryId
        {
            get;
            set;
        }

        public virtual Category Category
        {
            get;
            set;
        }

        private ICollection<PageLargeSlideshowImage> pageLargeSlideshowImages;

        public virtual ICollection<PageLargeSlideshowImage> PageLargeSlideshowImages
        {
            get
            {
                return pageLargeSlideshowImages ??
                    (pageLargeSlideshowImages = new List<PageLargeSlideshowImage>());
            }
            protected set
            {
                pageLargeSlideshowImages = value;
            }
        }

        private ICollection<PageSmallSlideshowImage> pageSmallSlideshowImages;

        public virtual ICollection<PageSmallSlideshowImage> PageSmallSlideshowImages
        {
            get
            {
                return pageSmallSlideshowImages ??
                    (pageSmallSlideshowImages = new List<PageSmallSlideshowImage>());
            }
            protected set
            {
                pageSmallSlideshowImages = value;
            }
        }
    }
}
