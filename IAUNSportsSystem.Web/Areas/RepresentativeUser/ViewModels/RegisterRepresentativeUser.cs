using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace IAUNSportsSystem.Web.Areas.RepresentativeUser.ViewModels
{
    public class RegisterRepresentativeUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int UniversityId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}