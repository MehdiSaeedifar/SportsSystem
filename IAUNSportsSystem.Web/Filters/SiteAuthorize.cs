using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Org.BouncyCastle.Crypto.Modes;

namespace IAUNSportsSystem.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class SiteAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                throw new UnauthorizedAccessException(); //to avoid multiple redirects
            }

            HandleAjaxRequest(filterContext);
            base.HandleUnauthorizedRequest(filterContext);
        }

        private static void HandleAjaxRequest(AuthorizationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            if (!ctx.Request.IsAjaxRequest())
                return;

            ctx.Response.SuppressFormsAuthenticationRedirect = true;
            ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
    }
}