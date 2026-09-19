using JobApplication.Application.DTOs.Requste;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IApplicationRepository
    {
        Task CreateApplicationAsync(JobCandidateApplication jobCandidateApplication);
        Task<JobCandidateApplication?> GetApplicationByIdAsync(int id);
        Task SaveChangesAsync();

    }
}
