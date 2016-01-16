using System;

namespace Nicole.Web.Models
{
    public class UserModel
    {
        public Guid AccountId { get; set; }
        public Guid EmpId { get; set; }
        public string Name { get; set; }
    }
}