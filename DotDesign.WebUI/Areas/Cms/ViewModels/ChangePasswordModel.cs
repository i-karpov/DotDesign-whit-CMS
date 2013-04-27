using System;
using System.ComponentModel.DataAnnotations;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class ChangePasswordModel
    {
        [MaxLength(100, ErrorMessage = "Provided username is too long (max {1} characters).")]
        public String Username
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public String OldPassword
        {
            get;
            set;
        }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public String NewPassword
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public String NewPasswordConfirmation
        {
            get;
            set;
        }
    }
}