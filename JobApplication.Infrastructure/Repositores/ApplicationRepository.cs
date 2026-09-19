using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Prestance;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Repositores
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public  async Task CreateApplicationAsync(JobCandidateApplication jobCandidateApplication)
        {
            await _context.JobCandidateApplications.AddAsync(jobCandidateApplication);
        }

        public async Task<JobCandidateApplication?> GetApplicationByIdAsync(int id)
        {
            return await _context.JobCandidateApplications.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void CancelApplicationAsync()
        {

        }

    }
}
