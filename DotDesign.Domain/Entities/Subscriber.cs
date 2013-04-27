using System;

namespace DotDesign.Domain.Entities
{
    public class Subscriber
    {
        public int Id
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

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
    }
}
