
using System;
using DotDesign.Domain.Entities;

namespace DotDesign.WebUI.ViewModels
{
    public class TwitterUpdateModel
    {
        public String Text
        {
            get;
            set;
        }

        public String Age
        {
            get;
            set;
        }

        public TwitterUpdateModel(TwitterUpdate twitterUpdate)
        {
            this.Text = twitterUpdate.Text;
            TimeSpan age = DateTime.Now.Subtract(twitterUpdate.CreateDate);
            if (age.Minutes < 1)
            {
                this.Age = "just now";
            }
            else if (age.Hours < 1)
            {
                this.Age = String.Format("about {0}min ago", age.Minutes);
            }
            else if (age.Days < 1)
            {
                this.Age = String.Format("about {0}h ago", age.Hours);
            }
            else if (age.Days < 32)
            {
                this.Age = String.Format("about {0}d ago", age.Days);
            }
            else if (age.Days < 367)
            {
                DateTime currentTime = DateTime.Now;
                this.Age = String.Format("about {0}m ago",
                    currentTime.Month - twitterUpdate.CreateDate.Month +
                    12 * (currentTime.Year - twitterUpdate.CreateDate.Year));
            }
            else
            {
                this.Age = String.Format("about {0}y ago",
                    DateTime.Now.Year - twitterUpdate.CreateDate.Year);
            }
        }
    }
}