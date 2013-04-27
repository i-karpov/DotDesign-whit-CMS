using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using DotDesign.Domain.Entities;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class AdminProfileModel
    {
        [MaxLength(100, ErrorMessage = "Provided first name is too long (max {1} characters).")]
        public String FirstName
        {
            get;
            set;
        }

        [MaxLength(100, ErrorMessage = "Provided last name is too long (max {1} characters).")]
        public String LastName
        {
            get;
            set;
        }

        [MaxLength(100, ErrorMessage = "Provided phone number is too long (max {1} characters).")]
        public String PhoneNumber
        {
            get;
            set;
        }

        [Email]
        [MaxLength(320, ErrorMessage = "Provided email is too long (max {1} characters).")]
        public String Email
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        public bool HasValue
        {
            get
            {
                return (FirstName != null ||
                        LastName != null ||
                        PhoneNumber != null ||
                        Email != null ||
                        Description != null);
            }
        }

        public AdminProfileModel()
        {
        }

        public AdminProfileModel(AdminProfile adminProfile)
        {
            this.FirstName = adminProfile.FirstName;
            this.LastName = adminProfile.LastName;
            this.PhoneNumber = adminProfile.PhoneNumber;
            this.Email = adminProfile.Email;
            this.Description = adminProfile.Description;
        }

        /// <summary>
        /// Updates given AdminProfile instance according to model.
        /// Updating affects all properties of given instance represented in model.
        /// </summary>
        public void UpdateAdminProfile(AdminProfile adminProfile)
        {
            adminProfile.FirstName = this.FirstName;
            adminProfile.LastName = this.LastName;
            adminProfile.PhoneNumber = this.PhoneNumber;
            adminProfile.Email = this.Email;
            adminProfile.Description = this.Description;
        }
    }
}