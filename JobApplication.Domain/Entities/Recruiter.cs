using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Domain.Entities
{
    public class Recruiter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property (اختياري بس مفيد)
        public ICollection<Job> Jobs { get; set; }
    }
}
