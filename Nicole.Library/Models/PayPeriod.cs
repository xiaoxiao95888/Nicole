﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    public class PayPeriod
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
    }
}
