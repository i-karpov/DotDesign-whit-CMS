using System;
using System.Collections.Generic;

namespace DotDesign.Domain.Entities
{
    public class Category
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

        public String Name
        {
            get;
            set;
        }

        private ICollection<Page> pages;

        public virtual ICollection<Page> Pages
        {
            get
            {
                return pages ?? (pages = new List<Page>());
            }
            protected set
            {
                pages = value;
            }
        }

        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// If true, category should be shown in the navbar, otherwise - should not.
        /// </summary>
        public bool IsPublic
        {
            get;
            set;
        }
    }
}
