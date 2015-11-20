using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Postal;

namespace PostalEmail
{
    public static class SendingMail
    {
        public static void Send()
        {
            var viewsPath = Path.GetFullPath(@"..\..\Views\Emails");

            var engines = new ViewEngineCollection();
            engines.Add(new FileSystemRazorViewEngine(viewsPath));

            var service = new EmailService(engines);

            MyEmail email = new MyEmail() { ViewName = "Test" };

            email.From = "aa.@aaa.com";
            email.To = "bbb@sss.com";
            // Will look for Test.cshtml or Test.vbhtml in Views directory.
            //email.Message = "Hello, non-asp.net world!";
            service.Send(email);
        }

        public class MyEmail : Email
        {
            public string From { get; set; }
            public string To { get; set; }

        }


    }
}
