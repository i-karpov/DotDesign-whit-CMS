using DotDesign.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class CategoryModel
    {
        public int Id
        {
            get;
            set;
        }

        [Required]
        [MaxLength(100, ErrorMessage = "Provided url is too long (max {1} characters).")]
        public String Url
        {
            get;
            set;
        }

        [Required]
        [MaxLength(100, ErrorMessage = "Provided name is too long (max {1} characters).")]
        public String Name
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }

        public bool IsPublic
        {
            get;
            set;
        }

        public CategoryModel()
        {
        }

        public CategoryModel(Category category)
        {
            this.Id = category.Id;
            this.Url = category.Url;
            this.Name = category.Name;
            this.CreateDate = category.CreateDate;
            this.IsPublic = category.IsPublic;
        }

        /// <summary>
        /// Updates given Category instance according to model.
        /// Updating affects Url, Name and IsPublic properties of given instance.
        /// </summary>
        public void UpdateCategory(Category category)
        {
            category.Url = this.Url;
            category.Name = this.Name;
            category.IsPublic = this.IsPublic;
        }
    }
}