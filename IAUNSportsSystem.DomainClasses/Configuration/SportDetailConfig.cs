using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class SportDetailConfig : EntityTypeConfiguration<SportDetail>
    {
        public SportDetailConfig()
        {
            HasRequired(sd => sd.Sport)
                .WithMany(s => s.SportDetails)
                .HasForeignKey(sd => sd.SportId)
                .WillCascadeOnDelete(false);

            Property(sd => sd.Name).HasMaxLength(100).IsRequired();
        }
    }
}
