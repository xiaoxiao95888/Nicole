using System;

namespace Plastic
{
    public class CollectionResult
    {
        public string ConditionId { get; set; }
        public string ConditionName { get; set; }
        public string Value { get; set; }
        public int? CreateByCustomerId { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Url { get; set; }

        public string JsonDate
        {
            get
            {
                var date = string.Empty;
                if (CreateTime != null)
                {
                    date = CreateTime.ToString();
                }
                return date;
            }
        }
    }
}
