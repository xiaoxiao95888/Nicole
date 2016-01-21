using System;

namespace Nicole.Web.Models
{
    public class CustomerModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string TelNumber { get; set; }        
        /// <summary>
        /// 类型
        /// </summary>
        public CustomerTypeModel CustomerTypeModel { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// 所属人
        /// </summary>
        public EmployeeModel[] EmployeeModels { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public EmployeeModel EmployeeModel { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }

    }

}