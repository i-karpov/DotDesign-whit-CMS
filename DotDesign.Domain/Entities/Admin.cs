using System;

namespace DotDesign.Domain.Entities
{

    public class Admin
    {
        public int Id
        {
            get;
            set;
        }

        public String Username
        {
            get;
            set;
        }

        public AdminProfile Profile
        {
            get;
            set;
        }

        public Admin()
        {
            Profile = new AdminProfile();
        }
    }
}
