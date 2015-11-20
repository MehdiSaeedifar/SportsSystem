using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.Models
{
    public class RunningCompetitionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsReadyActive { get; set; }
        public bool IsRegisterActive { get; set; }
        public bool IsPrintCardActive { get; set; }
    }


    public enum CompetitionStatus
    {
        Ready,
        Register
    }
}
