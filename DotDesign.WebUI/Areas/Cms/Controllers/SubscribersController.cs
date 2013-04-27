using DotDesign.DataLayer;
using DotDesign.Domain.Entities;
using System;
using System.Linq;
using System.Web.Mvc;
using DotDesign.WebUI.Areas.Cms.ViewModels;
using DotDesign.WebUI.Utility;
using PagedList;

namespace DotDesign.WebUI.Areas.Cms.Controllers
{   
    [Authorize]
    public class SubscribersController : Controller
    {
        private DotDesignCmsContext Context
        {
            get;
            set;
        }

        private readonly PagingSettings pagingSettings;

        public SubscribersController(DotDesignCmsContext context)
        {
            this.Context = context;
            this.pagingSettings = Config.CmsPagingSettings;
        }

        public ViewResult AllSubscribers(int? pageNumber)
        {
            return View(Context.Subscribers.OrderBy(s => s.Id).
                ToPagedList(pagingSettings.EnsurePageNumber(pageNumber, Context.Subscribers.Count()),
                    pagingSettings.ItemsPerPageCount));
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubscriberModel model)
        {
            if (ModelState.IsValid)
            {
                Subscriber newSubscriber = new Subscriber
                                           {
                                               CreateDate = DateTime.Now
                                           };
                model.UpdateSubscriber(newSubscriber);
                Context.Subscribers.Add(newSubscriber);
                Context.SaveChanges();
                return RedirectToAction("AllSubscribers");  
            }
            return View(model);
        }
        
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Subscriber subscriber = Context.Subscribers.SingleOrDefault(s => s.Id == id);
            if (subscriber == null)
            {
                return RedirectToRoute("HttpError404");
            }
            return PartialView("_Delete", new SubscriberModel(subscriber));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, String returnUrl)
        {
            Subscriber subscriber = Context.Subscribers.SingleOrDefault(s => s.Id == id);
            if (subscriber == null)
            {
                return RedirectToRoute("HttpError404");
            }
            Context.Subscribers.Remove(subscriber);
            Context.SaveChanges();
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("AllSubscribers");
        }

    }
}