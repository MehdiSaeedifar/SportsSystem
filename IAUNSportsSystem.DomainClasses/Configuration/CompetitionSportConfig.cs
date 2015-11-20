using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class CompetitionSportConfig : EntityTypeConfiguration<CompetitionSport>
    {
        public CompetitionSportConfig()
        {
            Property(cs => cs.CompetitionId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Unique_CompetitionSport") { IsUnique = true, Order = 1 }));

            Property(cs => cs.SportId)
               .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Unique_CompetitionSport") { IsUnique = true, Order = 2 }));

            Property(cs => cs.SportCategoryId)
               .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Unique_CompetitionSport") { IsUnique = true, Order = 3 }));

            Property(cs => cs.SportDetailId)
               .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Unique_CompetitionSport") { IsUnique = true, Order = 4 }));

            Property(cs => cs.Gender)
               .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Unique_CompetitionSport") { IsUnique = true, Order = 5 }));


            HasRequired(ps => ps.Competition)
                .WithMany(c => c.PresentedSports)
                .HasForeignKey(ps => ps.CompetitionId)
                .WillCascadeOnDelete(false);

            HasRequired(ps => ps.Sport)
                .WithMany(s => s.PresentedSports)
                .HasForeignKey(ps => ps.SportId)
                .WillCascadeOnDelete(false);

            HasOptional(ps => ps.SportCategory)
                .WithMany(sc => sc.PresentedSports)
                .HasForeignKey(ps => ps.SportCategoryId)
                .WillCascadeOnDelete(false);

            HasOptional(ps => ps.SportDetail)
               .WithMany(sd => sd.PresentedSports)
               .HasForeignKey(ps => ps.SportDetailId)
               .WillCascadeOnDelete(false);



        }

    }
}
