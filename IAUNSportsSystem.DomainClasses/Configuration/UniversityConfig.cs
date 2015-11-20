using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class UniversityConfig:EntityTypeConfiguration<University>
    {
        public UniversityConfig()
        {
            Property(x => x.Name).HasMaxLength(150).IsRequired();
        }
    }
}
