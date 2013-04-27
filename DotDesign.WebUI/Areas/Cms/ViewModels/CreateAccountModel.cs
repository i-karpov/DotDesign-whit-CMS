using System;
using System.ComponentModel.DataAnnotations;
using Itransition.DotDesign.Domain.Entities;

namespace Itransition.DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class CreateAccountModel
    {
        [Required]
        [Display(Name = "Username")]
        public String Username
        {
            get;
            set;
        }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String NewPassword
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public String NewPasswordConfirmation
        {
            get;
            set;
        }

        public AdminProfile Profile
        {
            get;
            set;
        }

        public CreateAccountModel()
        {
            Profile = new AdminProfile();
        }
    }
}