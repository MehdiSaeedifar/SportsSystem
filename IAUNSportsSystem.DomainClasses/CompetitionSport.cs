using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses
{
    public enum Gender
    {
        [Display(Name = "مرد")]
        Male,
        [Display(Name = "زن")]
        Female
    }

    public class CompetitionSport
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        public int SportId { get; set; }
        public virtual Sport Sport { get; set; }
        public Gender Gender { get; set; }
        public int MaxCompetitors { get; set; }
        public int MaxTechnicalStaff { get; set; }
        public bool HasRule { get; set; }
        public string Rule { get; set; }
        public bool IsIndividual { get; set; }
        public int? SportCategoryId { get; set; }
        public virtual SportCategory SportCategory { get; set; }
        public int? SportDetailId { get; set; }
        public virtual SportDetail SportDetail { get; set; }
        public virtual ICollection<Participation> Participates { get; set; }
    }
}
