
namespace DotDesign.Domain.Entities
{
    public class HomePage
    {
        public int Id
        {
            get;
            set;
        }

        // TODO: is it needed to get rid of it?
        public virtual Page Page
        {
            get;
            set;
        }
    }
}
