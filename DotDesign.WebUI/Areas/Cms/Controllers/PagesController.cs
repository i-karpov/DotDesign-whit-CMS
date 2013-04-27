using DotDesign.DataLayer;
using DotDesign.Domain.Entities;
using DotDesign.WebUI.Areas.Cms.Filters;
using DotDesign.WebUI.Areas.Cms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotDesign.WebUI.Extensions;
using DotDesign.WebUI.SearchEngine;
using PagedList;
using DotDesign.WebUI.Utility;

namespace DotDesign.WebUI.Areas.Cms.Controllers
{
    [Authorize]
    public class PagesController : Controller
    {
        private readonly PagingSettings pagingSettings;

        private DotDesignCmsContext Context
        {
            get;
            set;
        }

        public PagesController(DotDesignCmsContext context)
        {
            this.Context = context;
            this.pagingSettings = Config.CmsPagingSettings;
        }

        public ActionResult AddImage()
        {
            return PartialView("_AddImage", Context.Images.ToList());
        }

        public ViewResult AllPages(int? pageNumber)
        {
            HomePage homePage = Context.HomePages.FirstOrDefault();
            int homePageId = default(int);
            if (homePage != null)
            {
                homePageId = homePage.Id;
            }
            AllPagesModel model = new AllPagesModel
                {
                    Pages = Context.Pages.
                                OrderBy(p => p.Id).
                                ToPagedList(pagingSettings.EnsurePageNumber(pageNumber,
                                                Context.Pages.Count()),
                                    pagingSettings.ItemsPerPageCount),
                    HomePageId = homePageId
                };
            return View(model);
        }

        public ActionResult Details(int id)
        {
            Page page = Context.Pages.SingleOrDefault(p => p.Id == id);
            if (page == null)
            {
                return RedirectToRoute("HttpError404");
            }
            PageDetailsModel model = new PageDetailsModel
                                         {
                                             Page = page,
                                             IsHomePage = false,
                                             CategoryName = page.Category != null ?
                                                                page.Category.Name :
                                                                String.Empty
                                         };
            HomePage homePage = Context.HomePages.FirstOrDefault();
            if (homePage != null && homePage.Id == page.Id)
            {
                model.IsHomePage = true;
            }
            return View(model);
        }

