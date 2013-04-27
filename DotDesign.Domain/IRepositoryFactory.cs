using DotDesign.Domain.AbstractRepositories;

namespace DotDesign.Domain
{
    public interface IRepositoryFactory
    {
        IAdminsRepository AdminsRepository
        {
            get;
        }

        ICategoriesRepository CategoriesRepository
        {
            get;
        }

        IHomePageRepository HomePageRepository
        {
            get;
        }

        IImagesRepository ImagesRepository
        {
            get;
        }

        IPagesRepository PagesRepository
        {
            get;
        }

        ISubscribersRepository SubscribersRepository
        {
            get;
        }
    }
}
