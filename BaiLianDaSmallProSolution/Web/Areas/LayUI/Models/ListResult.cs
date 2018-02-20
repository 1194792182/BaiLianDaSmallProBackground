﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.LayUI.Models
{
    public class ListResult
    {
        public int code { get; set; }

        public int count { get; set; }

        public string msg { get; set; }

        public IEnumerable<object> data { get; set; }
    }
}