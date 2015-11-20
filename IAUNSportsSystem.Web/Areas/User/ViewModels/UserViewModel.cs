using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.Web.Areas.User.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}