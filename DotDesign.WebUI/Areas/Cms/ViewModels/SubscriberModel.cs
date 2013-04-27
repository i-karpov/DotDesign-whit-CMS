using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using DotDesign.Domain.Entities;

namespace DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class SubscriberModel
    {
        public int Id
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

        [Email]
        [Required]
        [MaxLength(320, ErrorMessage = "Provided email is too long (max {1} characters).")]
        public String Email
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }

        public SubscriberModel()
        {
        }

        public SubscriberModel(Subscriber subscriber)
        {
            this.Id = subscriber.Id;
            this.Name = subscriber.Name;
            this.Email = subscriber.Email;
            this.CreateDate = subscriber.CreateDate;
        }

        /// <summary>
        /// Updates given Subscriber instance according to model.
        /// Updating affects Name and Email properties of given instance.
        /// </summary>
        public void UpdateSubscriber(Subscriber subscriber)
        {
            subscriber.Name = this.Name;
            subscriber.Email = this.Email;
        }
    }
}