using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IAUNSportsSystem.Utilities
{
    public static class ExtenstionMethods
    {
        public static string RemoveHtmlTags(this string input)
        {
            return input == null ? string.Empty : Regex.Replace(input, "<.*?>", String.Empty);
        }

        
    }
}
