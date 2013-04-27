using DotDesign.Domain.Entities;
using System;

namespace DotDesign.WebUI.ViewModels
{
    public class BottomNavbarItemModel
    {
        public Category Category
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