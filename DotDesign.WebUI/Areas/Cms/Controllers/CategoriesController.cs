using DotDesign.DataLayer;
using DotDesign.Domain.Entities;
using System;
using System.Linq;
using System.Web.Mvc;
using DotDesign.WebUI.Areas.Cms.Filters;
using DotDesign.WebUI.Areas.Cms.ViewModels;
using DotDesign.WebUI.Extensions;
using DotDesign.WebUI.Utility;
using PagedList;

namespace DotDesign.WebUI.Areas.Cms.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly PagingSettings pagingSettings;

        private DotDesignCmsContext Context
        {
            get;
            set;
        }

        public CategoriesController(DotDesignCmsContext context)
        {
            this.Context = context;
            this.pagingSettings = Config.CmsPagingSettings;
        }

        public ViewResult AllCategories(int? pageNumber)
        {
            return View(Context.Categories.OrderBy(c => c.Id).
                ToPagedList(pagingSettings.EnsurePageNumber(pageNumber, Context.Categories.Count()),
                    pagingSettings.ItemsPerPageCount));
        }

        public ActionResult Details(int id)
        {
            Category category = Context.Categories.SingleOrDefault(x => x.Id == id);
            if (category == null)
            {
                return RedirectToRoute("HttpError404");
            }
            return View(new CategoryModel(category));
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                model.Url = model.Url.ReplaceWhiteSpacesWithHyphens().ToLower();
                if (!Context.Categories.Any(c => c.Url.ToLower() == model.Url.ToLower()))
                {
                    Category newCategory = new Category
                                               {
                                                   CreateDate = DateTime.Now
                                               };
                    model.UpdateCategory(newCategory);
                    Context.Categories.Add(newCategory);
                    Context.SaveChanges();
                    return RedirectToAction("AllCategories");
                }
                ModelState.AddModelError("Name", Config.TextErrorMessage);
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Category category = Context.Categories.SingleOrDefault(c => c.Id == id);
            if (category == null)
            {
                return RedirectToRoute("HttpError404");
            }
            return View(new CategoryModel(category));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateOnlyIncomingValues]
        public ActionResult Edit(CategoryModel model, String returnUrl)
        {
            Category oldCategory = Context.Categories.
                        SingleOrDefault(c => c.Id == model.Id);
            if (oldCategory == null)
            {
                return RedirectToRoute("HttpError404");
            }
            if (ModelState.IsValid)
            {
                oldCategory.Name = model.Name;
                oldCategory.IsPublic = model.IsPublic;
                Context.SaveChanges();

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("AllCategories");
            }
            model.Url = oldCategory.Url;
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Category category = Context.Categories.
                SingleOrDefault(c => c.Id == id);
            if (category == null)
            {
                return RedirectToRoute("HttpError404");
            }
            return PartialView("_Delete", new CategoryModel(category));
        }

        /// <summary>
        /// Deletes all related foreign keys in Pages table first, then deletes required category.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, String returnUrl)
        {
            Category category = Context.Categories.SingleOrDefault(c => c.Id == id);
            if (category == null)
            {
                return RedirectToRoute("HttpError404");
            }
            foreach(Page page in category.Pages)
            {
                page.CategoryId = null;
            }
            Context.Categories.Remove(category);
            Context.SaveChanges();

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("AllCategories");
        }

    }
}