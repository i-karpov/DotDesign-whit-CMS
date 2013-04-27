using System.Drawing;
using System;
using System.Linq;
using System.Web.Mvc;
using DotDesign.DataLayer;
using DotDesign.WebUI.Extensions;
using DotDesign.WebUI.Utility;
using PagedList;
using Image = DotDesign.Domain.Entities.Image;
using DotDesign.WebUI.Areas.Cms.ViewModels;

namespace DotDesign.WebUI.Areas.Cms.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private const String ImageContentType = "image/jpeg";

        private DotDesignCmsContext Context
        {
            get;
            set;
        }

        private readonly PagingSettings pagingSettings;

        public ImagesController(DotDesignCmsContext context)
        {
            this.Context = context;
            this.pagingSettings = Config.CmsPagingSettings;
        }

        public ViewResult AllImages(int? pageNumber)
        {
            return View(Context.Images.OrderBy(i => i.Id).
                ToPagedList(pagingSettings.EnsurePageNumber(pageNumber, Context.Images.Count()),
                    pagingSettings.ItemsPerPageCount));
        }

        /// <summary>
        /// Gets an image by the specified image Url.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Image(String imageUrl)
        {
            Image image = Context.Images.SingleOrDefault(i => i.Url == imageUrl);
            String serverFilePath;
            if (image == null || !System.IO.File.Exists(image.ServerFilePath))
            {
                serverFilePath = Server.MapPath(String.Format(Config.ResourceNotFoundImageRelativePath));
            }
            else
            {
                serverFilePath = image.ServerFilePath;
            }

            return File(serverFilePath, ImageContentType);
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(UploadImageModel model)
        {
            if (ModelState.IsValid)
            {
                model.Url = model.Url.ReplaceWhiteSpacesWithHyphens().ToLower();
                if (!Context.Images.Any(i => i.Url.ToLower() == model.Url.ToLower()))
                {
                    Bitmap bitmap = System.Drawing.Image.FromStream(model.ImageData.InputStream) as Bitmap;
                    if (bitmap != null)
                    {
                        int newId = Context.Images.Any()
                                        ? Context.Images.
                                              OrderByDescending(i => i.Id).First().Id + 1
                                        : 1;

                        String relativeFilePath = String.Format("/Content/Images/image-{0}.jpeg", newId);

                        String serverFilePath = Server.MapPath(String.Format("~{0}", relativeFilePath));
                        bitmap.Save(serverFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                        Image image = new Image
                                          {
                                              ServerFilePath = serverFilePath,
                                              Url = model.Url,
                                              CreateDate = DateTime.Now
                                          };
                        Context.Images.Add(image);
                        Context.SaveChanges();

                        return RedirectToAction("AllImages");
                    }
                    ModelState.AddModelError("ImageData", Config.ImageDataErrorMessage);
                }
                else
                {
                    ModelState.AddModelError("Url", Config.UrlErrorMessage);
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Image image = Context.Images.SingleOrDefault(x => x.Id == id);
            if (image == null)
            {
                return RedirectToRoute("HttpError404");
            }
            return PartialView("_Delete", image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, String returnUrl)
        {
            
            Image image = Context.Images.SingleOrDefault(x => x.Id == id);
            if (image == null)
            {
                return RedirectToRoute("HttpError404");
            }
            Context.Images.Remove(image);
            Context.SaveChanges();

            if (System.IO.File.Exists(image.ServerFilePath))
            {
                System.IO.File.Delete(image.ServerFilePath);
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("AllImages");
        }
    }
}
