using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DotDesign.WebUI.Extensions
{
    public static class HtmlExtensions
    {
        private const String PageServingControllerName = "Home";
        private const String PageServingActionName = "Page";
        private const String GeneralPageRouteName = "GeneralPage";
        private const String CategorizedPageRouteName = "CategorizedPage";

        public static MvcHtmlString PageLink(this HtmlHelper htmlHelper,
            String linkText, String pageTitle, String categoryUrl, String pageUrl)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary(new
                        {
                            action = PageServingActionName,
                            controller = PageServingControllerName,
                            pageUrl = pageUrl
                        });
            String routeName;
            if (String.IsNullOrEmpty(categoryUrl))
            {
                routeName = GeneralPageRouteName;
            }
            else
            {
                routeValues.Add("categoryUrl", categoryUrl);
                routeName = CategorizedPageRouteName;
            }
            IDictionary<String, Object> htmlAttributes = new Dictionary<String, Object>();
            htmlAttributes.Add("title", pageTitle);
            return htmlHelper.RouteLink(
                linkText,
                routeName,
                routeValues,
                htmlAttributes);
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper,
            String src, Object htmlAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("img");

            tagBuilder.MergeAttribute("src", src);
            tagBuilder.MergeAttribute("alt", String.Empty);

            if (htmlAttributes != null)
            {
                tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}