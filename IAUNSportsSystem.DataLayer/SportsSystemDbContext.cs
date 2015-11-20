using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EFSecondLevelCache;
using IAUNSportsSystem.DomainClasses.Configuration;

namespace IAUNSportsSystem.DataLayer
{
    public class SportsSystemDbContext : DbContext, IDbContext
    {

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<CompetitionSport> PresentedSports { get; set; }
        public DbSet<SportDetail> SportDetails { get; set; }
        public DbSet<SportCategory> SportCategories { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<RepresentativeUser> RepresentativeUsers { get; set; }
        public DbSet<Participation> Participates { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<TechnicalStaff> TechnicalStaves { get; set; }
        public DbSet<StudyField> StudyFields { get; set; }
        public DbSet<StudyFieldDegree> StudyFieldDegrees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SlideShowItem> SlideShowItems { get; set; }
        public DbSet<CompetitionCommonTechnicalStaff> CompetitionCommonTechnicalStaffs { get; set; }
        public DbSet<Dorm> Dorms { get; set; }
        public DbSet<CompetitionRepresentativeUser> CompetitionRepresentativeUsers { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<TechnicalStaffRole> TechnicalStaffRoles { get; set; }
        public DbSet<ParticipationTechnicalStaff> ParticipationTechnicalStaves { get; set; }

        public override int SaveChanges()
        {
            return SaveAllChanges(invalidateCacheDependencies: true);
        }

        public int SaveAllChanges(bool invalidateCacheDependencies = true)
        {
            var changedEntityNames = GetChangedEntityNames();
            var result = base.SaveChanges();
            if (invalidateCacheDependencies)
            {
                new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            }
            return result;
        }

        public async override Task<int> SaveChangesAsync()
        {
            return await SaveAllChangesAsync(invalidateCacheDependencies: true);
        }

        public async Task<int> SaveAllChangesAsync(bool invalidateCacheDependencies = true)
        {
            var changedEntityNames = GetChangedEntityNames();
            var result = await base.SaveChangesAsync();
            if (invalidateCacheDependencies)
            {
                new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            }
            return result;
        }


        private string[] GetChangedEntityNames()
        {
            return this.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added ||
                            x.State == EntityState.Modified ||
                            x.State == EntityState.Deleted)
                .Select(x => ObjectContext.GetObjectType(x.Entity.GetType()).FullName)
                .Distinct()
                .ToArray();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Conventions.Remove<ForeignKeyIndexConvention>();

            modelBuilder.Configurations.Add(new CompetitionConfig());
            modelBuilder.Configurations.Add(new CompetitorConfig());
            modelBuilder.Configurations.Add(new ParticipationConfig());
            modelBuilder.Configurations.Add(new CompetitionSportConfig());
            modelBuilder.Configurations.Add(new RepresentativeUserConfig());
            modelBuilder.Configurations.Add(new SportConfig());
            modelBuilder.Configurations.Add(new SportCategoryConfig());
            modelBuilder.Configurations.Add(new SportDetailConfig());
            modelBuilder.Configurations.Add(new CompetitionCommonTechnicalStaffConfig());
            modelBuilder.Configurations.Add(new AnnouncementConfig());
            modelBuilder.Configurations.Add(new DormConfig());
            modelBuilder.Configurations.Add(new NewsConfig());
            modelBuilder.Configurations.Add(new ParticipationTechnicalStaffConfig());
            modelBuilder.Configurations.Add(new SlideShowItemConfig());
            modelBuilder.Configurations.Add(new StudyFieldConfig());
            modelBuilder.Configurations.Add(new StudyFieldDegreeConfig());
            modelBuilder.Configurations.Add(new TeamColorConfig());
            modelBuilder.Configurations.Add(new TechnicalStaffConfig());
            modelBuilder.Configurations.Add(new TechnicalStaffRoleConfig());
            modelBuilder.Configurations.Add(new UniversityConfig());
            modelBuilder.Configurations.Add(new UserConfig());



            base.OnModelCreating(modelBuilder);
        }




        #region IUnitOfWork Members
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        #endregion
    }
}
