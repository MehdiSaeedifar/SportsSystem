using System.Collections.Generic;
using IAUNSportsSystem.DomainClasses;
using IAUNSportsSystem.Utilities;

namespace IAUNSportsSystem.DataLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<IAUNSportsSystem.DataLayer.SportsSystemDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SportsSystemDbContext context)
        {
            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Email = "admin@gmail.com",
                    FirstName = "مدیر",
                    LastName = "فنی",
                    Role = Role.Admin,
                    Password = EncryptionHelper.Encrypt("123admin123", EncryptionHelper.Key)
                }
                );

            context.Universities.AddOrUpdate(u => u.Name,
                new University { Name = "واحد نجف آباد" },
                new University { Name = "واحد دولت آباد" },
                new University { Name = "استان اصفهان" },
                new University { Name = "استان تهران" }
                );

            context.Sports.AddOrUpdate(s => s.Name, new Sport
            {
                Name = "کشتی",
                SportDetails = new List<SportDetail>
                {
                    new SportDetail {Name = "60 کیلوگرم"},
                    new SportDetail {Name = "80 کیلوگرم"}
                },
                SportCategories = new List<SportCategory>
                {
                    new SportCategory {Name = "آزاد"},
                    new SportCategory {Name = "فرنگی"}
                },
            });

            context.Sports.AddOrUpdate(s => s.Name, new Sport
            {
                Name = "فوتسال",
            });

            context.Sports.AddOrUpdate(s => s.Name, new Sport
            {
                Name = "دوچرخه سواری",
                SportCategories = new List<SportCategory>
                {
                    new SportCategory {Name = "1000 متر سرعت"},
                    new SportCategory {Name = "دور حذفی"}
                },
            });

            context.Dorms.AddOrUpdate(d => d.Name,
                new Dorm { Name = "شهید بهشتی" },
                new Dorm { Name = "شهید مطهری" }
                );

            context.StudyFields.AddOrUpdate(s => s.Name,
                new StudyField { Name = "کامپیوتر" },
                new StudyField { Name = "برق" },
                new StudyField { Name = "عمران" }
                );

            context.StudyFieldDegrees.AddOrUpdate(sf => sf.Name,
                new StudyFieldDegree { Name = "کارشناسی" },
                new StudyFieldDegree { Name = "کارشناسی ارشد" },
                new StudyFieldDegree { Name = "کاردانی" }
                );

            context.TechnicalStaffRoles.AddOrUpdate(tr => tr.Name,
                new TechnicalStaffRole { Name = "سرمربی" },
                new TechnicalStaffRole { Name = "سرپرست" },
                new TechnicalStaffRole { Name = "راننده", IsCommon = true }
                );

            context.News.AddOrUpdate(n => n.Title,
                new News
                {
                    Title = "سامانه مسابقات افتتاح شد",
                    CreatedDate = DateTime.Now.AddDays(-1),
                    Body = "سامانه ی خوبیه!"
                },
                new News
                {
                    Title = "قهرمان مسابقات فوتسال مشخص شد",
                    CreatedDate = DateTime.Now,
                    Body = "تیم فوتسال!"
                });

            context.SlideShowItems.AddOrUpdate(s => s.Image,
                new SlideShowItem
                {
                    Title = "دانشگاه آزاد اسلامی واحد نجف آباد",
                    Image = "1.jpg",
                    Order = 10,
                },
                new SlideShowItem
                {
                    Image = "2.jpg",
                    Order = 9
                });


            context.SaveChanges();


            context.RepresentativeUsers.AddOrUpdate(ru => ru.Email,
                new RepresentativeUser
                {
                    Email = "user@gmail.com",
                    FirstName = "مهدی",
                    LastName = "سعیدی فر",
                    MobileNumber = "09131234567",
                    Password = EncryptionHelper.Encrypt("123user123", EncryptionHelper.Key),
                    NationalCode = "1234567890",
                    UniversityId = context.Universities.First(u => u.Name == "واحد نجف آباد").Id,
                    FatherName = "علی"
                },
                new RepresentativeUser
                {
                    Email = "ali@gmail.com",
                    FirstName = "علی",
                    LastName = "احمدی",
                    MobileNumber = "09131234567",
                    Password = EncryptionHelper.Encrypt("123user123", EncryptionHelper.Key),
                    NationalCode = "1234567790",
                    UniversityId = context.Universities.First(u => u.Name == "استان تهران").Id,
                    FatherName = "حسین"
                }
                );

            context.SaveChanges();

            context.Competitions.AddOrUpdate(c => c.Name, new Competition
            {
                Name = "مسابقات فوتسال سراسری دانشگاه ها",
                IsPrintCardActive = true,
                IsReadyActive = true,
                IsRegisterActive = true,
                ReadyStartDate = new DateTime(2014, 1, 1),
                ReadyEndDate = new DateTime(2017, 1, 1),
                RegisterStartDate = new DateTime(2014, 1, 1),
                RegisterEndDate = new DateTime(2017, 1, 1),
                PrintCardStartDate = new DateTime(2014, 1, 1),
                PrintCardEndDate = new DateTime(2017, 1, 1),
                LogoImage = "31883536-7021-4fd7-b4e0-3e35d1bee581.jpg",
                Rule = "لطفا قوانین مسابقات را رعایت کنید:",
                MaxCommonTechnicalStaffs = 5,
                Announcements = new List<Announcement>
                {
                    new Announcement
                    {
                        CreatedDate = DateTime.Now.AddDays(-1),
                        WebsiteText = "متن اطلاعیه شماره 1",
                        Title = "اطلاعیه شماره 1",
                    },
                    new Announcement
                    {
                        CreatedDate = DateTime.Now,
                        WebsiteText = "متن اطلاعیه شماره 2",
                        Title = "اطلاعیه شماره 2",
                    }
                },
                PresentedSports = new List<CompetitionSport>
                {
                    new CompetitionSport
                    {
                        Gender = Gender.Male,
                        Sport = context.Sports.FirstOrDefault(s => s.Name == "فوتسال"),
                        Rule = "قوانین فدراسیون را رعایت کنید",
                        MaxTechnicalStaff = 3,
                        MaxCompetitors = 22,
                        HasRule = true,
                        IsIndividual = false,
                    }
                },
            });

            context.SaveChanges();

            context.CompetitionRepresentativeUsers.AddOrUpdate(c => new { c.RepresentativeUserId, c.CompetitionId },
                new CompetitionRepresentativeUser
                {
                    CompetitionId = context.Competitions.First(c => c.Name == "مسابقات فوتسال سراسری دانشگاه ها").Id,
                    RepresentativeUserId = context.RepresentativeUsers.First(r => r.Email == "user@gmail.com").Id
                });

            context.SaveChanges();



            var participation = new Participation
            {
                RepresentativeUserId = context.RepresentativeUsers.First(r => r.Email == "user@gmail.com").Id,
                IsApproved = true,
                PresentedSportId = context.PresentedSports.First(ps => ps.Sport.Name == "فوتسال").Id,
                TeamColors = new List<TeamColor>
                {
                    new TeamColor {Name = "آبی"},
                    new TeamColor {Name = "سفید"}
                },
                Competitors = new List<Competitor>
                {
                    new Competitor
                    {
                        FirstName = "سینا",
                        LastName = "مسعودی",
                        UserImage = "1.jpg",
                        AzmoonConfirmationImage = "afc642f4-8acf-48a7-b739-c3452b005770.JPG",
                        InsuranceImage = "9554825a-1d3f-4af2-b068-4e3ca2bffbb6.jpg",
                        StudentCertificateImage = "1dee1ced-0834-4fd6-9bd6-13e01ee25661.jpg",
                        BirthDate = new DateTime(1990, 2, 2),
                        IsApproved = true,
                        Dorm = context.Dorms.FirstOrDefault(d => d.Name == "شهید بهشتی"),
                        DormNumber = "402",
                        FatherName = "علی",
                        StudyFieldId = context.StudyFields.First(s => s.Name == "برق").Id,
                        StudentNumber = "123434545",
                        StudyFieldDegreeId = context.StudyFieldDegrees.First(s => s.Name == "کارشناسی").Id,
                        InsuranceEndDate = new DateTime(2016, 2, 2),
                        InsuranceNumber = "213325445645",
                        NationalCode = "1234567890"
                    },
                    new Competitor
                    {
                        FirstName = "نوید",
                        LastName = "عباسی",
                        UserImage = "2.jpg",
                        AzmoonConfirmationImage = "afc642f4-8acf-48a7-b739-c3452b005770.JPG",
                        InsuranceImage = "9554825a-1d3f-4af2-b068-4e3ca2bffbb6.jpg",
                        StudentCertificateImage = "1dee1ced-0834-4fd6-9bd6-13e01ee25661.jpg",
                        BirthDate = new DateTime(1990, 2, 2),
                        IsApproved = true,
                        Dorm = context.Dorms.FirstOrDefault(d => d.Name == "شهید بهشتی"),
                        DormNumber = "402",
                        FatherName = "نقی",
                        StudyFieldId = context.StudyFields.First(s => s.Name == "کامپیوتر").Id,
                        StudentNumber = "123434545",
                        StudyFieldDegreeId = context.StudyFieldDegrees.First(s => s.Name == "کارشناسی").Id,
                        InsuranceEndDate = new DateTime(2016, 2, 2),
                        InsuranceNumber = "213325445645",
                        NationalCode = "1234567890",
                    }
                },
            };


            context.Participates.AddOrUpdate(p => new { p.PresentedSportId, p.RepresentativeUserId }, participation);

            context.SaveChanges();

            context.TechnicalStaves.AddOrUpdate(t => t.NationalCode,
                new TechnicalStaff
                {
                    FirstName = "ناصر",
                    LastName = "علیان",
                    NationalCode = "1232567890",
                    BirthDate = new DateTime(1980, 2, 1),
                    IsApproved = true,
                    FatherName = "اصغر",
                    DormNumber = "222",
                    DormId = context.Dorms.First(d => d.Name == "شهید بهشتی").Id,
                    Image = "e56d21f7-c7fc-4793-8326-58c6597b5f8d.jpg"
                },
                new TechnicalStaff
                {
                    FirstName = "محمد",
                    LastName = "حسینی",
                    NationalCode = "1134567890",
                    BirthDate = new DateTime(1980, 6, 1),
                    IsApproved = true,
                    FatherName = "علی",
                    DormNumber = "222",
                    DormId = context.Dorms.First(d => d.Name == "شهید بهشتی").Id,
                    Image = "4ee4e2c6-0f32-4618-94c9-9516362564e9.jpg"
                },
                new TechnicalStaff()
                {
                    FirstName = "بابک",
                    LastName = "حاج بابایی",
                    NationalCode = "1234656901",
                    BirthDate = new DateTime(1980, 6, 1),
                    IsApproved = false,
                    FatherName = "اکبر",
                    DormNumber = "222",
                    DormId = context.Dorms.First(d => d.Name == "شهید بهشتی").Id,
                    Image = "bb09929c-811e-41f7-9c78-6ca3b5586319.jpg",
                    Error = "کد ملی اشتباه است"
                }
                );

            context.SaveChanges();

            context.ParticipationTechnicalStaves.AddOrUpdate(pts => new { pts.ParticipationId, pts.TechnicalStaffId },
                new ParticipationTechnicalStaff
                {
                    ParticipationId = context.Participates.First(p => p.PresentedSportId == participation.PresentedSportId && p.RepresentativeUserId == participation.RepresentativeUserId).Id,
                    TechnicalStaffRoleId = context.TechnicalStaffRoles.First(t => t.Name == "سرمربی").Id,
                    TechnicalStaffId = context.TechnicalStaves.First(t => t.NationalCode == "1232567890").Id
                },
                new ParticipationTechnicalStaff
                {
                    ParticipationId = context.Participates.First(p => p.PresentedSportId == participation.PresentedSportId && p.RepresentativeUserId == participation.RepresentativeUserId).Id,
                    TechnicalStaffRoleId = context.TechnicalStaffRoles.First(t => t.Name == "سرپرست").Id,
                    TechnicalStaffId = context.TechnicalStaves.First(t => t.NationalCode == "1134567890").Id
                }
                );

            context.SaveChanges();


            context.CompetitionCommonTechnicalStaffs.AddOrUpdate(c => new { c.CompetitonId, c.RepresentativeUserId, c.TechnicalStaffId },
                new CompetitionCommonTechnicalStaff
                {
                    RepresentativeUserId = context.RepresentativeUsers.First(r => r.Email == "user@gmail.com").Id,
                    CompetitonId = context.Competitions.First(c => c.Name == "مسابقات فوتسال سراسری دانشگاه ها").Id,
                    TechnicalStaffRoleId = context.TechnicalStaffRoles.First(t => t.Name == "راننده").Id,
                    TechnicalStaffId = context.TechnicalStaves.First(t => t.NationalCode == "1234656901").Id
                });


        }
    }
}
