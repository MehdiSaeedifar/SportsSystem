using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class DormConfig:EntityTypeConfiguration<Dorm>
    {
        public DormConfig()
        {
            Property(d => d.Name).HasMaxLength(100).IsRequired();
        }
    }
}
