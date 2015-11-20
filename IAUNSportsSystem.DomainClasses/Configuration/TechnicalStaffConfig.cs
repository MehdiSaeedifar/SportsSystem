using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class TechnicalStaffConfig:EntityTypeConfiguration<TechnicalStaff>
    {
        public TechnicalStaffConfig()
        {
            Property(c => c.FirstName).HasMaxLength(40);
            Property(c => c.LastName).HasMaxLength(80);
            Property(c => c.FatherName).HasMaxLength(40);
            Property(c => c.NationalCode).HasMaxLength(12);
            Property(c => c.MobileNumber).HasMaxLength(20);

            Property(ts => ts.BirthDate).HasColumnType("date");

            Property(ts => ts.Image).HasMaxLength(400);

            Property(ts => ts.Error).HasMaxLength(1000);

            Property(ts => ts.DormNumber).HasMaxLength(50);
        }
    }
}
