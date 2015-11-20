using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Iris.Web.IrisMembership;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Infrastructure
{
    public static class WebViewPageExtensions
    {
        public static IrisPrincipal IrisUser(this WebViewPage viewPage)
        {
            return viewPage.User.Identity as IrisPrincipal;
        }
    }
}