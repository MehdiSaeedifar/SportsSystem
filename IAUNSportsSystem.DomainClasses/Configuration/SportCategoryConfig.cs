using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class SportCategoryConfig : EntityTypeConfiguration<SportCategory>
    {
        public SportCategoryConfig()
        {
            HasRequired(sc => sc.Sport)
                .WithMany(s => s.SportCategories)
                .HasForeignKey(s => s.SportId)
                .WillCascadeOnDelete(true);

            Property(sc => sc.Name).HasMaxLength(100).IsRequired();
        }
    }
}
