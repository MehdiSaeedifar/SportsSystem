using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class SlideShowItemConfig : EntityTypeConfiguration<SlideShowItem>
    {
        public SlideShowItemConfig()
        {
            Property(x => x.Image).HasMaxLength(400).IsRequired();
            Property(x => x.Link).HasMaxLength(800);
            Property(x => x.Text).HasMaxLength(500);
            Property(x => x.Title).HasMaxLength(200);
        }
    }
}
