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
    public class RepresentativeUserConfig : EntityTypeConfiguration<RepresentativeUser>
    {
        public RepresentativeUserConfig()
        {
            Property(u => u.Email).HasMaxLength(120).IsRequired();
            Property(u => u.Email)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Unique_RepresentativeUser_Email") { IsUnique = true }));

            Property(c => c.FirstName).HasMaxLength(40);
            Property(c => c.LastName).HasMaxLength(80);
            Property(c => c.FatherName).HasMaxLength(40);

            Property(c => c.NationalCode).HasMaxLength(12);
            Property(c => c.MobileNumber).HasMaxLength(20);

            Property(c => c.Password).HasMaxLength(100).IsRequired();

        }
    }
}
