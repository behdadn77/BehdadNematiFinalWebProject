﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.ViewModels
{
    public class SitePropertiesViewModel
    {
        public AdminInfo AdminInfo { get; set; }
        public List<string> roles { get; set; }
    }
    public class AdminInfo
    {
        public string adminname { get; set; }
        public string adminpassword { get; set; }
        public string adminrole { get; set; }
        public string adminphone { get; set; }
    }

}
