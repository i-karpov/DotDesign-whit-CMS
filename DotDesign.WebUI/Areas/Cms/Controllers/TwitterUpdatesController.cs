using DotDesign.DataLayer;
using DotDesign.Domain.Entities;
using System;
using System.Linq;
using System.Web.Mvc;
using DotDesign.WebUI.Utility;
using PagedList;

namespace DotDesign.WebUI.Areas.Cms.Controllers
{   
    [Authorize]
    public class TwitterUpdatesController : Controller
    {
        private DotDesignCmsContext Context
        {
            get;
            set;
        }

        private readonly PagingSettings pagingSettings;

        public TwitterUpdatesController(DotDesignCmsContext context)
        {
            this.Context = context;
            this.pagingSettings = Config.CmsPagingSettings;
        }

        public ViewResult AllTwitterUpdates(int? pageNumber)
        {
            return View(Context.TwitterUpdates.OrderBy(tu => tu.Id).
                ToPagedList(pagingSettings.EnsurePageNumber(pageNumber, Context.TwitterUpdates.Count()),
                    pagingSettings.ItemsPerPageCount));
        }

        public ActionResult Details(int id)
        {
            TwitterUpdate twitterUpdate = Context.TwitterUpdates.
                SingleOrDefault(x => x.Id == id);
            if (twitterUpdate == null)
            {
                return RedirectToRoute("HttpError404");
            }
            return View(twitterUpdate);
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TwitterUpdate twitterUpdate)
        {
            if (ModelState.IsValid)
            {
                twitterUpdate.CreateDate = DateTime.Now;
                Context.TwitterUpdates.Add(twitterUpdate);
                Context.SaveChanges();
                return RedirectToAction("AllTwitterUpdates");  
            }

            return View(twitterUpdate);
        }

        public ActionResult Edit(int id)
        {
            TwitterUpdate twitterUpdate = Context.TwitterUpdates.SingleOrDefault(x => x.Id == id);
            if (twitterUpdate == null)
            {
                return RedirectToRoute("HttpError404");
            }
            return View(twitterUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TwitterUpdate twitterUpdate, String returnUrl)
        {
            if (ModelState.IsValid)
            {
                TwitterUpdate oldTwitterUpdate = Context.TwitterUpdates.
                    Single(tu => tu.Id == twitterUpdate.Id);
                oldTwitterUpdate.Text = twitterUpdate.Text;
                oldTwitterUpdate.IsPublic = twitterUpdate.IsPublic;
                Context.SaveChanges();
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("AllTwitterUpdates");
            }
            return View(twitterUpdate);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Context.TwitterUpdates.All(tu => tu.Id != id))
            {
                return RedirectToRoute("HttpError404");
            }
            return PartialView("_Delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, String returnUrl)
        {
            TwitterUpdate twitterUpdate = Context.TwitterUpdates.SingleOrDefault(x => x.Id == id);
            if (twitterUpdate == null)
            {
                return RedirectToRoute("HttpError404");
            }
            Context.TwitterUpdates.Remove(twitterUpdate);
            Context.SaveChanges();
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("AllTwitterUpdates");
        }
    }
}