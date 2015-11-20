using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAUNSportsSystem.Web.Areas.Sport.ViewModels
{
    public class AddSportViewModel
    {
        public string SportName { get; set; }
        public SportCategoryViewModel[] SportCategories { get; set; }
        public SportDetailViewModel[] SportDetails { get; set; }
    }

    public class SportDetailViewModel
    {
        public string Name { get; set; }
    }

    public class SportCategoryViewModel
    {
        public string Name { get; set; }
    }

    public class SportViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
}