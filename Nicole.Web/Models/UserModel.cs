using System;

namespace Nicole.Web.Models
{
    public class UserModel
    {
        public Guid AccountId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
    }
}