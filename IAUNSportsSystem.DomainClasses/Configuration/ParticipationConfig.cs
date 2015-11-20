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
    public class ParticipationConfig : EntityTypeConfiguration<Participation>
    {
        public ParticipationConfig()
        {

            Property(p => p.RepresentativeUserId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Single_UserPresentedSport") { IsUnique = true, Order = 1 }));

            Property(p => p.PresentedSportId)
            .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Single_UserPresentedSport") { IsUnique = true, Order = 2 }));

            

            HasRequired(p => p.RepresentativeUser)
                .WithMany(ru => ru.Participates)
                .HasForeignKey(p => p.RepresentativeUserId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.PresentedSport)
                .WithMany(ps => ps.Participates)
                .HasForeignKey(p => p.PresentedSportId)
                .WillCascadeOnDelete(false);



        }
    }
}
