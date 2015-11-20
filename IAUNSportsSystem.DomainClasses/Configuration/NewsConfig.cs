using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class NewsConfig : EntityTypeConfiguration<News>
    {
        public NewsConfig()
        {
            Property(n => n.Title).HasMaxLength(500).IsRequired();
        }
    }
}
