using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Web;

namespace IAUNSportsSystem.Web.ViewModels
{
    public class PresianDateViewModel
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            return string.Format("{0}/{1}/{2}", Year, Month, Day);
        }
    }
}