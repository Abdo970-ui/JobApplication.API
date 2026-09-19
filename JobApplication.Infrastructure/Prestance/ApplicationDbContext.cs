using JobApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Prestance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<JobCandidateApplication> JobCandidateApplications { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }

    }
}
