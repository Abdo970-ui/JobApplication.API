using JobApplication.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.Application.Features.Jobs.Commandes.ReopenJob
{
    public class ReopenJobHandler : IRequestHandler<ReopenJobCommande, bool>
    {
        private readonly IJobRepository _jobRepository;

        public ReopenJobHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<bool> Handle(ReopenJobCommande request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.Get()
               .FirstOrDefaultAsync(j => j.Id == request.jobId);

            if (job == null)
                throw new KeyNotFoundException("Job not found");

            if (job.RecruiterId != request.currentRecruiterId)
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
