using JobApplication.Application.DTOs.Requste;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IJobRepository
    {
        Task InsertAsync(Job job);
        IQueryable<Job> Get();
        IEnumerable<Job> GetAllJobs();
        void DeleteJob(Job job);
        Task<string> UpdateJobAsync(int id, UpdateJobDto updateJobDto);
        Task SaveChangesAsync();
    }
}
