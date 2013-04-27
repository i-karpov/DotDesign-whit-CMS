using System;
using System.ComponentModel.DataAnnotations;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public String Username
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String Password
        {
            get;
            set;
        }
    }
}