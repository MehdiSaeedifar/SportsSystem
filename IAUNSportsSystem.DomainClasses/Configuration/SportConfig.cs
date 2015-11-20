using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class SportConfig : EntityTypeConfiguration<Sport>
    {
        public SportConfig()
        {
            Property(s => s.Name).HasMaxLength(100).IsRequired();
        }
    }
}
