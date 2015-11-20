using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DomainClasses.Configuration
{
    public class CompetitorConfig : EntityTypeConfiguration<Competitor>
    {
        public CompetitorConfig()
        {
            HasRequired(c => c.Participate)
                .WithMany(p => p.Competitors)
                .HasForeignKey(c => c.ParticipateId)
                .WillCascadeOnDelete(false);

            HasRequired(c => c.StudyField)
                .WithMany(sf => sf.Competitors)
                .HasForeignKey(c => c.StudyFieldId)
                .WillCascadeOnDelete(false);

            HasRequired(c => c.StudyFieldDegree)
                .WithMany(sfd => sfd.Competitors)
                .HasForeignKey(c => c.StudyFieldDegreeId)
                .WillCascadeOnDelete(false);

            Property(c => c.BirthDate).HasColumnType("date");
            Property(c => c.InsuranceEndDate).HasColumnType("date");

            Property(c => c.FirstName).HasMaxLength(40);
            Property(c => c.LastName).HasMaxLength(80);
            Property(c => c.FatherName).HasMaxLength(40);

            Property(c => c.NationalCode).HasMaxLength(12);

            Property(c => c.StudentNumber).HasMaxLength(15);

            Property(c => c.Email).HasMaxLength(100);

            Property(c => c.MobileNumber).HasMaxLength(20);

            Property(c => c.PhoneNumber).HasMaxLength(20);

            Property(c => c.Weight).IsOptional();
            Property(c => c.Height).IsOptional();

            Property(c => c.InsuranceNumber).HasMaxLength(15);


            Property(c => c.UserImage).HasMaxLength(400);
            Property(c => c.InsuranceImage).HasMaxLength(400);
            Property(c => c.AzmoonConfirmationImage).HasMaxLength(400);
            Property(c => c.StudentCertificateImage).HasMaxLength(400);

            Property(c => c.Error).HasMaxLength(1000);

            Property(c => c.DormNumber).HasMaxLength(50);

        }
    }
}
