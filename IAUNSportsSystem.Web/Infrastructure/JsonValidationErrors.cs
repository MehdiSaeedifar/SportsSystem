using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Infrastructure
{
    public static class ControllerExtensionMethods
    {
        public static ActionResult JsonValidationErrors(this Controller controller)
        {
            //var modelState = context.Controller.ViewData.ModelState;

            if (controller.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(200);
            }



            var errorList = (from item in controller.ModelState.Values
                             from error in item.Errors
                             select error.ErrorMessage).ToList();

            controller.Response.StatusCode = 400;

            return new JsonResult()
            {
                Data = errorList,
                JsonRequestBehavior = JsonRequestBehavior.DenyGet,

            };

        }
    }
}