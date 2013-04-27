using System.Linq;
using DotDesign.Domain;
using DotDesign.Domain.Entities;
using DotDesign.WebUI.Areas.Cms.Filters;
using DotDesign.WebUI.Areas.Cms.ViewModels;
using DotDesign.WebUI.Utility;
using System;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using WebMatrix.WebData;

namespace DotDesign.WebUI.Areas.Cms.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly PagingSettings pagingSettings;

        private IRepositoryFactory RepositoryFactory
        {
            get;
            set;
        }

        public AccountsController(IRepositoryFactory repositoryFactory)
        {
            this.RepositoryFactory = repositoryFactory;
            this.pagingSettings = Config.CmsPagingSettings;
        }

        [AllowAnonymous]
        public ActionResult Login(String returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, String returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.Username, model.Password))
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("AllPages", "Pages");
            }

            ModelState.AddModelError("", Config.LoginErrorMessage);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login", "Accounts");
        }

        #region Profile CRUD

        public ViewResult AllAccounts(int? pageNumber)
        {
            // TODO: сделать вместо этого убожества нормальную вьюшку!
            return View(RepositoryFactory.AdminsRepository.
                            GetAllAdmins().
                            OrderBy(a => a.Id).
                            Select(a => new AccountModel
                                            {
                                                Id = a.Id,
                                                Username = a.Username,
                                                ProfileModel = new AdminProfileModel
                                                                   {
                                                                       FirstName = a.Profile.FirstName,
                                                                       LastName = a.Profile.LastName,
                                                                       PhoneNumber = a.Profile.PhoneNumber,
                                                                       Email = a.Profile.Email,
                                                                       Description = a.Profile.Description
                                                                   }
                                            }).
                            ToPagedList(pagingSettings.EnsurePageNumber(pageNumber,
                                                                        RepositoryFactory.
                                                                            AdminsRepository.
                                                                            CountAdmins()),
                                        pagingSettings.ItemsPerPageCount));
        }

        public ViewResult Details(int? id)
        {
            Admin admin = id.HasValue ?
                RepositoryFactory.AdminsRepository.GetAdmin(a => a.Id == id.Value) :
                RepositoryFactory.AdminsRepository.GetAdmin(a => a.Username == User.Identity.Name);
            return View(new AccountModel(admin,
                                !id.HasValue || id.Value == WebSecurity.CurrentUserId));
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                bool createdSuccessfully;
                try
                {
                    WebSecurity.CreateUserAndAccount(model.Username, model.NewPassword);
                    createdSuccessfully = true;
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    createdSuccessfully = false;
                }
                if (createdSuccessfully)
                {
                    Admin adminToUpdate = new Admin
                                              {
                                                  Username = model.Username
                                              };
                    model.ProfileModel.UpdateAdminProfile(adminToUpdate.Profile);
                    RepositoryFactory.AdminsRepository.UpdateAdmin(adminToUpdate);
                    return RedirectToAction("AllAccounts");
                }
            }
            return View(model);
        }

        public ActionResult Edit()
        {
            Admin currentAdmin = RepositoryFactory.AdminsRepository.
                                    GetAdmin(a => a.Username == User.Identity.Name);
            AccountModel model = new AccountModel
                {
                    Username = currentAdmin.Username,
                    ProfileModel = new AdminProfileModel(currentAdmin.Profile)
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateOnlyIncomingValues]
        public ActionResult Edit(AccountModel model, String returnUrl)
        {
            if (ModelState.IsValid)
            {
                Admin currentAdmin = new Admin
                                         {
                                             Username = User.Identity.Name
                                         };
                model.ProfileModel.UpdateAdminProfile(currentAdmin.Profile);
                RepositoryFactory.AdminsRepository.UpdateAdmin(currentAdmin);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Details"); 
            }
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid &&
                WebSecurity.ChangePassword(User.Identity.Name,
                                           model.OldPassword,
                                           model.NewPassword))
            {
                return RedirectToAction("PasswordChanged");
            }
            if (ModelState.IsValid)
            {
                ModelState.AddModelError("OldPassword", Config.WrongOldPasswordError);
            }   
            return View(model);
        }

        public ViewResult PasswordChanged()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete()
        {
            if (RepositoryFactory.AdminsRepository.CountAdmins() <= 1)
            {
                return View("CanNotDeleteAccount");
            }      
            return PartialView("_Delete",
                new Admin
                    {
                        Username = User.Identity.Name
                    });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            WebSecurity.Logout();   
            ((SimpleMembershipProvider) Membership.Provider).DeleteAccount(User.Identity.Name);
            Membership.Provider.DeleteUser(User.Identity.Name, true);
            return RedirectToAction("AccountDeleted");
        }

        [AllowAnonymous]
        public ViewResult AccountDeleted()
        {
            return View();
        }

        #endregion

        #region Helpers
        // TODO: review method
        private static String ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
