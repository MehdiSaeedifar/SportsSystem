using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class TeamColorConfig : EntityTypeConfiguration<TeamColor>
    {
        public TeamColorConfig()
        {
            Property(tc => tc.Name).HasMaxLength(30);
        }
    }
}
