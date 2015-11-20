using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class UserConfig:EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            Property(c => c.FirstName).HasMaxLength(40);
            Property(c => c.LastName).HasMaxLength(80);

            Property(u => u.Email).HasMaxLength(120).IsRequired();

            Property(u => u.Password).HasMaxLength(400).IsRequired();
        }
    }
}
