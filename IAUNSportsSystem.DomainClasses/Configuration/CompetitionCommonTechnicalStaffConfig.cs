using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class CompetitionCommonTechnicalStaffConfig : EntityTypeConfiguration<CompetitionCommonTechnicalStaff>
    {
        public CompetitionCommonTechnicalStaffConfig()
        {
            HasRequired(c => c.Competition)
                .WithMany(c => c.CompetitionCommonTechnicalStaffs)
                .HasForeignKey(c => c.CompetitonId)
                .WillCascadeOnDelete(false);
        }
    }
}
