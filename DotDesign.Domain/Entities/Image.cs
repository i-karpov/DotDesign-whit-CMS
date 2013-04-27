using System;
using System.Collections.Generic;

namespace DotDesign.Domain.Entities
{
    public class Image
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

        public String ServerFilePath
        {
            get;
            set;
        }

        public DateTime CreateDate
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
