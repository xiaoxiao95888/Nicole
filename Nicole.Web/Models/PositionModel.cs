using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class PositionModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PositionModel Parent { get; set; }
        /// <summary>
        /// 当前人员
        /// </summary>
        public EmployeeModel CurrentEmployeeModel { get; set; }
    }
}