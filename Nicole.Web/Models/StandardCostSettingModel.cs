using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class StandardCostSettingModel
    {
        public StandardCostModel[] StandardCostModels { get; set; }
        public int CurrentPageIndex { get; set; }
        public int AllPage { get; set; }
    }
}