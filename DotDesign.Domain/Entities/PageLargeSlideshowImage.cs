namespace DotDesign.Domain.Entities
{
    public class PageLargeSlideshowImage
    {
        public int Id
        {
            get;
            set;
        }

        public int PageId
        {
            get;
            set;
        }

        public virtual Page Page
        {
            get;
            set;
        }

        public int ImageId
        {
            get;
            set;
        }

        public virtual Image Image
        {
            get;
            set;
        }
    }
}
