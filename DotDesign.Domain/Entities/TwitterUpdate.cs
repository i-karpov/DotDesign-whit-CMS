using System;

namespace DotDesign.Domain.Entities
{
    public class TwitterUpdate
    {
        public int Id
        {
            get;
            set;
        }

        public String Text
        {
            get;
            set;
        }

        /// <summary>
        /// If true, link should be shown in the navbar, otherwise - should not.
        /// </summary>
        public bool IsPublic
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }
    }
}
