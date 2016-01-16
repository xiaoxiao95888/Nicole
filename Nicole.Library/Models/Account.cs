using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nicole.Library.Models
{
    public class Account : IDtStamped
    {
        public Guid Id { get; set; }        
        public string Password { get; set; }
        public virtual Employee Employee { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
