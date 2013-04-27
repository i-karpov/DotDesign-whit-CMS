using DotDesign.DataLayer.SqlRepositories;
using DotDesign.Domain;
using DotDesign.Domain.AbstractRepositories;
using System;

namespace DotDesign.DataLayer
{
    public class SqlRepositoryFactory : IRepositoryFactory
    {
        private IAdminsRepository adminsRepository;

        private DotDesignCmsContext DotDesignContext
        {
            get;
            set;
        } 

        public SqlRepositoryFactory(String connectionString)
        {
            this.DotDesignContext = new DotDesignCmsContext(connectionString);
        }

        IAdminsRepository IRepositoryFactory.AdminsRepository
        {
            get
            {
                return adminsRepository ??
                       (adminsRepository = new SqlAdminsRepository(DotDesignContext));
            }
        }

        ICategoriesRepository IRepositoryFactory.CategoriesRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IHomePageRepository IRepositoryFactory.HomePageRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IImagesRepository IRepositoryFactory.ImagesRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IPagesRepository IRepositoryFactory.PagesRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ISubscribersRepository IRepositoryFactory.SubscribersRepository
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
