using System;
using System.Collections.Generic;

namespace AL.Infrastructure.Persistance.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedUser { get; set; }
    }
}
