using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DotDesign.WebUI.Areas.Cms.Filters
{
    public class ValidateOnlyIncomingValuesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ModelStateDictionary modelState = filterContext.Controller.ViewData.ModelState;
            IValueProvider valueProvider = filterContext.Controller.ValueProvider;
            IEnumerable<String> keysWithNoIncomingValue = modelState.Keys.
                Where(x => !valueProvider.ContainsPrefix(x));
            foreach (String key in keysWithNoIncomingValue)
            {
                modelState[key].Errors.Clear();
            }
        }
    }
}