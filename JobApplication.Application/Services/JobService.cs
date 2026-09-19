using JobApplication.Application.DTOs.Requste;
using JobApplication.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Services
{
    public class JobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<int> CreateJobAsync(CreateJobDto createJobDto)
        {
            var job = new Job
            {
                Title = createJobDto.Title,
                Description = createJobDto.Description,
                IsActive = true
            };
          await  _jobRepository.InsertAsync(job);
            await _jobRepository.SaveChangesAsync();

            return job.Id;
        }

       public Job GetJob(int id)
        {
            var job = _jobRepository.Get().FirstOrDefault(j => j.Id == id);
            return job;
        }

        public IEnumerable<Job> GetAllJobs()
        {
            var jobs = _jobRepository.GetAllJobs().ToList();
            return jobs;
        }
        public async Task UpdateJobAsync(int id, UpdateJobDto updateJobDto)
        {
            var job = _jobRepository.Get().FirstOrDefault(j => j.Id == id);
            if (job != null)
            {
                job.Title = updateJobDto.Title;
                job.Description = updateJobDto.Description;
                job.IsActive = updateJobDto.IsActive;
              await  _jobRepository.UpdateJobAsync(id, updateJobDto);
               await  _jobRepository.SaveChangesAsync();
            }
        }
        public async Task DeleteJob(int id)
        {
            var job = _jobRepository.Get().FirstOrDefault(j => j.Id == id);
            if (job != null)
            {
                _jobRepository.DeleteJob(job);
                await _jobRepository.SaveChangesAsync();
            }
        }
        public async Task<bool> CloseAsync(int jobId, int currentRecruiterId)
        {
            var job = await _jobRepository.Get()
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null)
                throw new KeyNotFoundException("Job not found");

            if (job.RecruiterId == null || job.RecruiterId != currentRecruiterId)
                throw new UnauthorizedAccessException("You are not the owner of this job");

            if (job.ClosedAt != null)
                throw new InvalidOperationException("Job is already closed");

            job.ClosedAt = DateTime.UtcNow;
            job.ClosedBy = currentRecruiterId;
            job.IsActive = false;

            await _jobRepository.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ReopenAsync(int jobId, int currentRecruiterId)
        {
            var job = await _jobRepository.Get()
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null)
                throw new KeyNotFoundException("Job not found");

            if (job.RecruiterId != currentRecruiterId)
                throw new UnauthorizedAccessException("You are not the owner of this job");

            if (job.ClosedAt == null)
                throw new InvalidOperationException("Job is already open");

            job.ClosedAt = null;
            job.ClosedBy = null;
            job.IsActive = true;

            await _jobRepository.SaveChangesAsync();
            return true;
        }
    }
}
