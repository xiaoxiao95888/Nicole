﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class ProductSettingModel
    {
        public ProductModel[] ProductModels { get; set; }
        public int CurrentPageIndex { get; set; }
        public int AllPage { get; set; }
    }
}