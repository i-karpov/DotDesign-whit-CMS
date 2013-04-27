using DotDesign.Domain.Entities;
using System;
using System.Collections.Generic;

namespace DotDesign.WebUI.ViewModels
{
    public class BottomNavbarModel
    {
        public IEnumerable<Category> Categories
        {
            get;
            set;
        }

        public String CurrentCategoryUrl
        {
            get;
            set;
        }
    }
}