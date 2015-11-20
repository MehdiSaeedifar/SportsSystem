using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class AnnouncementConfig : EntityTypeConfiguration<Announcement>
    {
        public AnnouncementConfig()
        {
            Property(a => a.Title).HasMaxLength(100).IsRequired();
            Property(a => a.EmailText).HasMaxLength(5000);
            Property(a => a.SmsText).HasMaxLength(300);
        }
    }
}
