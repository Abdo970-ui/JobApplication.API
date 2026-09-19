using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Prestance;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Repositores
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(Job job)
        {
           await _context.Jobs.AddAsync(job);
        }
        public IQueryable<Job> Get()
        {
            var jobs = _context.Jobs.AsQueryable();
            return jobs;
        }
        public void DeleteJob(Job job)
        {
             _context.Jobs.Remove(job);
        }
       
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Job> GetAllJobs()
        {
            return _context.Jobs.ToList();
        }

        public async Task<string> UpdateJobAsync(int id, UpdateJobDto updateJobDto)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                job.Title = updateJobDto.Title;
                job.Description = updateJobDto.Description;
                job.IsActive = updateJobDto.IsActive;
                _context.Jobs.Update(job);
                await _context.SaveChangesAsync();
                return "Job updated successfully.";
            }
            return "Job not found.";
        }
    }
}
