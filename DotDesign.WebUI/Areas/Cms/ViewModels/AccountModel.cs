using System;
using System.ComponentModel.DataAnnotations;
using DotDesign.Domain.Entities;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class AccountModel
    {
        public int Id
        {
            get;
            set;
        }

        [Required]
        [MaxLength(100, ErrorMessage = "Provided username is too long (max {1} characters).")]
        public String Username
        {
            get;
            set;
        }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public String NewPassword
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public String NewPasswordConfirmation
        {
            get;
            set;
        }

        public AdminProfileModel ProfileModel
        {
            get;
            set;
        }

        public bool IsActiveAdmin
        {
            get;
            set;
        }

        public AccountModel()
        {
            ProfileModel = new AdminProfileModel();
        }

        /// <summary>
        /// Create new AccountModel according to Admin instance and
        /// information indicating whether given admin is current active one.
        /// </summary>
        /// <param name="admin">
        /// Admin instance to be modeled.
        /// </param>
        /// <param name="isActiveAdmin">
        /// Indicates whether given admin is current active admin of system.
        /// </param>
        public AccountModel(Admin admin, bool isActiveAdmin)
        {
            this.Id = admin.Id;
            this.Username = admin.Username;
            this.ProfileModel = new AdminProfileModel(admin.Profile);
            this.IsActiveAdmin = isActiveAdmin;
        }
    }
}