        public ActionResult Create()
        {
            PageCreateOrEditModel model = new PageCreateOrEditModel
                {
                    AvailableCategories = GetDefaultSelectCategoriesList()
                };
            model.AvailableCategories.AddRange(GetAvailableCategoriesList());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PageCreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.Url = model.Url.ReplaceWhiteSpacesWithHyphens().ToLower();
                if (!Context.Pages.Any(p => p.Url.ToLower() == model.Url.ToLower()))
                {
                    Page newPage = CreatePageByModel(model);
                    Context.Pages.Add(newPage);
                    Context.SaveChanges();
                    PageIndexHelper.AddOrUpdate(newPage);
                    return RedirectToAction("AllPages"); 
                }
                ModelState.AddModelError("Url", Config.UrlErrorMessage);
            }
            model = RefillFailedPageModel(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateOnlyIncomingValues]
        public ActionResult MakePageHome(int id)
        {
            Page page = Context.Pages.SingleOrDefault(x => x.Id == id);
            if (page == null)
            {
                return RedirectToRoute("HttpError404");
            }
            HomePage homePage = Context.HomePages.FirstOrDefault();
            if (homePage != null)
            {
                Context.HomePages.Remove(homePage);
            }
            homePage = new HomePage
                {
                    Page = page
                };
            Context.HomePages.Add(homePage);
            Context.SaveChanges();

            if (HttpContext.Request.UrlReferrer != null &&
                Url.IsLocalUrl(HttpContext.Request.UrlReferrer.PathAndQuery))
            {
                return Redirect(HttpContext.Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("AllPages");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateOnlyIncomingValues]
        public ActionResult MakePageCommon(int id)
        {
            Page page = Context.Pages.SingleOrDefault(x => x.Id == id);
            if (page == null)
            {
                return RedirectToRoute("HttpError404");
            }
            HomePage homePage = Context.HomePages.First();
            Context.HomePages.Remove(homePage);
            Context.SaveChanges();

            return RedirectToAction("HomePage");
        }


        public ActionResult Edit(int id)
        {
            Page page = Context.Pages.SingleOrDefault(x => x.Id == id);
            if (page == null)
            {
                return RedirectToRoute("HttpError404");
            }
            HomePage homePage = Context.HomePages.FirstOrDefault();
            return View(new PageCreateOrEditModel(page,
                                homePage != null && homePage.Id == page.Id,
                                GetAvailableCategoriesList()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateOnlyIncomingValues]
        public ActionResult Edit(PageCreateOrEditModel model, String returnUrl)
        {
            Page page = Context.Pages.SingleOrDefault(x => x.Id == model.Id);
            if (page == null)
            {
                return RedirectToRoute("HttpError404");
            }
            if (ModelState.IsValid)
            {
                UpdatePage(page, model);
                PageIndexHelper.AddOrUpdate(page);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("AllPages");
            }
            model = RefillFailedPageModel(model);
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Page page = Context.Pages.SingleOrDefault(x => x.Id == id);
            if (page == null)
            {
                return RedirectToRoute("HttpError404");
            }
            return PartialView("_Delete", page);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, String returnUrl)
        {
            Page page = Context.Pages.SingleOrDefault(x => x.Id == id);
            if (page == null)
            {
                return RedirectToRoute("HttpError404");
            }
            Context.Pages.Remove(page);
            Context.SaveChanges();
            PageIndexHelper.Delete(page.Id);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("AllPages");
        }

        #region HomePage CRUD

        public ViewResult HomePage()
        {
            HomePage homePage = Context.HomePages.FirstOrDefault();
            return homePage == null
                       ? View("HomePageDoesNotExist")
                       : View("Details", new PageDetailsModel
                                             {
                                                 Page = homePage.Page,
                                                 IsHomePage = true
                                             });
        }

        public ActionResult CreateHomePage()
        {
            PageCreateOrEditModel model = new PageCreateOrEditModel
                {
                    AvailableCategories = GetDefaultSelectCategoriesList()
                };
            model.AvailableCategories.AddRange(GetAvailableCategoriesList());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHomePage(PageCreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.Url = model.Url.ReplaceWhiteSpacesWithHyphens();
                if (!Context.Pages.Any(p => p.Url.ToLower() == model.Url.ToLower()))
                {
                    Page newPage = CreatePageByModel(model);
                    Context.Pages.Add(newPage);
                    HomePage homePage = new HomePage
                        {
                            Page = newPage
                        };
                    Context.HomePages.Add(homePage);
                    Context.SaveChanges();
                    PageIndexHelper.AddOrUpdate(newPage);
                    return RedirectToAction("HomePage");
                }
                ModelState.AddModelError("Url", Config.UrlErrorMessage);
            }
            model = RefillFailedPageModel(model);
            return View(model);
        }

        public ActionResult EditHomePage()
        {
            HomePage homePage = Context.HomePages.FirstOrDefault();
            if (homePage == null)
            {
                return RedirectToRoute("HttpError404");
            }
            return View("Edit", new PageCreateOrEditModel(homePage.Page,
                                                            true,
                                                            GetAvailableCategoriesList()));
        }

        [HttpGet]
        public ActionResult DeleteHomePage()
        {
            return PartialView("_DeleteHomePage");
        }

        [HttpPost, ActionName("DeleteHomePage")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteHomePageConfirmed()
        {
            HomePage homePage = Context.HomePages.FirstOrDefault();
            if (homePage != null)
            {
                Context.Pages.Remove(homePage.Page);
                Context.SaveChanges();
                PageIndexHelper.Delete(homePage.Id);
            }
            return RedirectToAction("HomePage");
        }

        #endregion

        #region Non-Action Methods

        /// <summary>
        /// Produces deep copy of PageCreateOrEditModel refilled with Url,
        /// Available Categories List, Small and Large Slideshow Images Lists.
        /// It's supposed that if submitted model allready contains page Id, this Id is correct.
        /// Use this method after model has failed validation
        /// before passing failed model back to view.
        /// </summary>
        /// <returns>Updated PageOrCreateModel.</returns>
        [NonAction]
        private PageCreateOrEditModel RefillFailedPageModel(PageCreateOrEditModel failedModel)
        {
            PageCreateOrEditModel updatedModel = (PageCreateOrEditModel)((ICloneable) failedModel).Clone();
            if (failedModel.Id > default(int))
            {
                updatedModel.Url = Context.Pages.Single(p => p.Id == failedModel.Id).Url;
            }
            updatedModel.AvailableCategories = GetDefaultSelectCategoriesList();
            updatedModel.AvailableCategories.AddRange(GetAvailableCategoriesList());
            updatedModel.LargeSlideshowImages = updatedModel.LargeSlideshowImages.
                Select(pi => Context.Images.Single(i => i.Id == pi.Id)).
                ToList();
            updatedModel.SmallSlideshowImages = updatedModel.SmallSlideshowImages.
                Select(pi => Context.Images.Single(i => i.Id == pi.Id)).
                ToList();
            return updatedModel;
        }

        // TODO: REFACTOR по жЄсткому
        // TODO: как-то перенести во вью-модели.
        /// <summary>
        /// Updates existing page accodring to the model and save changes to database.
        /// </summary>
        [NonAction]
        private void UpdatePage(Page page, PageCreateOrEditModel model)
        {
            foreach (Image image in model.LargeSlideshowImages)
            {
                if (Context.Images.Local.All(i => i.Id != image.Id))
                {
                    Context.Images.Attach(image);
                }
            }
            foreach (Image image in model.SmallSlideshowImages)
            {
                if (Context.Images.Local.All(i => i.Id != image.Id))
                {
                    Context.Images.Attach(image);
                }
            }
            page.Title = model.Title;
            page.IsDisplayedInLatestPosts = model.IsDisplayedInLatestPosts;
            page.IsPublic = model.IsPublic;
            page.Description = model.Description;
            page.ContentsMarkup = model.ContentsMarkup;
            page.CreateDate = DateTime.Now;
            page.CategoryId = model.CategoryId > 0 ?
                model.CategoryId :
                default(int?);
            page.IsLargeSlideshowEnabled = model.IsLargeSlideshowEnabled;
            page.IsSmallSlideshowEnabled = model.IsSmallSlideshowEnabled;

            List<PageLargeSlideshowImage> largeSlideshowImages = page.
                PageLargeSlideshowImages.ToList();
            foreach (PageLargeSlideshowImage largeSlideshowImage in largeSlideshowImages)
            {
                Context.PageLargeSlideshowImages.Remove(largeSlideshowImage);
            }
            List<PageSmallSlideshowImage> smallSlideshowImages = page.
                PageSmallSlideshowImages.ToList();
            foreach (PageSmallSlideshowImage smallSlideshowImage in smallSlideshowImages)
            {
                Context.PageSmallSlideshowImages.Remove(smallSlideshowImage);
            }

            foreach (Image image in model.LargeSlideshowImages)
            {
                page.PageLargeSlideshowImages.Add(new PageLargeSlideshowImage
                    {
                        Image = Context.Images.
                            Local.Single(li => li.Id == image.Id),
                        Page = page
                    });
            }
            foreach (Image image in model.SmallSlideshowImages)
            {
                page.PageSmallSlideshowImages.Add(new PageSmallSlideshowImage
                    {
                        Image = Context.Images.
                            Local.Single(li => li.Id == image.Id),
                        Page = page
                    });
            }
            Context.SaveChanges();
        }

        // TODO: как-то перенести во ¬ьюћодели.
        /// <summary>
        /// Creates new page according to model.
        /// </summary>
        /// <param name="model">New page will be created according to given model.</param>
        /// <returns>Just created Page instance.</returns>
        [NonAction]
        private Page CreatePageByModel(PageCreateOrEditModel model)
        {
            foreach (Image image in model.LargeSlideshowImages)
            {
                if (Context.Images.Local.All(i => i.Id != image.Id))
                {
                    Context.Images.Attach(image);
                }
            }
            foreach (Image image in model.SmallSlideshowImages)
            {
                if (Context.Images.Local.All(i => i.Id != image.Id))
                {
                    Context.Images.Attach(image);
                }
            }
            Page newPage = new Page
                    {
                        Title = model.Title,
                        Url = model.Url,
                        IsPublic = model.IsPublic,
                        IsDisplayedInLatestPosts = model.IsDisplayedInLatestPosts,
                        Description = model.Description,
                        ContentsMarkup = model.ContentsMarkup,
                        CreateDate = DateTime.Now,
                        CategoryId = model.CategoryId > 0 ? model.CategoryId : default(int?),
                        IsLargeSlideshowEnabled = model.IsLargeSlideshowEnabled,
                        IsSmallSlideshowEnabled = model.IsSmallSlideshowEnabled
                    };
            foreach (Image image in model.LargeSlideshowImages)
            {
                newPage.PageLargeSlideshowImages.Add(new PageLargeSlideshowImage
                    {
                        Image = Context.Images.
                            Local.Single(li => li.Id == image.Id),
                        Page = newPage
                    });
            }
            foreach (Image image in model.SmallSlideshowImages)
            {
                newPage.PageSmallSlideshowImages.Add(new PageSmallSlideshowImage
                    {
                        Image = Context.Images.
                            Local.Single(li => li.Id == image.Id),
                        Page = newPage
                    });
            }

            return newPage;
        }

        [NonAction]
        private List<SelectListItem> GetAvailableCategoriesList()
        {
            var categoriesQuery = Context.Categories.
                Select(n => new
                {
                    Text = n.Name,
                    n.Id
                });
            List<SelectListItem> categoriesList = categoriesQuery.AsEnumerable().
                Select(n => new SelectListItem
                {
                    Text = n.Text,
                    Value = Convert.ToString(n.Id)
                }).ToList();
            return categoriesList;
        }

        /// <summary>
        /// Gets default list of SelectListItems for editing page.
        /// </summary>
        [NonAction]
        private List<SelectListItem> GetDefaultSelectCategoriesList()
        {
            return new List<SelectListItem>
                        {
                            new SelectListItem
                                {
                                    Selected = true,
                                    Text = "NONE",
                                    Value = "-1"
                                }
                        };
        }

        #endregion

    }
}