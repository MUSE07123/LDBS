using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LDBS.Models
{
    public class StaffSetting
    {
        public class DoSettingIn
        {
            public string StaffNumber { get; set; }
            public string UserName { get; set; }
            public string Titile { get; set; }
            public string UserPassword { get; set; }
            public string UserPassword2 { get; set; }
            public string Email { get; set; }
            public string Permission { get; set; }
        }

        public class DoSettingrOut
        {
            public string ErrMsg { get; set; }
            public string ResultMsg { get; set; }
        }

    } 
}