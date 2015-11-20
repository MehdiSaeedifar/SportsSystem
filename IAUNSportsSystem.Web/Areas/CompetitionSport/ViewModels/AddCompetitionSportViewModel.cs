using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.Web.Areas.CompetitionSport.ViewModels
{
    public class AddCompetitionSportViewModel
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int SportId { get; set; }
        public Gender Gender { get; set; }
        public int MaxCompetitors { get; set; }
        public int MaxTechnicalStaffs { get; set; }
        public int? SportCategoryId { get; set; }
        public int? SportDetailId { get; set; }
        public bool HasRule { get; set; }
        public string Rule { get; set; }
        public bool IsIndividual { get; set; }
    }

    public class AddPresentedSportDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AddSportViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AddSportCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}