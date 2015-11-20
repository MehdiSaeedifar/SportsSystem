using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Web;
using System.Web.Mvc;

namespace IAUNSportsSystem.Web.Areas.Competition.ViewModels
{
    public class CompetitionViewModel
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "وارد کردن عنوان مسابقه ضروری است.")]
        //[MaxLength(500,ErrorMessage = "عنوان مسابقه حداکثر 250 حرف می تواند باشد.")]
        //[MinLength(5,ErrorMessage = "عنوان مسابقه حداقل 5 حرف می تواند باشد.")]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsReadyActive { get; set; }
        public bool IsRegisterActive { get; set; }
        public bool IsPrintCardActive { get; set; }
        public DateTime? ReadyStartDate { get; set; }
        public DateTime? ReadyEndDate { get; set; }
        public DateTime? RegisterStartDate { get; set; }
        public DateTime? RegisterEndDate { get; set; }
        public DateTime? PrintCardStartDate { get; set; }
        public DateTime? PrintCardEndDate { get; set; }
        public string LogoImage { get; set; }
        [AllowHtml]
        public string Rule { get; set; }
        public int MaxCommonTechnicalStaffs { get; set; }
    }

    public class AddEditPresentedSportViewModel
    {
       
    }

    

}