using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAUNSportsSystem.Web.Areas.SlideShow.ViewModels
{
    public class SlideShowIitemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
    }
}