using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iris.Web.IrisMembership
{
    public class IrisUser
    {
        public string UserName { get; set; }
        public IrisRole Role { get; set; }
    }
}