using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IAUNSportsSystem.Utilities;

namespace IAUNSportsSystem.Web.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required(AllowEmptyStrings = false),
        StringLength(30, MinimumLength = 3),
        RegularExpression(RegExes.PersianCharacters)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false),
        StringLength(40, MinimumLength = 2),
        RegularExpression(RegExes.PersianCharacters)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false), StringLength(30, MinimumLength = 3),
        RegularExpression(RegExes.PersianCharacters)]
        public string FatherName { get; set; }

        [Required(AllowEmptyStrings = false),
        EmailAddress()]
        public string Email { get; set; }

        [Required, StringLength(10, MinimumLength = 10)]
        public string NationalCode { get; set; }

        [Required, StringLength(30, MinimumLength = 6)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required, StringLength(11, MinimumLength = 11)]
        public string MobileNumber { get; set; }
        [Required]
        public int UniversityId { get; set; }
        [Required]
        public string CaptchaInputText { get; set; }
    }
}