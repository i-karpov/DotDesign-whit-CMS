using Itransition.DotDesign.Domain.Entities;
using System;

namespace Itransition.DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class EditProfileModel
    {
        public String Username
        {
            get;
            set;
        }

        public AdminProfile Profile
        {
            get;
            set;
        }

        public EditProfileModel()
        {
            Profile = new AdminProfile();
        }
    }
}