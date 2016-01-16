using System;
using System.Collections.Generic;

namespace Nicole.Web.Models
{
    public class LeftNavigationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public List<LeftNavigationModel> SubModels { get; set; }
    }
}