using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CasusRegistratie.Models
{
    public class Registration
    {
        public string Name { get; set; }
        public HttpPostedFileBase Photo { get; set; }
    }
}