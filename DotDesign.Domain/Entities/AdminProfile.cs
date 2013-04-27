using System;

namespace DotDesign.Domain.Entities
{
    public class AdminProfile
    {
        public String FirstName
        {
            get;
            set;
        }

        public String LastName
        {
            get;
            set;
        }

        public String PhoneNumber
        {
            get;
            set;
        }

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
    }
}
