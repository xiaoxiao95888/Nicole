using System;
using System.Linq;

namespace Nicole.Web.Models
{
    public class ProductTypeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage { get; set; }
        /// <summary>
        /// 容量
        /// </summary>
        public string Capacity { get; set; }
        /// <summary>
        /// 脚距
        /// </summary>
        public string Pitch { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string Level { get; set; }
        public ProductModel[] ProductModels { get; set; }
        /// <summary>
        /// 料号集合
        /// </summary>
        public string PartNumbers
        {
            get
            {
                var str = string.Empty;
                if (ProductModels != null && ProductModels.Any())
                {
                    str = string.Join(",", ProductModels.Select(n => n.PartNumber));

                }
                return str;
            }
        }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}