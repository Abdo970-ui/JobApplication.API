using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Services
{
    public class ApplicationService 
    {
        private readonly IApplicationRepository _applicationRepo;

        public ApplicationService(IApplicationRepository applicationRepo)
        {
            _applicationRepo = applicationRepo;
        }

        public async Task<int> CreateApplicationAsync(CreateApplicationRequest requse,int candidateId)
        {
            var application = new JobCandidateApplication
            {
                JobId = requse.JobId,
                CandidateId = candidateId,
        
            };
            await _applicationRepo.CreateApplicationAsync(application);
            await _applicationRepo.SaveChangesAsync();
            return application.Id;
        }

        public async Task<string?> CancelApplicationAsync(int applicationId)
        {
            var application = await _applicationRepo.GetApplicationByIdAsync(applicationId);

            if (application is null)
                return "Application not found.";

            if (application.JobApplicationStatus != JobApplication.Domain.Enms.JobApplicationStatus.Applied)
                return $"Cannot cancel an application that is already '{application.JobApplicationStatus}'.";

            application.JobApplicationStatus = JobApplication.Domain.Enms.JobApplicationStatus.Cancelled;
            application.StatusUpdatedAt = DateTime.UtcNow;

            await _applicationRepo.SaveChangesAsync();
            return null; // null means success
        }
    }
}
