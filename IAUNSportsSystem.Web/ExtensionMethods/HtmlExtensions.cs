using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAUNSportsSystem.Web.ExtensionMethods
{
    public static class HtmlExtensions
    {
        public static IHtmlString HtmlRaw(this string input)
        {
            return input == null ? new HtmlString(string.Empty) : (IHtmlString) new HtmlString(input);
        }
    }
}