using DotDesign.Domain.AbstractRepositories;
using DotDesign.Domain.AbstractRepositories;

namespace DotDesign.DataLayer.SqlRepositories
{
    public class SqlAdminsRepository : IAdminsRepository
    {
        private DotDesignCmsContext Context
        {
            get;
            set;
        }

        public SqlAdminsRepository(DotDesignCmsContext context)
        {
            this.Context = context;
        }
    }
}
