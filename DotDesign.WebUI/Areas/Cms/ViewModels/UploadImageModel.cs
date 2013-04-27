using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class UploadImageModel
    {
        [Required]
        [RegularExpression("[a-zA-Zа-яА-Я0-9- ]+",
            ErrorMessage = "Image url can contain letters, numbers, hyphens and whitespaces only.")]
        [MaxLength(100, ErrorMessage = "Provided url is too long (max {1} characters).")]
        public String Url
        {
            get;
            set;
        }

        [Required]
        public HttpPostedFileBase ImageData
        {
            get;
            set;
        }
    }
}