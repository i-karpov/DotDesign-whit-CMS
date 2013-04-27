using DotDesign.Domain.Entities;
using System;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class PageDetailsModel
    {
        public Page Page
        {
            get;
            set;
        }

        public String CategoryName
        {
            get;
            set;
        }

        public bool IsHomePage
        {
            get;
            set;
        }
    }
}