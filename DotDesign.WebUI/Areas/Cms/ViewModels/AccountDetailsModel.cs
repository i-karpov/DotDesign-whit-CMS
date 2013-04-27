using Itransition.DotDesign.Domain.Entities;

namespace Itransition.DotDesign.WebUI.Areas.Cms.ViewModels
{
    public class AccountDetailsModel
    {
        public Admin Admin
        {
            get;
            set;
        }

        public bool IsCurrentAdmin
        {
            get;
            set;
        }

    }
}