using System;
using System.Linq;
using System.Linq.Expressions;
using DotDesign.Domain.Entities;

namespace DotDesign.Domain.AbstractRepositories
{
    public interface IAdminsRepository
    {
        IQueryable<Admin> GetAllAdmins();

        Admin GetAdmin(Expression<Func<Admin, bool>> predicate);

        /// <summary>
        /// Identifies Admin entity by Username field and updates it.
        /// </summary>
        /// <param name="admin"></param>
        void UpdateAdmin(Admin admin);

        int CountAdmins();
    }
}
