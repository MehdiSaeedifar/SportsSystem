using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAUNSportsSystem.Web.Controllers
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CaptchaInputText { get; set; }
    }
}