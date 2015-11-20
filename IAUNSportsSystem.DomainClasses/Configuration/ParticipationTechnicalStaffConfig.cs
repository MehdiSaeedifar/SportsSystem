using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class ParticipationTechnicalStaffConfig : EntityTypeConfiguration<ParticipationTechnicalStaff>
    {
        public ParticipationTechnicalStaffConfig()
        {

            HasRequired(pt => pt.Participation)
                .WithMany(pt => pt.ParticipationTechnicalStaves)
                .HasForeignKey(ps => ps.ParticipationId)
                .WillCascadeOnDelete(false);

            HasRequired(pt => pt.TechnicalStaff)
                .WithMany(pt => pt.Participations)
                .HasForeignKey(ps => ps.TechnicalStaffId)
                .WillCascadeOnDelete(false);

            HasRequired(pt => pt.TechnicalStaffRole)
               .WithMany(pt => pt.ParticipationTechnicalStaves)
               .HasForeignKey(ps => ps.TechnicalStaffRoleId)
               .WillCascadeOnDelete(false);
        }
    }
}
