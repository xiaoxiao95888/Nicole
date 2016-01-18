using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class EnquiryManagerModel
    {
        public EnquiryModel[] EnquiryModels { get; set; }
        public int CurrentPageIndex { get; set; }
        public int AllPage { get; set; }
    }
}