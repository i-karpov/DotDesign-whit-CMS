using DotDesign.DataLayer;
using DotDesign.WebUI.ViewModels;
using DotDesign.Domain.Entities;
using DotDesign.WebUI.Areas.Cms.ViewModels;
using DotDesign.WebUI.SearchEngine;
using DotDesign.WebUI.Utility;
using DotDesign.WebUI.ViewModels;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using Page = DotDesign.Domain.Entities.Page;

namespace DotDesign.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly PagingSettings pagingSettings;

        private DotDesignCmsContext Context
        {
            get;
            set;
        }

        public HomeController(DotDesignCmsContext context)
        {
            this.Context = context;
            this.pagingSettings = Config.ClientPagingSettings;
        }

        //[OutputCache(Duration = 3600,
        //             Location = OutputCacheLocation.Server,
        //             VaryByParam = "*")]
        public ActionResult Index()
        {
            HomePage homePage = Context.HomePages.FirstOrDefault();
            if (homePage == null ||
                !homePage.Page.IsPublic)
            {
                return View(new ContentsPageModel(null, GetCommonPartForPage()));
            }
            return View(new ContentsPageModel(homePage.Page, GetCommonPartForPage()));
        }

        //[OutputCache(Duration = 3600, 
        //             Location = OutputCacheLocation.Server,
        //             VaryByParam = "*")]
        public ActionResult Page(String pageUrl)
        {
            Page page = Context.Pages.SingleOrDefault(p => p.Url == pageUrl &&
                                                           p.IsPublic);
            if (page == null)
            {
                return RedirectToRoute("HttpError404");
            }
            ContentsPageModel model = new ContentsPageModel(page,
                                            GetCommonPartForPage());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubscribeToNewsletter(SubscriberModel model)
        {
            String returnViewName = "_SubscriptionFailed";
            if (ModelState.IsValid &&
                    Context.Subscribers.All(s => s.Email != model.Email))
            {
                Subscriber subscriber = new Subscriber
                                            {
                                                CreateDate = DateTime.Now
                                            };
                model.UpdateSubscriber(subscriber);
                Context.Subscribers.Add(subscriber);
                Context.SaveChanges();
                returnViewName = "_SubscriptionCompleted";
                if (Request.IsAjaxRequest())
                {
                    return PartialView(returnViewName);
                }
                return View(returnViewName);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(returnViewName);
            }
            return View(returnViewName);
        }

        /// <summary>
        /// Fetches all pages related to selected category item.
        /// </summary>
        //[OutputCache(Duration = 3600, 
        //             Location = OutputCacheLocation.Server,
        //             VaryByParam = "*")]
        public ActionResult Category(String categoryUrl, int? pageNumber)
        {
            Category selectedCategory = Context.Categories.
                SingleOrDefault(nl => nl.IsPublic && 
                                      nl.Url.ToLower() == categoryUrl.ToLower());
            if (selectedCategory == null)
            {
                return RedirectToRoute("HttpError404");
            }

            // TODO: move to repository...
            IQueryable<Page> relatedPublicPagesQuery = Context.Pages.
                                                                 Where(p => p.IsPublic &&
                                                                            p.CategoryId == selectedCategory.Id);
            if (relatedPublicPagesQuery.Count() == 1)
            {
                return RedirectToAction("Page", new
                                                {
                                                    pageUrl = relatedPublicPagesQuery.
                                                                First().
                                                                Url
                                                });
            }

            CategoryPageModel briefReviewsPageModel = new CategoryPageModel(GetCommonPartForPage(), categoryUrl)
                {
                    CategoryName = selectedCategory.Name,
                    BriefReviews = relatedPublicPagesQuery.
                        OrderByDescending(p => p.Id).
                        Select(p => new BriefReview
                                        {
                                            Title = p.Title,
                                            Description = p.Description,
                                            Url = p.Url,
                                            CategoryUrl = p.CategoryId.HasValue ?
                                                        p.Category.Url.ToLower() :
                                                        null
                                        }).
                        ToPagedList(pagingSettings.EnsurePageNumber(pageNumber,
                                                                    relatedPublicPagesQuery.Count()),
                                    pagingSettings.ItemsPerPageCount)
                };
            return View(briefReviewsPageModel);
        }

        /// <summary>
        /// Fetches all pages that satisfy submitted query.
        /// </summary>
        //[OutputCache(Duration = 3600, 
        //             Location = OutputCacheLocation.Server,
        //             VaryByParam = "*")]
        public ViewResult Search(String searchQuery, int? pageNumber)
        {
            int[] satisfyingPagesIds = PageIndexHelper.Find(searchQuery).
                                               Select(p => p.Id).
                                               ToArray();

            IQueryable<Page> satisfyingPagesQuery = Context.Pages.
                                                            Where(p => p.IsPublic &&
                                                                       satisfyingPagesIds.Contains(p.Id));

            SearchResultsPageModel pageModel = new SearchResultsPageModel(GetCommonPartForPage(),
                                                                          searchQuery)
                    {
                        BriefReviews = satisfyingPagesQuery.
                                OrderByDescending(p => p.Id).
                                Select(p => new BriefReview
                                                {
                                                    Title = p.Title,
                                                    Description = p.Description,
                                                    Url = p.Url,
                                                    CategoryUrl = p.CategoryId.HasValue ?
                                                                p.Category.Url.ToLower() :
                                                                null
                                                }).
                                ToPagedList(pagingSettings.EnsurePageNumber(pageNumber ??
                                                                                PagingSettings.FirstPageNumber,
                                                                            satisfyingPagesQuery.Count()),
                                            pagingSettings.ItemsPerPageCount)
                    };

            return View(pageModel);
        }

        /// <summary>
        /// Prepares pages common part (navbar, footer, subscriber's form)
        /// </summary>
        [NonAction]
        private PagesCommonPart GetCommonPartForPage()
        {
            return new PagesCommonPart
                {
                    Categories = Context.Categories.Where(nl => nl.IsPublic).ToList(),
                    LatestPosts = Context.Pages.Where(p => p.IsPublic &&
                                                           p.IsDisplayedInLatestPosts).
                                        OrderByDescending(p => p.Id).
                                        Take(5).
                                        Select(p => new LatestPostModel
                                                        {
                                                            Title = p.Title,
                                                            Url = p.Url,
                                                            CategoryUrl = p.CategoryId.HasValue ?
                                                                p.Category.Url.ToLower() :
                                                                null
                                                        }).ToList(),
                    TwitterUpdates = Context.TwitterUpdates.
                                        Where(tu => tu.IsPublic).
                                        OrderByDescending(tu => tu.Id).
                                        Take(5).
                                        ToList().
                                        Select(twitterUpdate => new TwitterUpdateModel(twitterUpdate)).
                                        ToList(),
                    Subscriber = new Subscriber()
                };
        }
    }
}
