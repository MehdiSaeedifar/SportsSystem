using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class CompetitionConfig : EntityTypeConfiguration<Competition>
    {
        public CompetitionConfig()
        {
            Property(c => c.Name).IsRequired().HasMaxLength(250);

            Property(c => c.ReadyStartDate).HasColumnType("date").IsOptional();
            Property(c => c.ReadyEndDate).HasColumnType("date").IsOptional();

            Property(c => c.RegisterStartDate).HasColumnType("date").IsOptional();
            Property(c => c.RegisterEndDate).HasColumnType("date").IsOptional();

            Property(c => c.PrintCardStartDate).HasColumnType("date").IsOptional();
            Property(c => c.PrintCardEndDate).HasColumnType("date").IsOptional();


            Property(c => c.LogoImage).HasMaxLength(400);
        }
    }
}
