﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUse
{
    public class FaultContract
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string Details { get; set; }
    }
}
