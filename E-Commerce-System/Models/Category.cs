using System;
using System.Collections.Generic;

namespace E_Commerce_System.Models
{
    public partial class Category
    {
        public string CateId { get; set; }
        public string CateName { get; set; }
        public string ParentId { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
